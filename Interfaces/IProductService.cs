using ProvaPub.Models;

namespace ProvaPub.Interfaces
{
    public interface IProductService
    {
        ProductList ListProducts(int page);
    }
}
