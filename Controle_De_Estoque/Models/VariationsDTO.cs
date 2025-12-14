namespace Controle_De_Estoque.Models
{
    //public class UpdateEstoqueDTO
    //{
        //public string id { get; set; }
        //public List<VariationsDTO> variations {  get; set; }

        public class VariationsDTO
        {
            public long id { get; set; }
            public int available_quantity { get; set; }
            public string? seller_custom_field { get; set; }
        }
        
    //}
}
