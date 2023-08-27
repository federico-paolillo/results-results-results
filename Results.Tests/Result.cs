namespace Results.Tests;

/// <summary>
/// A no-value value
/// </summary>
public sealed record Nothing;

/// <summary>
/// Some kind of problem
/// </summary>
public abstract record Problem;

public interface IResult<TValue>
{
    IResult<TNewValue> Chain<TNewValue>(Func<TValue, IResult<TNewValue>> continuation);

    void Unwrap(Action<TValue> happyPath, Action<Problem> miserablePath);
}

public sealed class HappyPath<TValue> : IResult<TValue>
{
    private readonly TValue _value;

    public HappyPath(TValue value)
    {
        _value = value;
    }

    public IResult<TNewValue> Chain<TNewValue>(Func<TValue, IResult<TNewValue>> continuation)
    {
        return continuation(_value);
    }

    public void Unwrap(Action<TValue> happyPath, Action<Problem> miserablePath)
    {
        happyPath(_value);
    }
}

public sealed class MiserablePath<TValue> : IResult<TValue>
{
    private readonly Problem _problem;

    public MiserablePath(Problem problem)
    {
        _problem = problem;
    }

    public IResult<TNewValue> Chain<TNewValue>(Func<TValue, IResult<TNewValue>> continuation)
    {
        return Result.FromProblem<TNewValue>(_problem);
    }

    public void Unwrap(Action<TValue> happyPath, Action<Problem> miserablePath)
    {
        miserablePath(_problem);
    }
}

public static class Result
{
    public static IResult<TValue> FromProblem<TValue>(Problem problem)
    {
        return new MiserablePath<TValue>(problem);
    }

    public static IResult<TValue> FromValue<TValue>(TValue value)
    {
        return new HappyPath<TValue>(value);
    }

    public static IResult<Nothing> Nothing()
    {
        return new HappyPath<Nothing>(new Nothing());
    }
}