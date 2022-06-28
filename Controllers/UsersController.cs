using MAPay.Data;
using MAPay.Models;
using Microsoft.AspNetCore.Http;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MAPay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly MAPayAPIDbContext dbContext;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public UsersController(MAPayAPIDbContext dbContext, IWebHostEnvironment environment)
        {
            this.dbContext = dbContext;
            _webHostEnvironment = environment;
        }

        [HttpGet]
        [Route("/user-profile{Id:guid}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid Id)
        {
            var user = await dbContext.User.FindAsync(Id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("/document-status{Id:guid}")]
        public async Task<IActionResult> GetDocument([FromRoute] Guid Id)
        {
            var document = await dbContext.Document.FindAsync(Id);

            if (document == null)
            {
                return NotFound();
            }
            return Ok(document);
        }

        /*[HttpPost]
        [Route("/user-signup")]
        public async Task<IActionResult> AddUser(AddUserRequest addUserRequest)
        {
            var user = new UserSignUp()
            {
                Id = Guid.NewGuid(),
                Name = addUserRequest.Name,
                Role = addUserRequest.Role,
                Email = addUserRequest.Email,
                Password = addUserRequest.Password,
                LastLogin = DateTime.Now.Date,//addUserRequest.LastLogin,
                LastUpdated = addUserRequest.LastUpdated,
            };

            await dbContext.User.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return Ok(user);
        }*/

        /*[HttpPost]
        [Route("/upload-file")]
        public Task UploadFile(IFormFile formFile)
        {
            return Task.CompletedTask;
        }*/

        [HttpPost]
        [Route("/user-signup-upload")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFiles([FromForm]AddUserRequest addUserRequest)
        {
            if (addUserRequest.File != null)
            {
                //upload files to wwwroot 
                var fileName = Path.GetFileName(addUserRequest.File.FileName);
                // pdf file check
                string ext = Path.GetExtension(addUserRequest.File.FileName);
                if (ext.ToLower() != ".pdf")
                {
                    return View();
                }
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Documents", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await addUserRequest.File.CopyToAsync(fileStream);
                }

                //save file to database
                var user = new UserSignUp()
                {
                    Id = Guid.NewGuid(),
                    Name = addUserRequest.Name,
                    Role = "user",
                    Email = addUserRequest.Email,
                    Password = addUserRequest.Password,
                    LastLogin = DateTime.Now.Date,
                    LastUpdated = DateTime.Now.Date,
                    FilePath = filePath,
                };

                await dbContext.User.AddAsync(user);

                var document = new DocumentUpload()
                {
                    Id = user.Id,
                    //File = addUserRequest.File,
                    Status = "",
                    LastUpdated = DateTime.Now.Date,
                };

                await dbContext.Document.AddAsync(document);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                return BadRequest("No File Uploaded");
            }
            return Ok("File Uploaded...");
            //View();

            /*if (addUserRequest.File.Count == 0)
                return BadRequest();
            string directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Documents");

            foreach (var file in addUserRequest.File)
            {
                string filePath = Path.Combine(directoryPath, file.Name);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            return Ok("Uploded Successfully");*/
        }


        [HttpPost]
        [Route("/user-login")]
        public async Task<IActionResult> UserLogin([FromForm]UserLogin userLogin)
        {
            var user = dbContext.User.Where(x => x.Email == userLogin.Email &&
                            x.Password == userLogin.Password).FirstOrDefault();

            if (user != null)
            {
                user.LastLogin = DateTime.Now;
                return Ok("SuccessFully Login");
            }
            return BadRequest("Invalid User Login!!");
        }

        [HttpPut]
        [Route("/edit-user-profile{Id:guid}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid Id, [FromForm]UpdateUserRequest updateUserRequest)
        {
            var user = await dbContext.User.FindAsync(Id);

            if (user != null)
            {
                user.Name = updateUserRequest.Name;
                user.Email = updateUserRequest.Email;
                user.Password = updateUserRequest.Password;
                user.LastUpdated = DateTime.Now;//updateUserRequest.LastUpdated;

                await dbContext.SaveChangesAsync();
                return Ok(user);
            }
            return NotFound();
        }
    }
}
