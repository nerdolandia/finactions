using FinActions.Domain.Shared.DependencyInjection;

namespace FinActions.Application.Data;

/// <summary>
/// A interface <see cref="IFinActionsDbMigrationService"/> é responsável por aplicar as migrations no banco de dados 
/// e, em seguida, chamar o <see cref="IDataSeeder"/> para garantir que os dados essenciais sejam semeados automaticamente.
/// </summary>
/// <remarks>
/// <para>
/// Por que não usar a CLI do EF Core diretamente para aplicar as migrations? Quando você executa as migrations através da CLI 
/// (usando o comando <c>dotnet ef database update</c>), o EF Core apenas atualiza a estrutura do banco de dados, mas não se preocupa 
/// em inserir dados fixos necessários, como valores de <c>enum</c> ou tabelas de referência essenciais. Isso pode deixar seu banco de dados 
/// atualizado, mas sem os dados necessários para o funcionamento correto da aplicação.
/// </para>
/// <para>
/// O <see cref="IFinActionsDbMigrationService"/> aplica as migrations e em seguida chama o <see cref="IDataSeeder"/> para semear 
/// os dados essenciais, garantindo que o banco de dados esteja não só atualizado, mas também corretamente populado.
/// </para>
/// </remarks>

public interface IFinActionsDbMigrationService : ITransientDependency
{
    Task MigrateAsync(CancellationToken cancellationToken);
}
