using ApiEcommerce.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Tuple<int, List<Category>>> GetCategories(int page, int pageSize);
        Category GetById(int id);
        int Count();
        void Create(Category category);
        Task<Category> Create(string name, string description, List<IFormFile> files,
            long? userId = null);
        void Update(Category category);
        void Delete(int id);
    }
}
