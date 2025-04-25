namespace MatValidator;

public readonly ref struct ValidResult
{
    private readonly ReadOnlySpan<string> _errors;

    public ValidResult(ReadOnlySpan<string> errors)
    {
        _errors = errors;
    }
    public ReadOnlySpan<string> ErrorMessages => _errors;
    public bool IsValid => _errors.Length == 0;
}
