using group_12_ahmed_yu__GroupProject.dto;
using group_12_ahmed_yu__GroupProject.Models;

namespace group_12_ahmed_yu__GroupProject.Profiles
{
    public class AppointmentProfile : AutoMapper.Profile
    {
        public AppointmentProfile()
        {
            //Create Appointment to AppointmentDto
            CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.PatientName,
                opt => opt.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName))
            .ForMember(dest => dest.DoctorName,
                opt => opt.MapFrom(src => src.Doctor.FirstName + " " + src.Doctor.LastName))
            .ForMember(dest => dest.Specialization,
                opt => opt.MapFrom(src => src.Doctor.Specialization));
            //Create AppointmentDto to Appointment(for POST)
            CreateMap<CreateAppointmentDto, Appointment>()
                .ForMember(dest => dest.AppointmentId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Patient, opt => opt.Ignore())      
                .ForMember(dest => dest.Doctor, opt => opt.Ignore());
            //Update AppointmentDto to Appointment(for PUT)
            CreateMap<UpdateAppointmentDto, Appointment>()
                .ForMember(dest => dest.AppointmentId, opt => opt.Ignore())
                .ForMember(dest => dest.PatientId, opt => opt.Ignore())
                .ForMember(dest => dest.DoctorId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Patient, opt => opt.Ignore())      
                .ForMember(dest => dest.Doctor, opt => opt.Ignore());
            //Patch AppointmentDto to Appointment(for PATCH)
            CreateMap<PatchAppointmentDto, Appointment>()
                .ForMember(dest => dest.Patient, opt => opt.Ignore())      
                .ForMember(dest => dest.Doctor, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
       


    }
}
