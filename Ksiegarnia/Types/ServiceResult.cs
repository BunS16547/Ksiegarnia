namespace Ksiegarnia.Types;

public record ServiceResult(bool Status, string? Field = null, string? ErrorMessage = null) {
    public static ServiceResult Success() => new(true);
    public static ServiceResult Fail(string field, string error) => new(false, field, error);
}