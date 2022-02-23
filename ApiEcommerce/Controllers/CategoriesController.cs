using ApiEcommerce.Dtos.CategoryDtos;
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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;

        public CategoriesController(ICategoryService categoryService, IUserService userService)
        {
            _categoryService = categoryService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategories();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public  IActionResult GetById(int id)
        {
            var category = _categoryService.GetById(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateCategory(string name, string description, List<IFormFile> images)
        {
            // If the user POSTs the image the list<IFormFile> will be populated, if it's null instead it will be empty
            if (images?.Count == 0)
                images = Request.Form.Files.GetFiles("images[]").ToList();

            Category category = await _categoryService.Create(name, description, images, Convert.ToInt64(_userService.GetCurrentUserId()));
            return StatusCodeAndDtoWrapper.BuildSuccess(CategoryDto.Build(category), "Category created successfully");
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] Category category)
        {
            if (category == null || id != category.Id)
                return BadRequest();
            _categoryService.Update(category);
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            _categoryService.Delete(id);
            
            return StatusCode(204);
        }
    }
}
