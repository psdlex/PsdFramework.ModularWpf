namespace PsdFramework.ModularWpf.Interceptors;

internal static class InterceptorHelper
{
    public enum InterceptionPhase { PreExecution, PostExecution };

    public static async Task InterceptAllAsync<TInterceptableService, TContext>(
        IEnumerable<IInterceptor<TInterceptableService>> interceptors,
        TContext context,
        InterceptionPhase phase)
        where TInterceptableService : class, IInterceptableService<TContext>
    {
        foreach (var interceptor in interceptors)
        {
            if (interceptor is IInterceptor<TInterceptableService, TContext> interceptorWithContext)
            {
                await (phase switch
                {
                    InterceptionPhase.PreExecution => interceptorWithContext.InterceptPreExecutionAsync(context),
                    InterceptionPhase.PostExecution => interceptorWithContext.InterceptPostExecutionAsync(context),
                    _ => throw new ArgumentException("Invalid interception phase.", nameof(phase))
                });
            }

            else
            {
                await (phase switch
                {
                    InterceptionPhase.PreExecution => interceptor.InterceptPreExecutionAsync(),
                    InterceptionPhase.PostExecution => interceptor.InterceptPostExecutionAsync(),
                    _ => throw new ArgumentException("Invalid interception phase.", nameof(phase))
                });
            }
        }
    }

    public static async Task InterceptAllAsync<TInterceptableService>(
        IEnumerable<IInterceptor<TInterceptableService>> interceptors,
        InterceptionPhase phase)
        where TInterceptableService : class, IInterceptableService
    {
        foreach (var interceptor in interceptors)
        {
            await (phase switch
            {
                InterceptionPhase.PreExecution => interceptor.InterceptPreExecutionAsync(),
                InterceptionPhase.PostExecution => interceptor.InterceptPostExecutionAsync(),
                _ => throw new ArgumentException("Invalid interception phase.", nameof(phase))
            });
        }
    }
}
