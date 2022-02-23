using ApiEcommerce.Dtos.TagDtos;
using ApiEcommerce.Entities;
using ApiEcommerce.Helper;
using ApiEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService _tagsService;

        public TagsController(ITagsService tagsService)
        {
            _tagsService = tagsService;
        }


        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var tags = await _tagsService.GetTags();
            return Ok(tags);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag(string name, string description, List<IFormFile> images)
        {
            // If the user POSTs the image the list<IFormFile> will be populated, if it's null instead it will be empty
            if (images?.Count == 0)
                images = Request.Form.Files.GetFiles("images[]").ToList();
            Tag tag = await _tagsService.Create(name, description, images);
            return StatusCodeAndDtoWrapper.BuildSuccess(TagDto.Build(tag), "Tag created successfully");
        }
    }
}
