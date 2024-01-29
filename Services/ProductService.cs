using ProvaPub.Interfaces;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
	public class ProductService : IProductService
	{

        private readonly TestDbContext _ctx;

        public ProductService(TestDbContext ctx)
        {
            _ctx = ctx;
        }

        public ProductList ListProducts(int page)
        {
            int pageSize = 10; // Número de itens por página

            // Lógica de paginação no banco de dados
            var products = _ctx.Products
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalCount = _ctx.Products.Count();

            return new ProductList
            {
                Items = products,
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

    }
}
