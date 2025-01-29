namespace FinActions.Domain.Shared.DependencyInjection;

/// <summary>
/// Interface para serviços do tipo singleton, ou seja, a instância será mesma compartilhada em todas as chamadas  
/// durante toda a execução da aplicação.  
/// Implemente essa interface para registrar o seu serviço como singleton no container de injeção de dependências do .NET
/// </summary>
public interface ISingletonDependency;
