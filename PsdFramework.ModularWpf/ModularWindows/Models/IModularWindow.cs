namespace PsdFramework.ModularWpf.ModularWindows.Models;

public interface IModularWindow
{
    object GetResult();
}

public interface IModularWindow<out TResult>
{
    TResult GetResult();
}