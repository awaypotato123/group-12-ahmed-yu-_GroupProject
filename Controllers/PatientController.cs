using AutoMapper;
using group_12_ahmed_yu__GroupProject.Data;
using group_12_ahmed_yu__GroupProject.dto;
using group_12_ahmed_yu__GroupProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace group_12_ahmed_yu__GroupProject.Controllers
{
    //localhost:xxxx/api/patient
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PatientController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //get a list of patients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients([FromQuery] string searchTerm = null)
        {
            var query = _context.Patients.Where(p => p.IsActive);

            //search filter
            if(!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.FirstName.Contains(searchTerm) || p.LastName.Contains(searchTerm) || p.Email.Contains(searchTerm));
            }
            // Get patients from database
            var patients = await _context.Patients.ToListAsync();

            // Map Patient models to PatientDto
            var patientDtos = _mapper.Map<List<PatientDto>>(patients);

            return Ok(patientDtos);
        }

        //get a single patient by id
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetPatient(int id)
        {
            // Find patient in database
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            // Map Patient model to PatientDto
            var patientDto = _mapper.Map<PatientDto>(patient);

            return Ok(patientDto);
        }

        //create a new patient
        [HttpPost]
        public async Task<ActionResult<PatientDto>> CreatePatient(CreatePatientDto createPatientDto)
        {
            //check for duplicate email
            if(await _context.Patients.AnyAsync(p => p.Email == createPatientDto.Email))
            {
                return Conflict("A patient with the same email already exists.");
            }
            //check for duplicate mrn
            if (await _context.Patients.AnyAsync(p => p.MedicalRecordNumber == createPatientDto.MedicalRecordNumber))
            {
                return Conflict("A patient with the same medical record number already exists.");
            }

            var patient = _mapper.Map<Patient>(createPatientDto);

            patient.CreatedAt = DateTime.UtcNow;
            patient.IsActive = true;


            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

           
            var patientDto = _mapper.Map<PatientDto>(patient);

            
            return CreatedAtAction(nameof(GetPatient), new { id = patient.PatientId }, patientDto);
        }
        //replace a existing patient entirely
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, UpdatePatientDto updatePatientDto)
        {
            // Find patient
            var patient = await _context.Patients.FindAsync(id);

            // Check if patient exists and is active
            if (patient == null || !patient.IsActive)
            {
                return NotFound(new { message = $"Patient with ID {id} not found" });
            }

           
            if (updatePatientDto.Email != patient.Email)
            {
                if (await _context.Patients.AnyAsync(p => p.Email == updatePatientDto.Email && p.PatientId != id))
                {
                    return Conflict(new { message = "A patient with this email already exists" });
                }
            }
            
            _mapper.Map(updatePatientDto, patient);

            _context.Entry(patient).State = EntityState.Modified;
            await _context.SaveChangesAsync();

           
            var patientDto = _mapper.Map<PatientDto>(patient);
            return Ok(patientDto);
        }
        //update part of a existing patient
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchPatient(int id, PatchPatientDto patchPatientDto)
        {
            // Find patient
            var patient = await _context.Patients.FindAsync(id);
            // Check if patient exists and is active
            if (patient == null || !patient.IsActive)
            {
                return NotFound(new { message = $"Patient with ID {id} not found" });
            }

            // Update only provided fields (not null)
            if (!string.IsNullOrWhiteSpace(patchPatientDto.FirstName))
            {
                patient.FirstName = patchPatientDto.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(patchPatientDto.LastName))
            {
                patient.LastName = patchPatientDto.LastName;
            }

            if (!string.IsNullOrWhiteSpace(patchPatientDto.Email))
            {
                
                if (patchPatientDto.Email != patient.Email)
                {
                    if (await _context.Patients.AnyAsync(p => p.Email == patchPatientDto.Email && p.PatientId != id))
                    {
                        return Conflict(new { message = "A patient with this email already exists" });
                    }
                }
                patient.Email = patchPatientDto.Email;
            }

            if (!string.IsNullOrWhiteSpace(patchPatientDto.PhoneNumber))
            {
                patient.PhoneNumber = patchPatientDto.PhoneNumber;
            }

            if (!string.IsNullOrWhiteSpace(patchPatientDto.Address))
            {
                patient.Address = patchPatientDto.Address;
            }

            if (patchPatientDto.DateOfBirth.HasValue)
            {
                patient.DateOfBirth = patchPatientDto.DateOfBirth.Value;
            }

            _context.Entry(patient).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var patientDto = _mapper.Map<PatientDto>(patient);
            return Ok(patientDto);
        }
        //delete a patient (hard delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            // Find patient
            var patient = await _context.Patients.FindAsync(id);
            // Check if patient exists and is active
            if (patient == null || !patient.IsActive)
            {
                return NotFound(new { message = $"Patient with ID {id} not found" });
            }
            //check if patient has pending appointments(to be implemented)
            /*
            var hasPendingAppointments = await _context.Appointments
                .AnyAsync(a => a.PatientId == id && a.Status == "Pending" || a.Status = "Confirmed"));
            if (hasPendingAppointments)
            {
                return Conflict(new { message = "Cannot delete patient with pending appointments" });
            }
            */
           
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}