using ApiEcommerce.Dtos.PageDtos;
using ApiEcommerce.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.TagDtos
{
    public class ListTagsDto : PaginationDto
    {
        public IEnumerable<TagDto> Tags { get; set; }
        //    public int SortBy {get; set;}


        public static ListTagsDto Build(List<Entities.Tag> tags,
            string basePath,
            int currentPage, int pageSize, int totalItemCount)
        {
            var tagDtos = new List<TagDto>(tags.Count);
            foreach (var tag in tags)
            {
                tagDtos.Add(TagDto.Build(tag));
            }

            return new ListTagsDto
            {
                Pagination = new PaginationModel(tags.Count, basePath, currentPageNumber: currentPage, requestedPageSize: pageSize,
                    totalItemCount: totalItemCount),
                Tags = tagDtos
            };
        }
    }
}
