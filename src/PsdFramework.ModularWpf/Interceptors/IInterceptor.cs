namespace PsdFramework.ModularWpf.Interceptors;

public interface IInterceptor<TInterceptableService>
    where TInterceptableService : class, IInterceptableService
{
    Task InterceptPreExecutionAsync();
    Task InterceptPostExecutionAsync();
}

public interface IInterceptor<TInterceptableService, TContext> : IInterceptor<TInterceptableService>
    where TInterceptableService : class, IInterceptableService<TContext>
{
    Task InterceptPreExecutionAsync(TContext context);
    Task InterceptPostExecutionAsync(TContext context);
}