namespace MatValidator;

public readonly struct ValidResult
{
    private readonly List<string> _errors;

    public ValidResult(List<string> errors) => _errors = errors;
    public List<string> ErrorMessages => _errors;
    public bool IsValid => _errors.Count == 0;
}
