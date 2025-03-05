using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using FinActions.Application.Categorias.Requests;
using FinActions.Application.Categorias.Responses;
using FinActions.Application.Base.Responses;
using FinActions.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FinActions.Domain.Categorias;
using FinActions.Application.Validations.Categoria;

namespace FinActions.Application.Categorias.Services;


public class CategoriaService : ICategoriaService
{
    private readonly FinActionsDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICategoriaValidator _validator;

    public CategoriaService(FinActionsDbContext context, IMapper mapper, ICategoriaValidator validator)
    {
        _mapper = mapper;
        _context = context;
        _validator = validator;
    }

    public async Task<Results<Ok<PagedResultDto<CategoriaResponseDto>>, ProblemHttpResult>> ObterCategorias(GetCategoriaRequestDto categoriaRequestDto, Guid userId)
    {
        _validator.ModelToValidate(categoriaRequestDto);
        var validationResult = _validator.ValidateModel(out bool isValid);
        if (!isValid)
            return TypedResults.Problem(validationResult);

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
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> ObterPorId(IdsCategoriaRequestDto idsCategoriaRequestDto)
    {
        var dbEntity = await _context.Categorias
                                .AsNoTracking()
                                .Where(x => x.UserId == idsCategoriaRequestDto.userId
                                        && x.Id == idsCategoriaRequestDto.id
                                        && !x.IsDeleted)
                                .FirstOrDefaultAsync();

        var validation = _validator.EntityToValidate(dbEntity);

        return TypedResults.Ok(_mapper.Map<Categoria, CategoriaResponseDto>(dbEntity));
    }
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> Insert(PostCategoriaRequestDto categoriaRequestDto)
    {
        var validationModel = _validator.ModelToValidate(categoriaRequestDto)
                                        .ValidateModel(out var isModelValid);

        if (!isModelValid)
            return TypedResults.Problem(validationModel);

        var mappedEntity = _mapper.Map<PostCategoriaRequestDto, Categoria>(categoriaRequestDto);

        var dbEntity = await _context.Categorias.FirstOrDefaultAsync(x => x.Nome == categoriaRequestDto.Nome
                                                    && x.UserId == categoriaRequestDto.userId);

        var validationEntity = _validator.EntityToValidate(dbEntity)
                                    .AddEntityValidation<Categoria>
                                    (
                                        x => x is not null && !x.IsDeleted,
                                        CategoriaValidatorConsts.ErroCategoriaJaExiste,
                                        nameof(CategoriaValidatorConsts.ErroCategoriaJaExiste),
                                        StatusCodes.Status400BadRequest
                                    )
                                    .ValidateEntity(out var isValidEntity);

        if (!isValidEntity)
            return TypedResults.Problem(validationEntity);

        var addedEntity = _context.Add(mappedEntity).Entity;
        await _context.SaveChangesAsync();
        var mappedResult = _mapper.Map<Categoria, CategoriaResponseDto>(addedEntity);

        return TypedResults.Ok(mappedResult);
    }

    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> Update(PostCategoriaRequestDto categoriaRequestDto, Guid id)
    {
        var validationModel = _validator.ModelToValidate(categoriaRequestDto)
                                        .ValidateModel(out var isModelValid);

        if (!isModelValid)
            return TypedResults.Problem(validationModel);

        var mappedEntity = _mapper.Map<PostCategoriaRequestDto, Categoria>(categoriaRequestDto);

        var dbEntity = await _context.Categorias.FindAsync(id, categoriaRequestDto.userId);

        var validationEntity = _validator.EntityToValidate(dbEntity)
                                    .AddEntityValidation<Categoria>
                                    (
                                        x => x is null || x.IsDeleted,
                                        CategoriaValidatorConsts.ErroNaoFoiEncontradoCategoria,
                                        nameof(CategoriaValidatorConsts.ErroNaoFoiEncontradoCategoria),
                                        StatusCodes.Status404NotFound
                                    )
                                    .ValidateEntity(out var isEntityValid);

        if (!isEntityValid)
            return TypedResults.Problem(validationEntity);

        dbEntity.Nome = categoriaRequestDto.Nome;

        var saveResults = await _context.SaveChangesAsync();

        var mappedResults = _mapper.Map<Categoria, CategoriaResponseDto>(dbEntity);

        return TypedResults.Ok(mappedResults);
    }

    public async Task<Results<NoContent, ProblemHttpResult>> Delete(IdsCategoriaRequestDto idsCategoriaRequestDto)
    {
        var dbEntity = await _context.Categorias.FindAsync(idsCategoriaRequestDto.id, idsCategoriaRequestDto.userId);

        var validationEntity = _validator.EntityToValidate(dbEntity)
                                    .AddEntityValidation<Categoria>
                                    (
                                        x => x is null || x.IsDeleted,
                                        CategoriaValidatorConsts.ErroNaoFoiEncontradoCategoria,
                                        nameof(CategoriaValidatorConsts.ErroNaoFoiEncontradoCategoria),
                                        StatusCodes.Status404NotFound
                                    )
                                    .ValidateEntity(out var isEntityValid);

        if (!isEntityValid)
            return TypedResults.Problem(validationEntity);

        dbEntity.IsDeleted = true;
        await _context.SaveChangesAsync();

        return TypedResults.NoContent();
    }
}
