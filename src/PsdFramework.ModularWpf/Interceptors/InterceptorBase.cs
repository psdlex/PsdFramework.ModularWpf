namespace PsdFramework.ModularWpf.Interceptors;

public abstract class InterceptorBase<TInterceptableService> : IInterceptor<TInterceptableService>
    where TInterceptableService : class, IInterceptableService
{
    public abstract Task InterceptPreExecutionAsync();
    public abstract Task InterceptPostExecutionAsync();
}

public abstract class InterceptorBase<TInterceptableService, TContext> : InterceptorBase<TInterceptableService>, IInterceptor<TInterceptableService, TContext>
    where TInterceptableService : class, IInterceptableService<TContext>
{
    public sealed override Task InterceptPreExecutionAsync() => Task.CompletedTask;
    public sealed override Task InterceptPostExecutionAsync() => Task.CompletedTask;

    public abstract Task InterceptPreExecutionAsync(TContext context);
    public abstract Task InterceptPostExecutionAsync(TContext context);
}
