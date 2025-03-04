using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using FinActions.Application.Categorias.Requests;
using FinActions.Application.Categorias.Responses;
using FinActions.Application.Base.Responses;
using FinActions.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FinActions.Domain.Categorias;

namespace FinActions.Application.Categorias.Services;

// TODO: Escrever mensagem de erros mais claras, sempre retornando o status code e as mensagens com o TypedResults.Problem
// Se basear no retorno da linha 61 da classe IdentityService
public class CategoriaService : ICategoriaService
{
    private readonly FinActionsDbContext _context;
    private readonly IMapper _mapper;

    public CategoriaService(FinActionsDbContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    // TODO: Validar se o tamanho do nome da categoria procurada não é maior que 150 caracteres
    public async Task<Results<Ok<PagedResultDto<CategoriaResponseDto>>, ProblemHttpResult>> ObterCategorias(GetCategoriaRequestDto categoriaRequestDto)
    {
        var query = _context.Categorias
                        .AsNoTracking()
                        .Where(x => x.UserId == categoriaRequestDto.UserId && !x.IsDeleted);

        if (!string.IsNullOrEmpty(categoriaRequestDto.Nome))
        {
            query = query.Where(x => x.Nome.Contains(categoriaRequestDto.Nome, StringComparison.CurrentCultureIgnoreCase));
        }

        var count = await query.Select(x => x.Id).CountAsync();
        var dbEntities = await query.ToListAsync();

        var result = new PagedResultDto<CategoriaResponseDto>
        {
            Items = _mapper.Map<List<Categoria>, List<CategoriaResponseDto>>(dbEntities),
            TotalCount = count
        };

        return TypedResults.Ok(result);
    }
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> ObterPorId(Guid categoriaId, Guid userId)
    {
        var dbEntity = await _context.Categorias
                                .AsNoTracking()
                                .Where(x => x.UserId == userId
                                        && !x.IsDeleted
                                        && x.Id == categoriaId)
                                .FirstOrDefaultAsync();

        if (dbEntity is null)
        {
            return TypedResults.Problem(statusCode: StatusCodes.Status404NotFound);
        }

        return TypedResults.Ok(_mapper.Map<Categoria, CategoriaResponseDto>(dbEntity));
    }
    // TODO: Validar se já existe uma categoria com esse nome, devido ao indíce uníco que criamos
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> Insert(PostCategoriaRequestDto categoriaRequestDto)
    {
        var mappedEntity = _mapper.Map<PostCategoriaRequestDto, Categoria>(categoriaRequestDto);

        var dbEntity = _context.Categorias.Add(mappedEntity).Entity;

        await _context.SaveChangesAsync();
        var mappedResult = _mapper.Map<Categoria, CategoriaResponseDto>(dbEntity);

        return TypedResults.Ok(mappedResult);
    }

    // TODO: Validar se já existe uma categoria com esse nome, devido ao indíce uníco que criamos, 
    // também é necessário validar se a entidade existe no banco, então vc não nvai poder dar map direto nela
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> Update(PostCategoriaRequestDto categoriaRequestDto)
    {
        var mappedEntity = _mapper.Map<PostCategoriaRequestDto, Categoria>(categoriaRequestDto);

        var dbEntity = _context.Categorias.Update(mappedEntity).Entity;

        await _context.SaveChangesAsync();

        var mappedResults = _mapper.Map<Categoria, CategoriaResponseDto>(dbEntity);

        return TypedResults.Ok(mappedResults);
    }

    public async Task<Results<NoContent, ProblemHttpResult>> Delete(Guid categoriaId, Guid userId)
    {
        var dbEntity = await _context.Categorias
                                .Where(x => x.UserId == userId
                                        && !x.IsDeleted
                                        && x.Id == categoriaId)
                                .FirstOrDefaultAsync();
        if (dbEntity is null)
        {
            return TypedResults.Problem(statusCode: StatusCodes.Status404NotFound);
        }

        dbEntity.IsDeleted = true;
        await _context.SaveChangesAsync();

        return TypedResults.NoContent();
    }
}
