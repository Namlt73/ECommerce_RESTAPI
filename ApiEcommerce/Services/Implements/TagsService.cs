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
    public class TagsService : ITagsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IStorageService _storageService;


        public TagsService(ApplicationDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<Tuple<int, List<Tag>>> GetTags()
        {
            var queryable = _context.Tags;
            var count = await queryable.CountAsync();
            var results = await queryable.ToListAsync();

            return await Task.FromResult(Tuple.Create(count, results));
        }

        public async Task<Tuple<int, List<Tag>>> GetTagsWithImages()
        {
            var queryable = _context.Tags.Include(t => t.TagImgs)
                .Where(t => t.TagImgs != null && t.TagImgs.Count > 0);
            var count = await queryable.CountAsync();
            var results = await queryable.ToListAsync();

            return await Task.FromResult(Tuple.Create(count, results));
        }


        public async Task<Tag> Create(string name, string description, List<IFormFile> files)
        {
            ICollection<TagImg> fileUploads = new List<TagImg>(files.Count);
            foreach (IFormFile file in files)
            {
                FileUpload fileUpload = await _storageService.UploadFormFile(file, "tags");

                fileUploads.Add(new TagImg
                {
                    OriginalFileName = fileUpload.OriginalFileName,
                    FileName = fileUpload.FileName,
                    FilePath = fileUpload.FilePath,
                    FileSize = file.Length,
                });
            }


            var tag = new Tag
            {
                Name = name,
                Description = description,
                TagImgs = fileUploads
            };
            _context.Tags.Add(tag);

            await _context.SaveChangesAsync();
            return tag;
        }
    }
}
