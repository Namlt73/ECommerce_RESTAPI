using ApiEcommerce.Entities;
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

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategories();
            return Ok(categories);
        }
        public  IActionResult GetById(int id)
        {
            var category = _categoryService.GetById(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            if (category == null)
                return BadRequest(ModelState);

            _categoryService.Create(category);
            
            return StatusCode(201);
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
