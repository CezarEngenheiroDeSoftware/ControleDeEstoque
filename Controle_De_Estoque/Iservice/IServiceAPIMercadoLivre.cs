using Controle_De_Estoque.Model;
using Controle_De_Estoque.Models;
using Microsoft.AspNetCore.Mvc;

public interface IServiceAPIMercadoLivre
{
    //Task<bool> VerificarToken();

    Task<UserData> GetUserData(string token);

    Task<SerchProduct> GetProduct(string itemId);

    Task<List<SerchProduct>> GetListProducts(string userId);

    Task<string> SearchAttribute(string categ);

    Task<string> SearchCategory(string q);

    Task<PostParaAnuncio> CreateItem(PostParaAnuncio item);

    Task<ItemMeli> UpdateItem(string itemId, ItemMeli updateData);

    Task<VariationsDTO> UpdateItemVariation(string iten_id, VariationsDTO variations, long variationsId);

    Task<AcessToken> RefreshToken(string token);
}
