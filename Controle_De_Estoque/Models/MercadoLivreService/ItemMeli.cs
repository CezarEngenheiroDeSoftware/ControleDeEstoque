namespace Controle_De_Estoque.Model
{
    public class ItemMeliUniq
    {
        public string? title { get; set; }
        public int available_quantity { get; set; }
        public string? status { get; set; }
        public decimal price { get; set; }

    }
    public class ItemMeli
    {
        public string? title { get; set; }
        public int available_quantity {  get; set; }
        public string? status { get; set; }
        public string? currency_id { get; set; } = "BRL";
        public decimal price { get; set; }
        public string? listing_type_id { get; set; } = "gold_special";
        public string? category_id { get; set; }
        public string? buying_mode { get; set; } = "buy_it_now";
        public string? description { get; set; }
        public string? condition { get; set; }
        public List<Variations>? variations { get; set; } = new List<Variations>();
        public List<Pictures> pictures { get; set; }
        public List<AttributeCombination>? attribute_combinations { get; set; }
        public List<AttributeEan>? attributes { get; set; }
        public List<SalleTermes> sale_terms { get; set; }
    }
    public class Variations
    {
        public long id { get; set; }
        public int available_quantity { get; set; }
        public string? seller_custom_field { get; set; }


    }
    public class AttributeCombination
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? value_id { get; set; }
        public string? value_name { get; set; }
    }
    public class AttributeEan
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? value_id { get; set; }
        public string? value_name { get; set; }
    }
    public class Pictures
    {
        public string source { get; set; }

    }
    public class SalleTermes
    {
        public string id { get; set; }
        public string value_name { get; set; } = "Garantía del vendedor";
    }
}
