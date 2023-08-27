namespace Results.Tests;

public sealed record User;

public abstract record GetUserProblem : Problem
{
    public sealed record Missing : GetUserProblem;

    public sealed record Error : GetUserProblem;
}

public sealed class UserStore
{
    public IResult<User> GetUser(string id, bool fail)
    {
        if (fail)
        {
            return Result.FromProblem<User>(new GetUserProblem.Missing());
        }

        return Result.FromValue(new User());
    }
}

public abstract record LoginProblem : Problem
{
    public sealed record CredentialsMismatch : LoginProblem;

    public sealed record Generic : LoginProblem;
}

public sealed class LoginService
{
    public IResult<Nothing> Login(User user, bool fail)
    {
        if (fail)
        {
            return Result.FromProblem<Nothing>(new LoginProblem.CredentialsMismatch());
        }

        return Result.Nothing();
    }
}