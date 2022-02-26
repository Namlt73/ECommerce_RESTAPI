using ApiEcommerce.Dtos.Handles;
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
        private readonly IAuthorizationService _authorizationService;
        private readonly IConfigurationService _configurationService;

        public ProductsController(
            IProductsService productService,
            IUserService usersService,
            IAuthorizationService authorizationService,
            IConfigurationService configurationService)
        {
            _productsService = productService;
            _usersService = usersService;
            _authorizationService = authorizationService;
            _configurationService = configurationService;
        }

        [HttpGet()]
        public async Task<IActionResult> Index()
        {
            var products = await _productsService.GetAll();

            return Ok(products);
        }

        [HttpGet("by_cat/{id}")]
        //[Route("/by_category/{category}")]
        public async Task<IActionResult> GetByCategory([FromRoute] string category)
        {
            Tuple<int, List<Product>> products = await _productsService.GetByCategory(category);

            return Ok(products);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetProductBySlug(string slug)
        {
            var product = await _productsService.GetProductBySlug(slug);
            if (product == null)
                return StatusCodeAndDtoWrapper.BuildNotFound(new ErrorDtoResponse("Not Found"));

            return new StatusCodeAndDtoWrapper(ProductDetailsDto.Build(product));
        }

        [HttpGet("by_id/{id}")]
        public async Task<IActionResult> GetProductById(long id)
        {
            var product = await _productsService.GetById(id);
            if (product == null)
                return StatusCodeAndDtoWrapper.BuildNotFound(new ErrorDtoResponse("Not Found"));

            return StatusCodeAndDtoWrapper.BuildGeneric(ProductDetailsDto.Build(product));
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

        [HttpPut("{slug}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(string slug, [FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
                return StatusCodeAndDtoWrapper.BuilBadRequest(ModelState);


            Product product = await _productsService.Update(slug, productDto);

            return StatusCodeAndDtoWrapper.BuildSuccess(ProductDetailsDto.Build(product), "Updated successfully");
        }


        [HttpDelete]
        [Authorize]
        //[Route("products/{slug}")]
        //[Route("products/by_id/{id}")]
        public async Task<IActionResult> Delete(long? id, string slug)
        {
            Product product;
            if (id != null)
                product = await _productsService.GetById(id.Value);
            else
                product = await _productsService.GetProductBySlug(slug);

            if (product == null)
            {
                return StatusCodeAndDtoWrapper.BuildGenericNotFound();
            }

            var result = await _authorizationService.AuthorizeAsync(User, product,
                _configurationService.GetManageProductPolicyName());
            if (result.Succeeded)
            {
                if ((await _productsService.Delete(product)) > 0)
                {
                    return StatusCodeAndDtoWrapper.BuildSuccess("Product deleted successfully");
                }
                else
                {
                    return StatusCodeAndDtoWrapper.BuildErrorResponse("Error, please try again later");
                }
            }
            else
            {
                return StatusCodeAndDtoWrapper.BuildUnauthorized("Access denied");
            }
        }
    }
}
