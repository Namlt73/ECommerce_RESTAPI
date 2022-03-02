using ApiEcommerce.Dtos.PageDtos;
using ApiEcommerce.Entities;
using ApiEcommerce.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.CategoryDtos
{
    public class ListCategoriesDto : PaginationDto
    {
        public IEnumerable<CategoryDto> Categories { get; set; }


        public static ListCategoriesDto Build(List<Category> categories,
            string basePath,
            int currentPage, int pageSize, int totalItemCount)
        {
            var catergoriesDtos = new List<CategoryDto>(categories.Count);
            foreach (var category in categories)
            {
                catergoriesDtos.Add(CategoryDto.Build(category));
            }

            return new ListCategoriesDto
            {
                Pagination = new PaginationModel(categories.Count, basePath, currentPageNumber: currentPage, requestedPageSize: pageSize,
                    totalItemCount: totalItemCount),
                Categories = catergoriesDtos
            };
        }
    }
}
