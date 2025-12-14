using Controle_De_Estoque.Data;
using Controle_De_Estoque.Models;
using Controle_De_Estoque.Models.Login;
using Microsoft.EntityFrameworkCore;

public class LoginRepository : ILogin
{
    private readonly Context _context;
    private readonly ISession _session;

    public LoginRepository(Context context, ISession session)
    {
        _context = context;
        _session = session;
    }

    public async Task<Login> Create(Login login)
    {
        await _context.Logins.AddAsync(login);
        await _context.SaveChangesAsync();
        return login;
    }

    public async Task<List<Login>> GetAll()
    {
        return await _context.Logins.ToListAsync();
    }

    public async Task<LoginExecute> Logar(LoginExecute login)
    {
        var usuario = await _context.Logins
            //.Include(x => x.UserConfig)
            //.Include(x => x.UserMeliToken)
            .FirstOrDefaultAsync(a => a.Email == login.Email && a.Password == login.Password);

        if (usuario != null)
        {
            _session.CreateSession(usuario);
            _session.CreateSessionToMeli(usuario);
            //login.Login = usuario;
        }

        return login;
    }

}
