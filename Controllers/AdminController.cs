using MAPay.Data;
using MAPay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MAPay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly MAPayAPIDbContext dbContext;
        public AdminController(MAPayAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("/admin-profile{Id:guid}")]
        public async Task<IActionResult> GetAdmin([FromRoute] Guid Id)
        {
            var admin = await dbContext.Admin.FindAsync(Id);
            if (admin == null)
            {
                return NotFound();
            }
            return Ok(admin);
        }

        [HttpGet]
        [Route("/view-user{Id:guid}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid Id)
        {
            var user = await dbContext.User.FindAsync(Id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [HttpPost]
        [Route("/admin-signup")]
        public async Task<IActionResult> AddAdmin([FromForm]AddAdminRequest addAdminRequest)
        {
            var admin = new AdminSignUp()
            {
                Id = Guid.NewGuid(),
                Name = addAdminRequest.Name,
                Role = addAdminRequest.Role,
                Email = addAdminRequest.Email,
                Password = addAdminRequest.Password,
                LastLogin = DateTime.Now.Date,//addAdminRequest.LastLogin,
                LastUpdated = DateTime.Now.Date,//addAdminRequest.LastUpdated,
            };

            await dbContext.Admin.AddAsync(admin);
            await dbContext.SaveChangesAsync();

            return Ok(admin);
        }


        [HttpPost]
        [Route("/admin-login")]
        public async Task<IActionResult> AdminLogin([FromForm]AdminLogin adminLogin)
        {
            /*var ad = await dbContext.Admin.FindAsync(o => o.Email == adminLogin.Email);*/

            var admin = dbContext.Admin.Where(x => x.Email == adminLogin.Email &&
                                x.Password == adminLogin.Password).FirstOrDefault();

            if (admin != null)
            {
                admin.LastLogin = DateTime.Now;
                return Ok("SuccessFully Login");
            }
            return BadRequest("Invalid User Login!!");
        }

        [HttpPut]
        [Route("/edit-admin-profile{Id:guid}")]
        public async Task<IActionResult> UpdateAdmin([FromRoute] Guid Id, [FromForm]UpdateAdminRequest updateAdminRequest)
        {
            var admin = await dbContext.Admin.FindAsync(Id);

            if (admin != null)
            {
                admin.Name = updateAdminRequest.Name;
                admin.Role = updateAdminRequest.Role;
                admin.Email = updateAdminRequest.Email;
                admin.Password = updateAdminRequest.Password;
                admin.LastLogin = DateTime.Now;
                admin.LastUpdated = DateTime.Now;

                await dbContext.SaveChangesAsync();
                return Ok(admin);
            }
            return NotFound();
        }

        [HttpPut]
        [Route("/approve-document{Id:guid}")]
        public async Task<IActionResult> ApproveDocument([FromRoute] Guid Id, [FromForm] UpdateDocumentRequest updateDocumentRequest)
        {
            var document = await dbContext.Document.FindAsync(Id);

            if (document != null)
            {
                document.Status = updateDocumentRequest.Status;
                document.LastUpdated = DateTime.Now;

                await dbContext.SaveChangesAsync();
                return Ok(document);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("/remove-profile{Id:guid}")]
        public async Task<IActionResult> DeleteAdmin([FromRoute] Guid Id)
        {
            var admin = await dbContext.Admin.FindAsync(Id);

            if (admin != null)
            {
                dbContext.Remove(admin);
                await dbContext.SaveChangesAsync();
                return Ok(admin);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("/remove-user{Id:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid Id)
        {
            var user = await dbContext.User.FindAsync(Id);

            if (user != null)
            {
                dbContext.Remove(user);
                await dbContext.SaveChangesAsync();
                return Ok(user);
            }
            return NotFound();
        }
    }
}
