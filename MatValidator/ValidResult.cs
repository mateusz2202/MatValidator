namespace MatValidator;

public record ValidResult(List<string> ErrorMessages)
{
    public bool IsValid => ErrorMessages.Count == 0;
}
