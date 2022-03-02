using ApiEcommerce.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Interfaces
{
    public interface ITagsService
    {
        Task<Tuple<int, List<Tag>>> GetTags(int page, int pageSize);
        Task<Tuple<int, List<Tag>>> GetTagsWithImages(int page, int pageSize);
        Task<Tag> Create(string name, string description, List<IFormFile> files);
    }
}
