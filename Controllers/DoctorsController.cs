using MAPay.Data;
using MAPay.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MAPay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DoctorsController : Controller
    {
        private readonly MAPayAPIDbContext dbContext;
        public DoctorsController(MAPayAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        [Route("/doctor-profile{Id:guid}")]
        public async Task<IActionResult> GetDoctor([FromRoute] Guid Id)
        {
            var doctor = await dbContext.Doctor.FindAsync(Id);
            if (doctor == null)
            {
                return NotFound();
            }
            return Ok(doctor);
        }

        [HttpPost]
        [Route("/doctor-signup")]
        public async Task<IActionResult> AddDoctor([FromForm] AddDoctorRequest addDoctorRequest)
        {
            var doctor = new DoctorSignUp()
            {
                Id = Guid.NewGuid(),
                Name = addDoctorRequest.Name,
                Role = addDoctorRequest.Role,
                Email = addDoctorRequest.Email,
                Password = addDoctorRequest.Password,
                LastLogin = DateTime.Now.Date,//addAdminRequest.LastLogin,
                LastUpdated = DateTime.Now.Date,//addAdminRequest.LastUpdated,
            };

            await dbContext.Doctor.AddAsync(doctor);
            await dbContext.SaveChangesAsync();

            return Ok(doctor);
        }

        [HttpPost]
        [Route("/doctor-login")]
        public async Task<IActionResult> DoctorLogin([FromForm] DoctorLogin doctorLogin)
        {
            var doctor = dbContext.Doctor.Where(x => x.Email == doctorLogin.Email &&
                                x.Password == doctorLogin.Password).FirstOrDefault();

            if (doctor != null)
            {
                doctor.LastLogin = DateTime.Now;
                return Ok("SuccessFully Login");
            }
            return BadRequest("Invalid User Login!!");
        }

        [HttpPut]
        [Route("/edit-doctor-profile{Id:guid}")]
        public async Task<IActionResult> UpdateDoctor([FromRoute] Guid Id, [FromForm] UpdateDoctorRequest updateDoctorRequest)
        {
            var doctor = await dbContext.Doctor.FindAsync(Id);

            if (doctor != null)
            {
                doctor.Name = updateDoctorRequest.Name;
                doctor.Role = updateDoctorRequest.Role;
                doctor.Email = updateDoctorRequest.Email;
                doctor.Password = updateDoctorRequest.Password;
                doctor.LastLogin = DateTime.Now;
                doctor.LastUpdated = DateTime.Now;

                await dbContext.SaveChangesAsync();
                return Ok(doctor);
            }
            return NotFound();
        }

        [HttpPut]
        [Route("/doctor/approve-document{Id:guid}")]
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
    }
}
