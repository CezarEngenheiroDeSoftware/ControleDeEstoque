using System.ComponentModel.DataAnnotations;

namespace Controle_De_Estoque.Model
{
    public class AcessToken
    {
        [Key]
        public int Id { get; set; }
        public string? access_token {  get; set; }
        public string? token_type { get; set; }
        public int? expires_in { get; set; }
        public long? user_id { get; set; }
        public string? refresh_token { get; set; }
        public int Expiret_At { get; set; }

    }
}
