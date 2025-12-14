using Controle_De_Estoque.Models;
using Controle_De_Estoque.Models.Login;

public interface ISession
{
    public Task<Login> LoginAsync(LoginExecute login);
    public void CreateSession(Login login);
    public void CreateSessionToMeli(Login login);
    public Login? GetSession();
    public Login? GetSessionToMeli();

    public void RemoveSession(Login login);
}
