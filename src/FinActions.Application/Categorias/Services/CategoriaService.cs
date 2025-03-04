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
    // validação de campos faço depois
    public async Task<Results<Ok<PagedResultDto<CategoriaResponseDto>>, ProblemHttpResult>> ObterCategorias(GetCategoriaRequestDto categoriaRequestDto, Guid userId)
    {
        var query = _context.Categorias
                        .AsNoTracking()
                        .Where(x => x.UserId == userId && !x.IsDeleted);

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
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> Insert(PostCategoriaRequestDto categoriaRequestDto, Guid userId)
    {
        var mappedEntity = _mapper.Map<PostCategoriaRequestDto, Categoria>(categoriaRequestDto);
        mappedEntity.UserId = userId;

        var dbEntity = await _context.Categorias.FirstOrDefaultAsync(x => x.Nome == categoriaRequestDto.Nome 
                                                    && x.UserId == userId);

        if (dbEntity is not null && !dbEntity.IsDeleted)
            return TypedResults.Problem(detail: "Este nome de categoria já existe", 
                                        title: "Erro ao inserir categoria", 
                                        statusCode: StatusCodes.Status400BadRequest);

        var addedEntity = _context.Add(mappedEntity).Entity;
        await _context.SaveChangesAsync();
        var mappedResult = _mapper.Map<Categoria, CategoriaResponseDto>(addedEntity);

        return TypedResults.Ok(mappedResult);
    }

    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> Update(PostCategoriaRequestDto categoriaRequestDto, Guid userId, Guid id)
    {
        var mappedEntity = _mapper.Map<PostCategoriaRequestDto, Categoria>(categoriaRequestDto);

        var dbEntity = await _context.Categorias.FindAsync(id, userId);

        if (dbEntity is null || dbEntity.IsDeleted)
            return TypedResults.Problem(detail: "Esta categoria não existe", 
                                        title: "Erro ao atualizar categoria", 
                                        statusCode: StatusCodes.Status404NotFound);

        dbEntity.Nome = categoriaRequestDto.Nome;

        var saveResults = await _context.SaveChangesAsync();

        var mappedResults = _mapper.Map<Categoria, CategoriaResponseDto>(dbEntity);

        return TypedResults.Ok(mappedResults);
    }

    public async Task<Results<NoContent, ProblemHttpResult>> Delete(Guid categoriaId, Guid userId)
    {
        var dbEntity = await _context.Categorias.FindAsync(categoriaId, userId);

        if (dbEntity is null || dbEntity.IsDeleted)
            return TypedResults.Problem(detail: "Esta categoria não existe", 
                                        title: "Erro ao excluir categoria", 
                                        statusCode: StatusCodes.Status404NotFound);

        dbEntity.IsDeleted = true;
        await _context.SaveChangesAsync();

        return TypedResults.NoContent();
    }
}
