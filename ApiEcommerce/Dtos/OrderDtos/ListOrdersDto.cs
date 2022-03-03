using ApiEcommerce.Dtos.PageDtos;
using ApiEcommerce.Entities;
using ApiEcommerce.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.OrderDtos
{
    public class ListOrdersDto : PaginationDto
    {
        public IEnumerable<OrderDto> Orders { get; set; }

        public static ListOrdersDto Build(List<Order> orders,
            string basePath,
            int currentPage, int pageSize, int totalItemCount)
        {
            var orderDtos = new List<OrderDto>(orders.Count);
            foreach (var order in orders)
            {
                orderDtos.Add(OrderDto.Build(order));
            }

            return new ListOrdersDto
            {
                Pagination = new PaginationModel(orders.Count, basePath, currentPageNumber: currentPage, requestedPageSize: pageSize,
                    totalItemCount: totalItemCount),
                Orders = orderDtos
            };
        }
    }
}
