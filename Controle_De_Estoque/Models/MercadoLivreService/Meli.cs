namespace Controle_De_Estoque.Model
{
    public class Meli
    {
        public const string NomeDaSecao = "Meli";
        public string client_id {  get; set; }
        public string client_secret { get; set; }
        public string redirect_uri { get; set; }
        public string token { get; set; }
        public string refreshtoken { get; set; }
        public DateTime Expire_in { get; set; }
    }
}
