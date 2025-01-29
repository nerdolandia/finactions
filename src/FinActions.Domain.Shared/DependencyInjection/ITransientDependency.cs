namespace FinActions.Domain.Shared.DependencyInjection;

/// <summary>
/// Interface para serviços do tipo transient, ou seja, que a cada chamada do método são criados. 
/// Implemente essa interface para registrar o seu serviço como transient no container de injeção de dependências do .NET
/// </summary>
public interface ITransientDependency;
