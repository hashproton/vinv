namespace Application.Shared
{
    public class Error
    {
        public ErrorType Type { get; set; }

        public string Description { get; set; }

        private Error(ErrorType type, string description)
        {
            Type = type;
            Description = description;
        }

        public static Error Create(ErrorType type, string description) => new Error(type, description);
    }

}
