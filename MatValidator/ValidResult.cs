namespace MatValidator;

public readonly struct ValidResult(List<string> errorMessages)
{
    public List<string> ErrorMessages { get; } = errorMessages;
    public bool IsValid => ErrorMessages.Count == 0;
}
