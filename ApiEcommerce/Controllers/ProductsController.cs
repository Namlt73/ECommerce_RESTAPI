using ApiEcommerce.Dtos.ProductDtos;
using ApiEcommerce.Entities;
using ApiEcommerce.Helper;
using ApiEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly IUserService _usersService;

        public ProductsController(
            IProductsService productService,
            IUserService usersService)
        {
            _productsService = productService;
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productsService.GetAll();

            return Ok(products);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(string name, string description, int price, int stock,
            List<IFormFile> images)
        {
            if (!(await _usersService.IsAdmin()))
                return StatusCodeAndDtoWrapper.BuildUnauthorized("Only admin user can create prodcuts");
         
            if (images?.Count == 0)
                images = Request.Form.Files.GetFiles("images[]").ToList();

            var tags = new List<Tag>();
            var categories = new List<Category>();

            foreach (string formKey in Request.Form.Keys)
            {
                var regex = new Regex("tags|categories\\[(?<name>\\w+)\\]");
                Match match = regex.Match(formKey);
                if (match.Success && formKey.StartsWith("tag"))
                {
                    var tagName = match.Groups["name"].Value;
                    tags.Add(new Tag
                    {
                        Name = tagName,
                        Description = Request.Form[key: formKey].ToString()
                    });
                }

                if (match.Success && formKey.StartsWith("cate"))
                {
                    var categoryName = match.Groups["name"].Value;
                    categories.Add(new Category
                    {
                        Name = categoryName,
                        Description = Request.Form[key: formKey].ToString()
                    });
                }
            }
            Product product = await _productsService.Create(name, description, price, stock, tags, categories, images);
            return StatusCodeAndDtoWrapper.BuildSuccess(ProductDetailsDto.Build(product));
        }
    }
}
