using API.Data;
using API.Dtos;
using API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController(IConfiguration configuration) : ControllerBase
    {
        private readonly DataContextDapper _dataContextDapper = new(configuration);
        private readonly IConfiguration _configuration = configuration;

        [HttpGet("Posts")]
        public IEnumerable<Post> GetPosts()
        {
            IEnumerable<Post> result = _dataContextDapper.LoadData<Post>("SELECT * FROM TutorialAppSchema.Posts;");
            return result;
        }

        [HttpGet("Post/{postId}")]
        public Post GetPost(int postId)
        {
            Post result = _dataContextDapper.LoadDataSingle<Post>("SELECT * FROM TutorialAppSchema.Posts WHERE PostId = " + postId.ToString() + ";");
            return result;
        }

        [HttpGet("MyPosts")]
        public IEnumerable<Post> GetMyPosts()
        {
            IEnumerable<Post> result = _dataContextDapper.LoadData<Post>("SELECT * FROM TutorialAppSchema.Posts WHERE UserId = @UserId;", new { UserId = int.Parse(User.FindFirst("userId")!.Value) });

            return result;
        }

        [HttpPost("Post")]
        public IActionResult AddPost(PostToAddDto postToAddDto)
        {
            int row = _dataContextDapper.ExecuteSqlwithRowCount("INSERT INTO TutorialAppSchema.Posts(UserId, PostTitle, PostContent, PostCreated, PostUpdated) VALUES (@UserId, @PostTitle, @PostContent, GETDATE(), GETDATE());", new
            {
                UserId = this.User.FindFirst("userId")?.Value,
                PostTitle = postToAddDto.PostTitle,
                PostContent = postToAddDto.PostContent
            });

            if (row > 0)
            {
                return Ok("Post Added");
            }
            else
            {
                throw new Exception("Failed to add Post");
            }
        }

        [HttpPut("PostToEdit")]
        public IActionResult PostToEdit(PostToEditDto postToEditDto)
        {
            int row = _dataContextDapper.ExecuteSqlwithRowCount("UPDATE TutorialAppSchema.Posts SET PostTitle = @PostTitle, PostContent = @PostContent, PostUpdated = GETDATE() WHERE PostId = @PostId AND UserId = @UserId;",
            new
            {
                UserId = this.User.FindFirst("userId")?.Value,
                PostId = postToEditDto.PostId,
                PostTitle = postToEditDto.PostTitle,
                PostContent = postToEditDto.PostContent,
                PostUpdated = "GETDATE()"
            });
            if (row > 0)
            {
                return Ok("Post Edited");
            }
            throw new Exception("Bruh it failed");
        }

        [HttpDelete("DeletePost/{postId}")]
        public IActionResult DeletePost(int postId)
        {
            int row = _dataContextDapper.ExecuteSqlwithRowCount("DELETE FROM TutorialAppSchema.Posts WHERE PostId = @PostId AND UserId = @UserId;",
            new
            {
                UserId = this.User.FindFirst("userId")?.Value,
                PostId = postId
            });
            if (row > 0)
            {
                return Ok("Post Deleted");
            }
            throw new Exception("Failed to delete post.");
        }
    }
}