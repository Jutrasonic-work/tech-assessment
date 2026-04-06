namespace Shared.Mediator;

/// <summary>
/// A request that can be handled by a request handler, optionally returning a result.
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IRequest<TResult> { }

/// <summary>
/// A request that can be handled by a request handler without returning a result.
/// </summary>
public interface IRequest { }
