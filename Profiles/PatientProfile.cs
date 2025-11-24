using group_12_ahmed_yu__GroupProject.dto;
using group_12_ahmed_yu__GroupProject.Models;

namespace group_12_ahmed_yu__GroupProject.Profiles
{
    public class PatientProfile : AutoMapper.Profile
    {
        public PatientProfile()
        {
            CreateMap<Models.Patient, dto.PatientDto>()
                 .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.CreatedAt));
            //Create PatientDto to Patient(for POST)
            CreateMap<CreatePatientDto, Patient>()
                .ForMember(dest => dest.PatientId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());
            //Update PatientDto to Patient(for PUT)
            CreateMap<UpdatePatientDto, Patient>()
                .ForMember(dest => dest.PatientId, opt => opt.Ignore())
                .ForMember(dest => dest.MedicalRecordNumber, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());
            //Patch PatientDto to Patient(for PATCH)
            CreateMap<PatchPatientDto, Patient>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
