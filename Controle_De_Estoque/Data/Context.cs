using Controle_De_Estoque.Model;
using Controle_De_Estoque.Models;
using Controle_De_Estoque.Models;
using Controle_De_Estoque.Models.Login;
using Microsoft.EntityFrameworkCore;

namespace Controle_De_Estoque.Data
{
    public class Context:DbContext
    {
        public Context(DbContextOptions<Context> options):base(options) { }
        public DbSet<EstoquePrincipal> Estoque { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<UserConfig> UserConfig { get; set; }
        public DbSet<UserMeliToken> UserMeliToken { get; set; }
        

    }

}
