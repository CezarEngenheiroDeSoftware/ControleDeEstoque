namespace Controle_De_Estoque.Model
{
    public class UserData
    {
        public string? id { get; set; }
        public string? nickname { get; set; }
        public DateTime registration_date { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? country_id { get; set; }
        public string? email { get; set; }
        public string? state { get; set; }
        public string? city { get; set; }
        public Identification identification { get; set; }
        public Address address { get; set; }
        public Phone phone { get; set; }
    }
    public class Identification
    {
        public long number { get; set; }
        public string? type { get; set; }
    }
    public class Address
    {
        public string? state { get; set; }
        public string? city { get; set; }
        public string? address { get; set; }
    }
    public class Phone
    {
        public string? area_code { get; set; }
        public long number { get; set; }
        //public int extension { get; set; }
        //public bool verified { get; set; }
    }
}
