using ApiEcommerce.Dtos.CommentDtos;
using ApiEcommerce.Entities;
using ApiEcommerce.Helper;
using ApiEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConfigurationService _configService;

        public CommentsController(IUserService userService, ICommentService commentService,
           IAuthorizationService authorizationService, IConfigurationService configService)
        {
            _userService = userService;
            _commentService = commentService;
            _authorizationService = authorizationService;
            _configService = configService;            
        }

        [HttpGet("products/{slug}/comments")]
        public async Task<IActionResult> GetComments(string slug)
        {
            Tuple<int, List<Comment>> comments = await _commentService.GetCommentsByProduct(slug);

            return Ok(comments);
        }


        [HttpGet("products/{slug}/comments/{comment_id}")]
        [HttpGet("/comments/{comment_id}")]
        public async Task<IActionResult> GetDetails(long commentId)
        {
            Comment comment = await _commentService.GetCommentById(commentId);
            return StatusCodeAndDtoWrapper.BuildSuccess(CommentDetailsDto.Build(comment));
        }

        
        [HttpPost("products/{slug}/comments")]
        public async Task<IActionResult> CreateComment(string slug, [FromBody] CommentDto model)
        {
            if (!ModelState.IsValid)
                return StatusCodeAndDtoWrapper.BuilBadRequest(ModelState);

            long userId = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            User user = await _userService.GetCurrentUserAsync();
            Comment comment = await _commentService.AddComment(user, slug, model, userId);

            return StatusCodeAndDtoWrapper.BuildSuccess(CommentDetailsDto.Build(comment));
        }

        [HttpPut]
        [Authorize]
        [Route("comments/{slug}/comments/{id}")]
        [Route("comments/{id}")]
        public async Task<IActionResult> UpdateComment([FromBody] CommentDto model, [FromRoute] long id)
        {
            Comment comment = await _commentService.GetCommentById(id, includeUser: true);
            if (comment == null)
            {
                return StatusCodeAndDtoWrapper.BuildGenericNotFound();
            }

            var result = await _authorizationService.AuthorizeAsync(User, comment,
                _configService.GetDeleteCommentPolicyName());
            if (result.Succeeded)
            {
                await _commentService.UpdateComment(comment, model);
                return StatusCodeAndDtoWrapper.BuildSuccess(CommentDetailsDto.Build(comment),
                    "Comment updated successfully");
            }
            else
            {
                return StatusCodeAndDtoWrapper.BuildUnauthorized("Deny access!");
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("comments/{slug}/comments/{id}")]
        [Route("comments/{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            Comment comment = await _commentService.GetCommentById(id);
            if (comment == null)
            {
                return StatusCodeAndDtoWrapper.BuildGenericNotFound();
            }

            var result = await _authorizationService.AuthorizeAsync(User, comment,
                _configService.GetDeleteCommentPolicyName());
            if (result.Succeeded)
            {
                if ((await _commentService.DeleteComment(id)) > 0)
                {
                    return StatusCodeAndDtoWrapper.BuildSuccess("Comment deleted successfully");
                }
                else
                {
                    return StatusCodeAndDtoWrapper.BuildErrorResponse("Error! Please try again later");
                }
            }
            else
            {
                return StatusCodeAndDtoWrapper.BuildUnauthorized("Deny access!");
            }
        }
    }
}
