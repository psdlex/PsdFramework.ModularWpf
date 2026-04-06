using System.Runtime.ExceptionServices;
using System.Windows.Threading;

namespace PsdFramework.ModularWpf.Tests.Unit;

public static class TestHelper
{
    public static void ExecuteOnStaThread(Action action)
    {
        Exception? exception = null;
        var thread = new Thread(() =>
        {
            try { action(); }
            catch (Exception ex) { exception = ex; }
        });

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        if (exception is not null)
            ExceptionDispatchInfo.Capture(exception).Throw();
    }

    public static void ExecuteOnStaThread(Func<Task> func)
    {
        Exception? exception = null;
        var thread = new Thread(() =>
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(dispatcher));

            dispatcher.InvokeAsync(async () =>
            {
                try { await func(); }
                catch (Exception ex) { exception = ex; }
                finally { dispatcher.BeginInvokeShutdown(DispatcherPriority.Background); }
            });

            Dispatcher.Run();
        });

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        if (exception is not null)
            ExceptionDispatchInfo.Capture(exception).Throw();
    }
}
