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
        Task<Tuple<int, List<Product>>> GetProductsPage(int page = 1, int pageSize = 10);        
        Task<Product> GetById(long id, bool onlyIfPublished = false);
        Task Create(Product product);
        Task<int> Delete(long id);
        Task<int> Delete(Product product);

        Task<Tuple<int, List<Product>>> GetBySearchTerm(string term, int page, int pageSize);
        Task<Product> GetProductBySlug(string slug);
        Task<Tuple<int, List<Product>>> GetByCategory(string category, int pageSize, int page);
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
        Task<List<Product>> GetByIdInRetrieveNamePriceAndSlug(IEnumerable<long> productIds);
    }
}
