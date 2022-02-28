using ApiEcommerce.Data;
using ApiEcommerce.Dtos.CommentDtos;
using ApiEcommerce.Entities;
using ApiEcommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Implements
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductsService _productService;
        private readonly HtmlEncoder _htmlEncoder;

        public CommentService(ApplicationDbContext context, IProductsService productService, HtmlEncoder htmlEncoder)
        {
            _context = context;
            _productService = productService;
            _htmlEncoder = htmlEncoder;
        }
        public async Task<Comment> AddComment(User user, string productSlug, CommentDto commentDto, long userId)
        {
            var product = await _productService.GetProductBySlug(productSlug);

            var comment = new Comment()
            {
                Product = product,
                ProductId = product.Id,
                Content = _htmlEncoder.Encode(commentDto.Content),
                Rating = commentDto.Rating,
                UserId = userId,
                User = user,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };


            await _context.Comments.AddAsync(comment);
            product.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<int> DeleteComment(long id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null)
            {
                return -1;
            }

            _context.Comments.Remove(comment);
            return await _context.SaveChangesAsync();
        }

        public async Task<Comment> GetCommentById(long id, bool includeUser = false)
        {
            if (includeUser)
                return await _context.Comments.Include(c => c.User).FirstAsync(c => c.Id == id);
            else
                return await _context.Comments.FindAsync(id);
        }

        public async Task<int> UpdateComment(Comment comment, CommentDto commentDto)
        {
            comment.Content = _htmlEncoder.Encode(commentDto.Content);
            comment.Rating = commentDto.Rating;
            return await _context.SaveChangesAsync();
        }
    }
}
