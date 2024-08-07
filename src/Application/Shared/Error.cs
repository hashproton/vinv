namespace Application.Shared;

public class Error
{
    public ErrorType Type { get; init; }

    public string Description { get; init; }

    private Error(ErrorType type, string description)
    {
        Type = type;
        Description = description;
    }

    public static Error Create(ErrorType type, string description) => new(type, description);
}