namespace Controle_De_Estoque.Model
{
    public class Items_Id
    
        {
            public int seller_id { get; set; }
            public string? query { get; set; }
            public Paging? paging { get; set; }
            public List<string>? results { get; set; }
    }
    public class Paging
        {
            public int limit { get; set; }
            public int offset { get; set; }
            public int total { get; set; }
        }
    }
