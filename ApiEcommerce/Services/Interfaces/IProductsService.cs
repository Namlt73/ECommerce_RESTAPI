using ApiEcommerce.Dtos.CategoryDtos;
using ApiEcommerce.Dtos.ProductDtos;
using ApiEcommerce.Dtos.TagDtos;
using ApiEcommerce.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Interfaces
{
    public interface IProductsService
    {
        Task<List<Product>> GetAll();
        Task<Product> GetById(long id, bool onlyIfPublished = false);

        Task Create(Product product);

        Task<Task> Update(Product product);

        Task<int> Delete(long id);
        Task<int> Delete(Product product);
        Task<int> Delete(string slug);

        Product GetByIdSync(long id);
        Task<Tuple<int, List<Product>>> GetBySearchTerm(string term);
        Task<Product> GetProductBySlug(string slug);
        Task<Tuple<int, List<Product>>> GetByCategory(string category);
        Task SaveProduct(Product product);

        Task<Product> Create(string name, string description,
                    IEnumerable<TagOnlyNameDto> tagOnlyNameDtos, 
                    IEnumerable<CategoryOnlyNameDto> categoryOnlyNameDtos,
                    List<IFormFile> images);

        Task<Product> Create(string name, string description, 
                        int price, int stock, List<Tag> tags,
                        List<Category> categories, List<IFormFile> images, 
                        bool processTags = true, bool processCategories = true);

        Task<Product> Update(string slug, ProductDto dto);
        Product DeleteProductById(long id);

    }
}
