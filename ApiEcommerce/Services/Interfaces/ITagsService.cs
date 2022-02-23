using ApiEcommerce.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Interfaces
{
    public interface ITagsService
    {
        Task<Tuple<int, List<Tag>>> GetTags();
        Task<Tuple<int, List<Tag>>> GetTagsWithImages();
        Task<Tag> Create(string name, string description, List<IFormFile> files);
    }
}
