namespace Results.Tests;

public class Tests
{
    [Test]
    public void Executes_chain()
    {
        var userStore = new UserStore();
        var loginService = new LoginService();

        userStore.GetUser("pippo", fail: false)
            .Chain(u => loginService.Login(u, fail: false))
            .Unwrap(_ => Assert.Pass(), _ => Assert.Fail());
    }

    [Test]
    public void Shorts_circuit_to_first_problem()
    {
        var userStore = new UserStore();
        var loginService = new LoginService();

        userStore.GetUser("pippo", fail: true)
            .Chain(u => loginService.Login(u, fail: false))
            .Unwrap(_ => Assert.Fail(), _ => Assert.Pass());
    }
}