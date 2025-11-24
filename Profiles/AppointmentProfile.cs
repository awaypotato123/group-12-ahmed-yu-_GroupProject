using group_12_ahmed_yu__GroupProject.dto;
using group_12_ahmed_yu__GroupProject.Models;

namespace group_12_ahmed_yu__GroupProject.Profiles
{
    public class AppointmentProfile : AutoMapper.Profile
    {
        public AppointmentProfile()
        {
           
            CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.CreatedAt)) 
            .ForMember(dest => dest.Updated, opt => opt.MapFrom(src => src.UpdatedAt)) 
            .ForMember(dest => dest.PatientName,
                opt => opt.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName))
            .ForMember(dest => dest.DoctorName,
                opt => opt.MapFrom(src => src.Doctor.FirstName + " " + src.Doctor.LastName))
            .ForMember(dest => dest.Specialization,
                opt => opt.MapFrom(src => src.Doctor.Specialization));

            
            CreateMap<CreateAppointmentDto, Appointment>()
                .ForMember(dest => dest.AppointmentId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore());

            
            CreateMap<UpdateAppointmentDto, Appointment>()
                .ForMember(dest => dest.AppointmentId, opt => opt.Ignore())
                .ForMember(dest => dest.PatientId, opt => opt.Ignore())
                .ForMember(dest => dest.DoctorId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore());

            
            CreateMap<PatchAppointmentDto, Appointment>()
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}