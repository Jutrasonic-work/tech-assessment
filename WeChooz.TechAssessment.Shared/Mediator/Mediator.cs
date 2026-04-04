using Microsoft.Extensions.DependencyInjection;

namespace Shared.Mediator.Application;

public class Mediator(IServiceProvider provider) : IMediator
{

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
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResult));
        var handler = provider.GetService(handlerType)
            ?? throw new InvalidOperationException($"No handler registered for {requestType.Name}");
        var method = handlerType.GetMethod("HandleAsync")
            ?? throw new InvalidOperationException($"Handler for {requestType.Name} does not implement HandleAsync method");
        var result = await (Task<TResult>)method.Invoke(handler, [request, cancellationToken])!;

        return result;
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
