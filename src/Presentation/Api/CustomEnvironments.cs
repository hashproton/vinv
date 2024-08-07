namespace Presentation.Api;

public static class CustomEnvironments
{
    public const string Test = "Test";
    
    public static bool IsTest(this IHostEnvironment env)
    {
        return env.IsEnvironment(Test);
    }
}