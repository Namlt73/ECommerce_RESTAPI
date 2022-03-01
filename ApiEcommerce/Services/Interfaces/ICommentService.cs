using ApiEcommerce.Dtos.CommentDtos;
using ApiEcommerce.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> GetCommentById(long id, bool includeUser = false);
        Task<Comment> AddComment(User user, string productSlug, CommentDto commentDto,
            long userId);
        Task<int> DeleteComment(long id);
        Task<int> UpdateComment(Comment comment, CommentDto commentDto);
        Task<Tuple<int, List<Comment>>> GetCommentsByProduct(string slug);
    }
}
