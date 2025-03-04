using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using FinActions.Application.Categoria.Requests;
using FinActions.Application.Categoria.Responses;
using FinActions.Application.Base.Responses;
using FinActions.Application.Base.Services;
using FinActions.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CurrentDomain = FinActions.Domain.Categorias;

namespace FinActions.Application.Categoria.Services;
public class CategoriaService : ICategoriaService
{
    private readonly FinActionsDbContext _context;
    private readonly IMapper _mapper;

    public CategoriaService(FinActionsDbContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<Results<Ok<PagedResultDto<CategoriaResponseDto>>, ProblemHttpResult>> ObterCategorias(GetCategoriaRequestDto categoriaRequestDto)
    {
        var dbEntities = await _context.Categorias.AsNoTracking()
                                    .Where(x => x.UserId == categoriaRequestDto.UserId
                                                && !x.IsDeleted)
                                    .Skip(categoriaRequestDto.Skip)
                                    .Take(categoriaRequestDto.Take)
                                    .ToListAsync();

        var mappedResult = _mapper.Map<List<CurrentDomain.Categoria>, List<CategoriaResponseDto>>(dbEntities);

        var result = new PagedResultDto<CategoriaResponseDto> { Items = mappedResult, TotalCount = mappedResult.Count };

        return TypedResults.Ok(result);
    }
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> ObterPorId(Guid id)
    {
        var dbEntity = await _context.Categorias.FindAsync(id.ToString());

        var mappedResult = _mapper.Map<CurrentDomain.Categoria, CategoriaResponseDto>(dbEntity);

        return TypedResults.Ok(mappedResult);
    }
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> Insert(PostCategoriaRequestDto categoriaRequestDto)
    {
        var mappedEntity = _mapper.Map<PostCategoriaRequestDto, CurrentDomain.Categoria>(categoriaRequestDto);

        var dbEntity = _context.Categorias.Add(mappedEntity).Entity;

        var result = await _context.SaveChangesAsync();
        var mappedResult = _mapper.Map<CurrentDomain.Categoria, CategoriaResponseDto>(dbEntity);

        return TypedResults.Ok<CategoriaResponseDto>(mappedResult);
    }
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> Update(PostCategoriaRequestDto categoriaRequestDto)
    {
        var mappedEntity = _mapper.Map<PostCategoriaRequestDto, CurrentDomain.Categoria>(categoriaRequestDto);

        var dbEntity = _context.Categorias.Update(mappedEntity).Entity;

        var result = await _context.SaveChangesAsync();

        var mappedResults = _mapper.Map<CurrentDomain.Categoria, CategoriaResponseDto>(dbEntity);

        return TypedResults.Ok(mappedResults);
    }
    public async Task<Results<NoContent, ProblemHttpResult>> Delete(Guid id)
    {
        var dbEntity = _context.Categorias.Find(id);
        var dbResult = dbEntity.IsDeleted = true;

        var result = await _context.SaveChangesAsync();

        return TypedResults.NoContent();
    }
}
