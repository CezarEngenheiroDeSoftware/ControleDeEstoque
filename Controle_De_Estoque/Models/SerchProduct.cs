using Controle_De_Estoque.Model;
using Controle_De_Estoque.Models.Login;

namespace Controle_De_Estoque.Models
{
    public class SerchProduct
    {
        public string? title { get; set; }
        public string id { get; set; }
        public string? domain_id { get; set; }
        public string? domain_name { get;set; }
        public string? category_id { get;set; }
        public string? category_name{ get;set; }
        public int? available_quantity { get; set; }
        public List<AttributeEan>? attributes { get; set; }
        public List<Variations>? variations { get; set; }  = new List<Variations>();
        public UserMeliToken? userMeliToken { get; set; }
        public UserConfig? userConfig { get; set; }

    }
    public class AttriuteEan
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? value_id { get; set; }
        public string? value_name { get; set; }
    }
}
