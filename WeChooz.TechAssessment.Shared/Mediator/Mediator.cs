using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Mediator.Application;

public class Mediator(IServiceProvider provider) : IMediator
{
    private static readonly MethodInfo SendWithPipelineDefinition =
        typeof(Mediator).GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(m => m.Name == nameof(SendAsync)
                && m.IsGenericMethodDefinition
                && m.GetGenericArguments().Length == 2);

    public async Task<TResult> SendAsync<TRequest, TResult>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest<TResult>
    {
        var handler = provider.GetService<IRequestHandler<TRequest, TResult>>()
            ?? throw new InvalidOperationException($"No handler registered for {typeof(TRequest).Name}");
        var behaviors = provider.GetServices<IPipelineBehavior<TRequest, TResult>>().Reverse();
        var handlerDelegate = () => handler.HandleAsync(request, cancellationToken);
        foreach (var behavior in behaviors)
        {
            var next = handlerDelegate;
            handlerDelegate = () => behavior.HandleAsync(request, next, cancellationToken);
        }
        return await handlerDelegate();
    }

    public async Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var closed = SendWithPipelineDefinition.MakeGenericMethod(requestType, typeof(TResult));
        var task = (Task)closed.Invoke(this, [request, cancellationToken])!;
        await task.ConfigureAwait(false);
        return (TResult)task.GetType().GetProperty("Result", BindingFlags.Public | BindingFlags.Instance)!.GetValue(task)!;
    }

    public async Task SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        var handler = provider.GetService<IRequestHandler<TRequest>>()
            ?? throw new InvalidOperationException($"No handler registered for {typeof(TRequest).Name}");
        var behaviors = provider.GetServices<IPipelineBehavior<TRequest>>().Reverse();
        var handlerDelegate = () => handler.HandleAsync(request, cancellationToken);
        foreach (var behavior in behaviors)
        {
            var next = handlerDelegate;
            handlerDelegate = () => behavior.HandleAsync(request, next, cancellationToken);
        }
        await handlerDelegate();
    }
}
