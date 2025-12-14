namespace Controle_De_Estoque.Model
{
    public class PostParaAnuncio
    {
        public string site_id { get; set; } = "MLB";
        public string title { get; set; }
        //public long seller_id { get; set; } 
        public string category_id { get; set; }
        public decimal price { get; set; }
        public string currency_id { get; set; } = "BRL";
        public int available_quantity { get; set; }
        public string buying_mode { get; set; } = "buy_it_now";
        public string listing_type_id { get; set; } = "gold_special";
        public string condition { get; set; } = "new";
        public List<SaleTerm> sale_terms { get; set; }
        public List<Picture> pictures { get; set; }
        public List<Variation> variations { get; set; }
        public List<Attribute> attributes { get; set; }
        public Shipping shipping { get; set; }
    }

    public class SaleTerm
    {
        public string id { get; set; }
        public string value_name { get; set; }
    }

    public class Picture
    {
        public string source { get; set; }
    }

    public class Variation
    {
        public decimal price { get; set; }
        public List<AttributeCombinatiosn> attribute_combinations { get; set; }
        public int available_quantity { get; set; }
        public List<string> picture_ids { get; set; }
    }

    public class AttributeCombinatiosn
    {
        public string id { get; set; }
        public string name { get; set; }
        public string value_id { get; set; }
        public string value_name { get; set; }
    }

    public class Attribute
    {
        public string id { get; set; }
        public string value_id { get; set; }
        public string value_name { get; set; }
    }

    public class Shipping
    {
        public string mode { get; set; } = "me2";
        public bool local_pick_up { get; set; } = false;
        public bool free_shipping { get; set; } = false;
        public string logistic_type { get; set; } = "drop_off";
    }

}
