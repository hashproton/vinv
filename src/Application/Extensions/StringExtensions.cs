using System.Text;

namespace Application.Extensions;

public static class StringExtensions
{
    public static string ToPascalCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        var sb = new StringBuilder();
        sb.Append(str[0]);

        for (int i = 1; i < str.Length; i++)
        {
            if (char.IsUpper(str[i]))
            {
                sb.Append(' ');
            }
            sb.Append(str[i]);
        }

        return sb.ToString();
    }
}