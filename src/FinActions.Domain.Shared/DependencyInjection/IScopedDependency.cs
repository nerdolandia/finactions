namespace FinActions.Domain.Shared.DependencyInjection;

/// <summary>
/// Interface para serviços do tipo scoped, ou seja, que será compartilhado se houveram mais de uma chamada ao mesmo tempo.  
/// Implemente essa interface para registrar o seu serviço como scoped no container de injeção de dependências do .NET
/// </summary>
public interface IScopedDependency;
