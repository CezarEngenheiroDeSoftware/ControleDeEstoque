namespace Controle_De_Estoque.Models
{
    public class UpdateStock
    {
        public int id { get; set; }
        public int? stock_quantity { get; set; }
        public string? sku { get; set; }

    }
}
