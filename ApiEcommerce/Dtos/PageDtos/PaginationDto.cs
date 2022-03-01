using ApiEcommerce.Dtos.Handles;
using ApiEcommerce.Helper;

namespace ApiEcommerce.Dtos.PageDtos
{
    public abstract class PaginationDto : SuccessResponse
    {
        public PaginationModel Pagination { get; set; }
    }
}
