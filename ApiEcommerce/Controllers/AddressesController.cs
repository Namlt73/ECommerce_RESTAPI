using ApiEcommerce.Dtos.AddressDtos;
using ApiEcommerce.Helper;
using ApiEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressesService _addressesService;
        private readonly IUserService _usersService;

        public AddressesController(IAddressesService addressesService, IUserService usersService)
        {
            _addressesService = addressesService;
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var addresses =
                await _addressesService.GetAddressByUser(await _usersService.GetCurrentUserAsync(), page,
                    pageSize);
            var basePath = Request.Path;

            return StatusCodeAndDtoWrapper.BuildSuccess(ListAddressesDto.Build(addresses.Item2, basePath,
                currentPage: page, pageSize: pageSize, totalItemCount: addresses.Item1));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddressFormRequest dto)
        {
            var address = await _addressesService.AddAddress(await _usersService.GetCurrentUserAsync(), 
                dto.FirstName,
                dto.Lastname, 
                dto.Country, 
                dto.City, 
                dto.StreetAddress, 
                dto.ZipCode);


            return StatusCodeAndDtoWrapper.BuildSuccess(AddressDto.Build(address));
        }

    }
}
