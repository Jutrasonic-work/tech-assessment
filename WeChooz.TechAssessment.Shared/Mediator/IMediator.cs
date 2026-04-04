namespace Shared.Mediator.Application;

/// <summary>
/// Mediator interface for sending requests to their respective handlers.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Sends a request to the appropriate handler and returns a result.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns> The result from the handler.
    /// </returns>
    Task<TResult> SendAsync<TRequest, TResult>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest<TResult>;

    /// <summary>
    /// Sends a request to the appropriate handler and returns a result.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns> The result from the handler.
    /// </returns>
    Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a request to the appropriate handler without returning a result.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns> A task that represents the asynchronous operation.
    /// </returns>
    Task SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest;
}
