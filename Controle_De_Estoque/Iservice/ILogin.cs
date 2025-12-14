using Controle_De_Estoque.Models;
using Controle_De_Estoque.Models.Login;

public interface ILogin
{
    public Task<List<Login>> GetAll();
    public Task<Login> Create(Login login);
    public Task<LoginExecute> Logar(LoginExecute login);
}
