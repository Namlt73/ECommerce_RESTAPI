using ApiEcommerce.Dtos.Handles;
using ApiEcommerce.Dtos.OrderDtos;
using ApiEcommerce.Entities;
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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderItemsService _orderItemsService;
        private readonly IUserService _userService;

        public OrdersController(IOrderItemsService orderItemsService, IUserService userService)
        {
            _orderItemsService = orderItemsService;
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetOrderOfCurrentUser([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            long userId = Convert.ToInt64(_userService.GetCurrentUserId());
            Tuple<int, List<Order>> orders = await _orderItemsService.GetOrderByUser(userId);

            return StatusCodeAndDtoWrapper.BuildSuccess(ListOrdersDto.Build(orders.Item2, Request.Path,
                currentPage: page, pageSize: pageSize, totalItemCount: orders.Item1));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrdersById(long id)
        {
            var order = await _orderItemsService.GetById(id, includeOrderItems: true, includeAddress: true);
            if (order == null)
                return StatusCodeAndDtoWrapper.BuildGeneric(new ErrorDtoResponse("Not found! Please try again later"), statusCode: 404);


            return new StatusCodeAndDtoWrapper(OrderDetailDto.Build(order, false));
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDtoRequest formRequest)
        {
            var order = await _orderItemsService.Create(formRequest, await _userService.GetCurrentUserAsync());
            if (order != null)
            {
                return StatusCodeAndDtoWrapper.BuildGeneric(OrderDetailDto.Build(order));
            }
            else
            {
                return StatusCodeAndDtoWrapper.BuildErrorResponse("Something went wrong! Please try again later");
            }
        }
    }
}
