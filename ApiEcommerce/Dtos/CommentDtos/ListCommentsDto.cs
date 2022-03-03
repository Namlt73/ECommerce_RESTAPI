using ApiEcommerce.Dtos.Handles;
using ApiEcommerce.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.CommentDtos
{
    class ListCommentsDto : SuccessResponse
    {

        public PaginationModel PaginationModel { get; set; }
        public ICollection<CommentDetailsDto> Comments { get; set; }

        public static ListCommentsDto Build(ICollection<Entities.Comment> comments,
            string basePath,
            int currentPage, int pageSize, int totalItemCount)
        {
            ICollection<CommentDetailsDto> result = new List<CommentDetailsDto>();

            foreach (var comment in comments)
                result.Add(CommentDetailsDto.Build(comment, false, true));

            return new ListCommentsDto
            {
                Success = true,
                PaginationModel = new PaginationModel(result.Count, basePath, currentPageNumber: currentPage, requestedPageSize: pageSize,
                    totalItemCount: totalItemCount),
                Comments = result
            };
        }
    }
}
