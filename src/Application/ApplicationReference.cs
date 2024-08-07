using System.Reflection;

namespace Application;

public static class ApplicationReference
{
    public static Assembly Assembly => typeof(ApplicationReference).Assembly;
}