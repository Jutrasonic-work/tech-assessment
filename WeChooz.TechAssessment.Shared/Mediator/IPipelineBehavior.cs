namespace Shared.Mediator;

/// <summary>
/// Handles the input and output of a request, allowing for pre- and post-processing logic.
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public interface IPipelineBehavior<in TInput, TOutput>
{
    /// <summary>
    /// Handles the input and output of a request, allowing for pre- and post-processing logic.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns> The output from the handler.
    /// </returns>
    Task<TOutput> HandleAsync(TInput input, Func<Task<TOutput>> next, CancellationToken cancellationToken = default);
}

/// <summary>
/// Handles the input of a request without returning a result, allowing for pre- and post-processing logic.
/// </summary>
/// <typeparam name="TInput"></typeparam>
public interface IPipelineBehavior<in TInput>
{
    /// <summary>
    /// Handles the input of a request without returning a result, allowing for pre- and post-processing logic.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task HandleAsync(TInput input, Func<Task> next, CancellationToken cancellationToken = default);
}