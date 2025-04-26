namespace MatValidator;

public readonly struct ValidResult
{
    private readonly string[] _errors;

    public ValidResult(string[] errors) => _errors = errors;
    public string[] ErrorMessages => _errors;
    public bool IsValid => _errors.Length == 0;
}
