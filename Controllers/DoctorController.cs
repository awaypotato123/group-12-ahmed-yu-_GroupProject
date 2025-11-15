using AutoMapper;
using group_12_ahmed_yu__GroupProject.Data;
using group_12_ahmed_yu__GroupProject.dto;
using group_12_ahmed_yu__GroupProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace group_12_ahmed_yu__GroupProject.Controllers
{
    //localhost:xxxx/api/Doctor
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
       
        public DoctorController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //get a list of doctors
       [HttpGet]
       public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors([FromQuery] string searchTerm = null)
       {
            var query = _context.Doctors.Where(d => d.IsActive);
            //search filter
            if(!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(d => d.FirstName.Contains(searchTerm) || d.LastName.Contains(searchTerm) || d.Email.Contains(searchTerm) || d.Specialization.Contains(searchTerm));
            }
            // Get doctors from database
            var doctors = await query.ToListAsync();
            // Map Doctor models to DoctorDto
            var doctorDtos = _mapper.Map<List<DoctorDto>>(doctors);
            return Ok(doctorDtos);
        }
    }
}
