// DateTime
using System.Text.RegularExpressions;
// regex
public class RegexBefore
{
    public Regex regex = new Regex(@"abc|def", RegexOptions.IgnoreCase);

    public bool Bar(string input)
    {
        return regex.IsMatch(input);
    }
}


public partial class RegexAfter
{
    public bool Bar(string input)
    {
        return Regex().IsMatch(input);
    }

    [GeneratedRegex("abc|def", RegexOptions.IgnoreCase, "en-UA")]
    private static partial Regex Regex();
}
