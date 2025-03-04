using AutoMapper;
using FinActions.Application.Base.Responses;
using FinActions.Application.Movimentacoes.Contracts.Requests;
using FinActions.Application.Movimentacoes.Contracts.Responses;
using FinActions.Application.Movimentacoes.Services;
using FinActions.Domain.Movimentacoes;
using FinActions.Domain.Shared.ContasBancarias;
using FinActions.Domain.Shared.Movimentacoes;
using FinActions.Infrastructure.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinActions.Application.Movimentacoes.Services;

public sealed class MovimentacaoService : IMovimentacaoService
{
    private readonly FinActionsDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<MovimentacaoService> _logger;

    public MovimentacaoService(
        ILogger<MovimentacaoService> logger,
        FinActionsDbContext context,
        IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    public async Task<Results<NoContent, ProblemHttpResult>> Delete(Guid MovimentacaoId, Guid userId)
    {
        var Movimentacao = await _context.Movimentacoes
                                    .FirstOrDefaultAsync(x => x.UserId == userId
                                            && !x.IsDeleted
                                            && x.Id == MovimentacaoId);

        if (Movimentacao is null)
        {
            return TypedResults.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: MovimentacaoConsts.ErroMovimentacaoNaoEncontrada,
                    type: nameof(MovimentacaoConsts.ErroMovimentacaoNaoEncontrada));
        }

        Movimentacao.IsDeleted = true;
        await _context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    public async Task<Results<Ok<MovimentacaoResponseDto>, ProblemHttpResult>> Insert(
        PostPutMovimentacaoRequestDto insertRequest,
        Guid userId)
    {
        if (string.IsNullOrEmpty(insertRequest.Descricao))
        {
            return TypedResults.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: MovimentacaoConsts.ErroMovimentacaoDescricaoVazio,
                    type: nameof(MovimentacaoConsts.ErroMovimentacaoDescricaoVazio));
        }
        if (insertRequest.Descricao.Length > MovimentacaoConsts.DescricaoMaxLength)
        {
            return TypedResults.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: MovimentacaoConsts.ErroMovimentacaoNomeTamanhoMax,
                    type: nameof(MovimentacaoConsts.ErroMovimentacaoNomeTamanhoMax));
        }
        if (insertRequest.ValorMovimentado <= 0 || insertRequest.ValorMovimentado > decimal.MaxValue)
        {
            return TypedResults.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: MovimentacaoConsts.ErroValorMovimentadoInvalido,
                    type: nameof(MovimentacaoConsts.ErroValorMovimentadoInvalido));
        }
        if (insertRequest.DataMovimentacao > DateTimeOffset.MaxValue
            || insertRequest.DataMovimentacao < DateTimeOffset.MinValue)
        {
            return TypedResults.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: MovimentacaoConsts.ErroDataMovimentacaoInvalida,
                    type: nameof(MovimentacaoConsts.ErroDataMovimentacaoInvalida));
        }
        if (insertRequest.ContaBancariaId == Guid.Empty
            || !await _context.ContasBancarias.AnyAsync(x => x.Id == insertRequest.ContaBancariaId))
        {
            return TypedResults.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: ContaBancariaConsts.ErroContaBancariaNaoEncontrada,
                    type: nameof(ContaBancariaConsts.ErroContaBancariaNaoEncontrada));
        }
        if (insertRequest.CategoriaId == Guid.Empty
            || !await _context.Categorias.AnyAsync(x => x.Id == insertRequest.CategoriaId))
        {
            return TypedResults.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Categoria não existe",
                    type: "ErroCategoriaNaoEncontrada");
        }

        var movimentacao = _mapper.Map<PostPutMovimentacaoRequestDto, Movimentacao>(insertRequest);
        movimentacao.UserId = userId;
        var movimentacaoDb = (await _context.Movimentacoes.AddAsync(movimentacao)).Entity;
        await _context.SaveChangesAsync();

        return TypedResults.Ok(_mapper.Map<Movimentacao, MovimentacaoResponseDto>(movimentacaoDb));
    }

    public async Task<Results<Ok<PagedResultDto<MovimentacaoResponseDto>>, ProblemHttpResult>> ObterMovimentacaos(
        GetMovimentacaoRequestDto getRequest,
        Guid userId)
    {
        var query = _context.Movimentacoes
                        .AsNoTrackingWithIdentityResolution()
                        .Include(x => x.ContaBancaria)
                        .Include(x => x.Categoria)
                        .Where(x => x.UserId == userId && !x.IsDeleted);

        if (getRequest.TipoMovimentacao.HasValue)
        {
            query = query.Where(x => x.TipoMovimentacao == getRequest.TipoMovimentacao);
        }
        if (!string.IsNullOrEmpty(getRequest.Descricao))
        {
            query = query.Where(x => x.Descricao.ToUpper().Contains(getRequest.Descricao.ToUpper()));
        }
        if (!string.IsNullOrEmpty(getRequest.Tag))
        {
            query = query.Where(x => x.Tag.ToUpper().Contains(getRequest.Tag.ToUpper()));
        }
        if (getRequest.DataMovimentacaoInicio.HasValue)
        {
            query = query.Where(x => x.DataMovimentacao >= getRequest.DataMovimentacaoInicio);
        }
        if (getRequest.DataMovimentacaoFim.HasValue)
        {
            query = query.Where(x => x.DataMovimentacao <= getRequest.DataMovimentacaoFim);
        }
        if (getRequest.ContaBancariaId.HasValue)
        {
            query = query.Where(x => x.ContaBancariaId == getRequest.ContaBancariaId);
        }
        if (getRequest.CategoriaId.HasValue)
        {
            query = query.Where(x => x.CategoriaId == getRequest.CategoriaId);
        }

        var count = await query.Select(x => x.Id).CountAsync();
        var dbEntities = await query.Skip(getRequest.Skip).Take(getRequest.Take).ToListAsync();

        var result = new PagedResultDto<MovimentacaoResponseDto>
        {
            Items = _mapper.Map<List<Movimentacao>, List<MovimentacaoResponseDto>>(dbEntities),
            TotalCount = count
        };

        return TypedResults.Ok(result);
    }

    public async Task<Results<Ok<MovimentacaoResponseDto>, ProblemHttpResult>> ObterPorId(
        Guid movimentacaoId,
        Guid userId)
    {
        var Movimentacao = await _context.Movimentacoes
                                    .AsNoTrackingWithIdentityResolution()
                                    .Include(x => x.Categoria)
                                    .Include(x => x.ContaBancaria)
                                    .Where(x => x.UserId == userId
                                            && !x.IsDeleted
                                            && x.Id == movimentacaoId)
                                    .FirstOrDefaultAsync();

        if (Movimentacao is null)
        {
            return TypedResults.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: MovimentacaoConsts.ErroMovimentacaoNaoEncontrada,
                type: nameof(MovimentacaoConsts.ErroMovimentacaoNaoEncontrada));
        }

        return TypedResults.Ok(_mapper.Map<Movimentacao, MovimentacaoResponseDto>(Movimentacao));
    }

    public async Task<Results<Ok<MovimentacaoResponseDto>, ProblemHttpResult>> Update(
        Guid MovimentacaoId,
        Guid userId,
        PostPutMovimentacaoRequestDto updateRequest)
    {
        var movimentacao = await _context.Movimentacoes
                                    .Where(x => x.UserId == userId
                                            && !x.IsDeleted
                                            && x.Id == MovimentacaoId)
                                    .FirstOrDefaultAsync();
        
        if (movimentacao is null)
        {
            return TypedResults.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: MovimentacaoConsts.ErroMovimentacaoNaoEncontrada,
                    type: nameof(MovimentacaoConsts.ErroMovimentacaoNaoEncontrada));
        }
        if (string.IsNullOrEmpty(updateRequest.Descricao))
        {
            return TypedResults.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: MovimentacaoConsts.ErroMovimentacaoDescricaoVazio,
                    type: nameof(MovimentacaoConsts.ErroMovimentacaoDescricaoVazio));
        }
        if (updateRequest.Descricao.Length > MovimentacaoConsts.DescricaoMaxLength)
        {
            return TypedResults.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: MovimentacaoConsts.ErroMovimentacaoNomeTamanhoMax,
                    type: nameof(MovimentacaoConsts.ErroMovimentacaoNomeTamanhoMax));
        }
        if (updateRequest.ValorMovimentado <= 0 || updateRequest.ValorMovimentado > decimal.MaxValue)
        {
            return TypedResults.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: MovimentacaoConsts.ErroValorMovimentadoInvalido,
                    type: nameof(MovimentacaoConsts.ErroValorMovimentadoInvalido));
        }
        if (updateRequest.DataMovimentacao > DateTimeOffset.MaxValue
            || updateRequest.DataMovimentacao < DateTimeOffset.MinValue)
        {
            return TypedResults.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: MovimentacaoConsts.ErroDataMovimentacaoInvalida,
                    type: nameof(MovimentacaoConsts.ErroDataMovimentacaoInvalida));
        }
        if (updateRequest.ContaBancariaId == Guid.Empty
            || !await _context.ContasBancarias.AnyAsync(x => x.Id == updateRequest.ContaBancariaId))
        {
            return TypedResults.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: ContaBancariaConsts.ErroContaBancariaNaoEncontrada,
                    type: nameof(ContaBancariaConsts.ErroContaBancariaNaoEncontrada));
        }
        if (updateRequest.CategoriaId == Guid.Empty
            || !await _context.Categorias.AnyAsync(x => x.Id == updateRequest.CategoriaId))
        {
            return TypedResults.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Categoria não existe",
                    type: "ErroCategoriaNaoEncontrada");
        }

        movimentacao.TipoMovimentacao = updateRequest.TipoMovimentacao;
        movimentacao.Descricao = updateRequest.Descricao;
        movimentacao.Tag = updateRequest.Tag;
        movimentacao.Cor = updateRequest.Cor;
        movimentacao.ValorMovimentado = updateRequest.ValorMovimentado;
        movimentacao.DataMovimentacao = updateRequest.DataMovimentacao;
        movimentacao.ContaBancariaId = updateRequest.ContaBancariaId;
        movimentacao.CategoriaId = updateRequest.CategoriaId;
        await _context.SaveChangesAsync();

        return TypedResults.Ok(_mapper.Map<Movimentacao, MovimentacaoResponseDto>(movimentacao));
    }

}
