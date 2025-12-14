using Newtonsoft.Json;
namespace Controle_De_Estoque.Models;

public class MercadoLivre
{
    public string? id { get; set; }
    public string? title { get; set; }
    public string? domain_id { get; set; }
    public string? domain_name { get; set; }
    public string? category_id { get; set; }
    public string? category_name { get; set; }
    public int? available_quantity { get; set; }
    public List<Attributes> attributes { get; set; } = new List<Attributes>();
    public List<Variations> variations { get; set; } = new List<Variations>();

}
public class Attributes
{
public string id { get; set; }
public string name { get; set; }
public string value_id { get; set; }
public string value_name { get; set; }

}
public class Variations
{

    [JsonProperty("id")]
    public long variationsId { set; get; }
   

    [JsonProperty("available_quantity")]
    public int available_quantity { get; set; }

    [JsonProperty("price")]
    public decimal price { get; set; }

    public string? seller_custom_field { get; set; }
}

