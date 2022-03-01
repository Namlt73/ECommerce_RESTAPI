using ApiEcommerce.Dtos.PageDtos;
using ApiEcommerce.Entities;
using ApiEcommerce.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.ProductDtos
{
    public class ListProductsDto : PaginationDto
    {
        public IEnumerable<SummaryProductDto> Products { get; set; }


        public static ListProductsDto Build(List<Product> products,
            string basePath,
            int currentPage, int pageSize, int totalItemCount)
        {
            var summaryProductDtos = new List<SummaryProductDto>(products.Count);
            foreach (var product in products)
            {
                summaryProductDtos.Add(SummaryProductDto.Build(product));
            }

            return new ListProductsDto
            {
                Pagination = new PaginationModel(products.Count, basePath, currentPageNumber: currentPage, requestedPageSize: pageSize,
                    totalItemCount: totalItemCount),
                Products = summaryProductDtos
            };
        }
    }
}
