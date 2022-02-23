using ApiEcommerce.Data;
using ApiEcommerce.Entities;
using ApiEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Implements
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IStorageService _storageService;
        public CategoryService(ApplicationDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public int Count()
        {
            return _context.Categories.Count();
        }

        public void Create(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public async Task<Category> Create(string name, string description, List<IFormFile> files, long? userId = null)
        {
            ICollection<CategoryImg> fileUploads = new List<CategoryImg>(files.Count);
            foreach (IFormFile file in files)
            {
                FileUpload fileUpload = await _storageService.UploadFormFile(file, "categories");

                fileUploads.Add(new CategoryImg
                {
                    OriginalFileName = fileUpload.OriginalFileName,
                    FileName = fileUpload.FileName,
                    FilePath = fileUpload.FilePath,
                    FileSize = file.Length
                });
            }


            var category = new Category
            {
                Name = name,
                Description = description,
                CategoryImages = fileUploads
            };
            _context.Categories.Add(category);

            await _context.SaveChangesAsync();
            return category;
        }

        public void Delete(int id)
        {
            var category = GetById(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            _context.SaveChanges();
        }

        public Category GetById(int id)
        {
            return _context.Categories.SingleOrDefault(c => c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }
    }
}
