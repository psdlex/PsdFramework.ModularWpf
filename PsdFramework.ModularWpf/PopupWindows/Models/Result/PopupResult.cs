namespace PsdFramework.ModularWpf.PopupWindows.Models.Result;

public sealed record PopupResult<TResult>(
    PopupExitBasis ExitBasis,
    TResult? Value
);