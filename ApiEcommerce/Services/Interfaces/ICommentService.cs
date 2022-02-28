using ApiEcommerce.Dtos.CommentDtos;
using ApiEcommerce.Entities;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Interfaces
{
    interface ICommentService
    {
        Task<Comment> GetCommentById(long id, bool includeUser = false);
        Task<Comment> AddComment(User user, string productSlug, CommentDto commentDto,
            long userId);
        Task<int> DeleteComment(long id);
        Task<int> UpdateComment(Comment comment, CommentDto commentDto);
    }
}
