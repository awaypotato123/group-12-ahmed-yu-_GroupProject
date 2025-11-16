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
            var query = _context.Doctors.AsQueryable();
            //search filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(d => d.FirstName.Contains(searchTerm) || d.LastName.Contains(searchTerm) || d.Email.Contains(searchTerm) || d.Specialization.Contains(searchTerm));
            }
            // Get doctors from database
            var doctors = await query.ToListAsync();
            // Map Doctor models to DoctorDto
            var doctorDtos = _mapper.Map<List<DoctorDto>>(doctors);
            return Ok(doctorDtos);
        }
        //get a single doctor by id
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDto>> GetDoctor(int id)
        {
            // Find doctor in database
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            // Map Doctor model to DoctorDto
            var doctorDto = _mapper.Map<DoctorDto>(doctor);
            return Ok(doctorDto);
        }
        //create a new doctor
        [HttpPost]
        public async Task<ActionResult<DoctorDto>> CreateDoctor([FromBody] CreateDoctorDto createDoctorDto)
        {
            //check if email already exists
            if(await _context.Doctors.AnyAsync(d => d.Email == createDoctorDto.Email))
            {
                return Conflict("A doctor with the same email already exists.");
            }
            //check if license number already exists
            if (await _context.Doctors.AnyAsync(d => d.LicenseNumber == createDoctorDto.LicenseNumber))
            {
                return Conflict("A doctor with the same license number already exists.");
            }
            // Map DoctorDto to Doctor model
            var doctor = _mapper.Map<Doctor>(createDoctorDto);
            doctor.CreatedAt = DateTime.UtcNow;
           
            // Add doctor to database
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            // Map Doctor model back to DoctorDto
            var createdDoctorDto = _mapper.Map<DoctorDto>(doctor);
            return CreatedAtAction(nameof(GetDoctor), new { id = createdDoctorDto.DoctorId }, createdDoctorDto);
        }
        //replace an existing doctor
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] UpdateDoctorDto updateDoctorDto)
        {
            // Find existing doctor
            var existingDoctor = await _context.Doctors.FindAsync(id);
            if (existingDoctor == null)
            {
                return NotFound();
            }
            // Check for email conflict
            // Only check if email is being changed
            if (updateDoctorDto.Email != existingDoctor.Email)
            {
                if (await _context.Doctors.AnyAsync(d => d.Email == updateDoctorDto.Email && d.DoctorId != id))
                {
                    return Conflict(new { message = "A doctor with this email already exists" });
                }
            }
            // Map updated fields to existing doctor
            _mapper.Map(updateDoctorDto, existingDoctor);
            // Save changes to database
            await _context.SaveChangesAsync();
            var doctorDto = _mapper.Map<DoctorDto>(existingDoctor);
            return Ok(doctorDto);
            
        }
        //partial update an existing doctor
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDoctor(int id, [FromBody] PatchDoctorDto patchDoctorDto)
        {
            // Find existing doctor
            var existingDoctor = await _context.Doctors.FindAsync(id);
            if (existingDoctor == null)
            {
                return NotFound(new { message = $"Doctor with ID {id} not found" });
            }

            // Update only provided fields
            if (!string.IsNullOrWhiteSpace(patchDoctorDto.FirstName))
            {
                existingDoctor.FirstName = patchDoctorDto.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(patchDoctorDto.LastName))
            {
                existingDoctor.LastName = patchDoctorDto.LastName;
            }

            if (!string.IsNullOrWhiteSpace(patchDoctorDto.Email))
            {
                // Check duplicate email only if it's being changed
                if (patchDoctorDto.Email != existingDoctor.Email)
                {
                    if (await _context.Doctors.AnyAsync(d => d.Email == patchDoctorDto.Email && d.DoctorId != id))
                    {
                        return Conflict(new { message = "A doctor with this email already exists" });
                    }
                }
                existingDoctor.Email = patchDoctorDto.Email;
            }

            if (!string.IsNullOrWhiteSpace(patchDoctorDto.PhoneNumber))
            {
                existingDoctor.PhoneNumber = patchDoctorDto.PhoneNumber;
            }

            if (!string.IsNullOrWhiteSpace(patchDoctorDto.Specialization))
            {
                existingDoctor.Specialization = patchDoctorDto.Specialization;
            }

            if (patchDoctorDto.ConsultationFee.HasValue)
            {
                existingDoctor.ConsultationFee = patchDoctorDto.ConsultationFee.Value;
            }

            _context.Entry(existingDoctor).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var doctorDto = _mapper.Map<DoctorDto>(existingDoctor);
            return Ok(doctorDto);
        }
        //delete a doctor
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            // Find existing doctor
            var existingDoctor = await _context.Doctors.FindAsync(id);
            if (existingDoctor == null)
            {
                return NotFound();
            }
            //prevent delete if the doctor has appointments
            /*var hasPendingAppointments = await _context.Appointments
        .AnyAsync(a => a.DoctorId == id &&
                      (a.Status == "Pending" || a.Status == "Confirmed"));

            if (hasPendingAppointments)
            {
                return BadRequest(new { message = "Cannot delete doctor with pending appointments" });
            }
            */
            // Remove doctor from database
            _context.Doctors.Remove(existingDoctor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
