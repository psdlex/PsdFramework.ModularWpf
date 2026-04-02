namespace PsdFramework.ModularWpf.Popup.Models.Result;

public sealed record PopupResult<TResult>(
    PopupExitBasis ExitBasis,
    TResult? Value
);