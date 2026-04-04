namespace Shared.Mediator.Application;

/// <summary>
/// Handles a request and returns a result.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IRequestHandler<in TRequest, TResult> where TRequest : IRequest<TResult>
{
    /// <summary>
    /// Handles the request and returns a result.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns> The result from the handler.
    /// </returns>
    Task<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Handles a request without returning a result.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
public interface IRequestHandler<in TRequest> where TRequest : IRequest
{
    /// <summary>
    /// Handles the request without returning a result.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}