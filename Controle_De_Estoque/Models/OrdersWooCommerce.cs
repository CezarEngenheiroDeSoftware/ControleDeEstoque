namespace Controle_De_Estoque.Models
{
    public class OrdersWooCommerce
    {
        public int id { get; set; }
        public int number { get; set; }
        public string? status { get; set; }
        public DateTime date_created { get; set; }
        public decimal shipping_total { get; set; }
        public decimal total {  get; set; }
        public Billing? billing { get; set; }
    }
    public class Billing
    {
        public string? first_name { get; set; }
        public string? address_1 { get; set; }
        public string? city {  get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
    }
}
