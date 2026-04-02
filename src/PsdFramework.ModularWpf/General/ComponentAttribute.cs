namespace PsdFramework.ModularWpf.General;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public abstract class ComponentAttribute : Attribute
{
    public bool IsCached
    {
        get;
        init
        {
            field = value;
            IsCachingExplicitelyDefined = true;
        }
    }

    internal bool IsCachingExplicitelyDefined { get; private set; }
}