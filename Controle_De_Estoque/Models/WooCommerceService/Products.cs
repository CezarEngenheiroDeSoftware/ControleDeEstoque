namespace Controle_De_Estoque.Models
{
    public class Products
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? permalink { get; set; }
        public int? stock_quantity { get; set; }
        public string? sku { get; set; }
        public DateTime date_created { get; set; }
        public List<AttributeWoo>? attributes { get; set; }
        public List<int> variations { get; set; }
        public List<Products> variationsProducts { get; set; } = new List<Products>();

    }
    public class AttributeWoo
    {
        public int id { get; set; }
        public string? name { get; set; }
        public bool variation { get; set; }
        public List<string>? options { get; set; }
    }
}
