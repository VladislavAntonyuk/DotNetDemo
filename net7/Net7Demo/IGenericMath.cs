// DateTime
using System.Numerics;
// Generic math

internal interface IGenericMath
{
    public static TResult Sum<T, TResult>(IEnumerable<T> values)
            where T : INumber<T>
            where TResult : INumber<TResult>
    {
        TResult result = TResult.Zero;

        foreach (var value in values)
        {
            result += TResult.CreateChecked(value);
        }

        return result;
    }
}

public interface IGetNext<T> where T : IGetNext<T>
{
    static abstract T operator ++(T other);
}

public struct RepeatSequence : IGetNext<RepeatSequence>
{
    private const char Ch = 'A';
    public string Text = new string(Ch, 1);

    public RepeatSequence() { }

    public static RepeatSequence operator ++(RepeatSequence other)
        => other with { Text = other.Text + Ch };

    public override string ToString() => Text;
}