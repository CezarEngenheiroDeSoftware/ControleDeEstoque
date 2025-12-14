using Controle_De_Estoque.Models.Login;
using Microsoft.EntityFrameworkCore;
using Controle_De_Estoque.Data;

namespace Controle_De_Estoque.Service
{
    public class ServiceConfigurations
    {
        private readonly Context _context;
        private readonly ISession _session;
        public ServiceConfigurations(Context context, ISession session)
        {
            _context = context;
            _session = session;
        }
        public async Task<UserConfig> BuscarPorSessao()
        {
            var sessao = _session.GetSession();
            var usuario = await _context.Logins.FirstOrDefaultAsync(a => a.Id == sessao.UserConfig.LoginId);
            var configurationsUser = new UserConfig
            {
                LoginId = usuario.Id,
                MeliClientId = usuario.UserConfig.MeliClientId,
                MeliClientSecret = usuario.UserConfig.MeliClientSecret,
                redirect_uri = usuario.UserConfig.redirect_uri,
                MeliRefreshToken = usuario.UserConfig.MeliRefreshToken,
                WooUrl = usuario.UserConfig.WooUrl,
                WooConsumerKey = usuario.UserConfig.WooConsumerKey,
                WooConsumerSecret = usuario.UserConfig.WooConsumerSecret,
                Login = usuario,
            };
            return configurationsUser;
        }
    }
}
