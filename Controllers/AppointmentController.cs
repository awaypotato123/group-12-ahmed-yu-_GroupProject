using AutoMapper;
using group_12_ahmed_yu__GroupProject.Data;
using Microsoft.AspNetCore.Http;
using group_12_ahmed_yu__GroupProject.Models;
using group_12_ahmed_yu__GroupProject.dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;



namespace group_12_ahmed_yu__GroupProject.Controllers
{
    //localhost:xxxx/api/Appointment
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public AppointmentController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments(
            [FromQuery] int? patientId = null,
            [FromQuery] int? doctorId = null,
            [FromQuery] string status = null,
            [FromQuery] DateTime? date = null)
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .AsQueryable();

            
            if (patientId.HasValue)
            {
                query = query.Where(a => a.PatientId == patientId.Value);
            }

            
            if (doctorId.HasValue)
            {
                query = query.Where(a => a.DoctorId == doctorId.Value);
            }

            
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(a => a.Status == status);
            }

            
            if (date.HasValue)
            {
                query = query.Where(a => a.AppointmentDate.Date == date.Value.Date);
            }

            var appointments = await query
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToListAsync();

            var appointmentDtos = _mapper.Map<List<AppointmentDto>>(appointments);

            return Ok(appointmentDtos);

        }
        //get a appointment by id
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetAppointment(int id)
        {
            // Find appointment in database
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound(new { message = $"Appointment with ID {id} not found" });
            }
            // Map Appointment model to AppointmentDto
            var appointmentDto = _mapper.Map<AppointmentDto>(appointment);
            return Ok(appointmentDto);
        }
        //create a new appointment
        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> CreateAppointment([FromBody] CreateAppointmentDto createAppointmentDto)
        {
            
            var patient = await _context.Patients.FindAsync(createAppointmentDto.PatientId);
            if (patient == null)
            {
                return NotFound(new { message = $"Patient with ID {createAppointmentDto.PatientId} not found" });
            }
            
            var doctor = await _context.Doctors.FindAsync(createAppointmentDto.DoctorId);
            if (doctor == null)
            {
                return NotFound(new { message = $"Doctor with ID {createAppointmentDto.DoctorId} not found" });
            }
            
            var doctorConflict = await _context.Appointments
       .AnyAsync(a =>
           a.DoctorId == createAppointmentDto.DoctorId &&
           a.AppointmentDate == createAppointmentDto.AppointmentDate &&
           a.AppointmentTime == createAppointmentDto.AppointmentTime &&
           a.Status != "Cancelled");

            if (doctorConflict)
            {
                return Conflict(new { message = "Doctor is already booked at this time" });
            }

           
            var patientConflict = await _context.Appointments
                .AnyAsync(a =>
                    a.PatientId == createAppointmentDto.PatientId &&
                    a.AppointmentDate == createAppointmentDto.AppointmentDate &&
                    a.AppointmentTime == createAppointmentDto.AppointmentTime &&
                    a.Status != "Cancelled");
            if (patientConflict)
            {
                return Conflict(new { message = "Patient already has an appointment at this time" });
            }
            
            var appointment = _mapper.Map<Appointment>(createAppointmentDto);
            appointment.CreatedAt = DateTime.UtcNow;
            appointment.UpdatedAt = DateTime.UtcNow;
            
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            
            var appointmentDto = _mapper.Map<AppointmentDto>(appointment);
            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.AppointmentId }, appointmentDto);
        }
        //replace a existing appointment
        [HttpPut("{id}")]
        public async Task<ActionResult<AppointmentDto>> UpdateAppointment(int id, [FromBody] UpdateAppointmentDto updateAppointmentDto)
        {
           
            var existingAppointment = await _context.Appointments.FindAsync(id);
            if (existingAppointment == null)
            {
                return NotFound(new { message = $"Appointment with ID {id} not found" });
            }

            
            if (existingAppointment.Status == "Completed")
            {
                return BadRequest(new { message = "Cannot update completed appointments" });
            }

            
            if (updateAppointmentDto.AppointmentDate != existingAppointment.AppointmentDate ||
                updateAppointmentDto.AppointmentTime != existingAppointment.AppointmentTime)
            {
               
                var doctorConflict = await _context.Appointments
                    .AnyAsync(a =>
                        a.DoctorId == existingAppointment.DoctorId &&
                        a.AppointmentDate == updateAppointmentDto.AppointmentDate &&
                        a.AppointmentTime == updateAppointmentDto.AppointmentTime &&
                        a.AppointmentId != id &&  
                        a.Status != "Cancelled");

                if (doctorConflict)
                {
                    return Conflict(new { message = "Doctor is already booked at this time" });
                }

               
                var patientConflict = await _context.Appointments
                    .AnyAsync(a =>
                        a.PatientId == existingAppointment.PatientId &&
                        a.AppointmentDate == updateAppointmentDto.AppointmentDate &&
                        a.AppointmentTime == updateAppointmentDto.AppointmentTime &&
                        a.AppointmentId != id &&  
                        a.Status != "Cancelled");

                if (patientConflict)
                {
                    return Conflict(new { message = "Patient already has an appointment at this time" });
                }
            }

            
            _mapper.Map(updateAppointmentDto, existingAppointment);
            existingAppointment.UpdatedAt = DateTime.UtcNow;

            _context.Entry(existingAppointment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var appointmentDto = _mapper.Map<AppointmentDto>(existingAppointment);
            return Ok(appointmentDto);

            


        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAppointment(int id, [FromBody] PatchAppointmentDto patchDto)
        {
            
            var existing = await _context.Appointments.FindAsync(id);
            if (existing == null)
            {
                return NotFound(new { message = $"Appointment with ID {id} not found" });
            }

            
            if (existing.Status == "Completed")
            {
                return BadRequest(new { message = "Cannot update completed appointments" });
            }

            
            bool dateOrTimeChanging = false;

            if (patchDto.AppointmentDate.HasValue && patchDto.AppointmentDate.Value != existing.AppointmentDate)
            {
                dateOrTimeChanging = true;
            }

            if (patchDto.AppointmentTime.HasValue && patchDto.AppointmentTime.Value != existing.AppointmentTime)
            {
                dateOrTimeChanging = true;
            }

            
            if (dateOrTimeChanging)
            {
                
                var newDate = patchDto.AppointmentDate ?? existing.AppointmentDate;
                var newTime = patchDto.AppointmentTime ?? existing.AppointmentTime;

                
                var doctorConflict = await _context.Appointments
                    .AnyAsync(a =>
                        a.DoctorId == existing.DoctorId &&
                        a.AppointmentDate == newDate &&
                        a.AppointmentTime == newTime &&
                        a.AppointmentId != id &&
                        a.Status != "Cancelled");

                if (doctorConflict)
                {
                    return Conflict(new { message = "Doctor is already booked at this time" });
                }

                
                var patientConflict = await _context.Appointments
                    .AnyAsync(a =>
                        a.PatientId == existing.PatientId &&
                        a.AppointmentDate == newDate &&
                        a.AppointmentTime == newTime &&
                        a.AppointmentId != id &&
                        a.Status != "Cancelled");

                if (patientConflict)
                {
                    return Conflict(new { message = "Patient already has an appointment at this time" });
                }
            }

           
            if (patchDto.AppointmentDate.HasValue)
            {
                existing.AppointmentDate = patchDto.AppointmentDate.Value;
            }

            if (patchDto.AppointmentTime.HasValue)
            {
                existing.AppointmentTime = patchDto.AppointmentTime.Value;
            }

            if (!string.IsNullOrWhiteSpace(patchDto.Status))
            {
                existing.Status = patchDto.Status;
            }

            if (!string.IsNullOrWhiteSpace(patchDto.ReasonForVisit))
            {
                existing.ReasonForVisit = patchDto.ReasonForVisit;
            }

            if (!string.IsNullOrWhiteSpace(patchDto.Notes))
            {
                existing.Notes = patchDto.Notes;
            }

           
            existing.UpdatedAt = DateTime.UtcNow;

            _context.Entry(existing).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var appointmentDto = _mapper.Map<AppointmentDto>(existing);
            return Ok(appointmentDto);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound(new { message = $"Appointment with ID {id} not found" });
            }

           
            if (appointment.Status == "Completed")
            {
                return BadRequest(new { message = "Cannot delete completed appointments" });
            }

            
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }



    }
}
