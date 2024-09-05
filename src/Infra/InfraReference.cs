using System.Reflection;

namespace Infra;

public static class InfraReference
{
    public static Assembly Assembly => typeof(InfraReference).Assembly;
}