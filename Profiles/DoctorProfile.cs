using group_12_ahmed_yu__GroupProject.dto;
using group_12_ahmed_yu__GroupProject.Models;

namespace group_12_ahmed_yu__GroupProject.Profiles
{
    public class DoctorProfile : AutoMapper.Profile
    {
        public DoctorProfile()
        {
            CreateMap<Models.Doctor, dto.DoctorDto>();
            //Create DoctorDto to Doctor(for POST)
            CreateMap<CreateDoctorDto, Doctor>()
                .ForMember(dest => dest.DoctorId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                
            //Update DoctorDto to Doctor(for PUT)
            CreateMap<UpdateDoctorDto, Doctor>()
                .ForMember(dest => dest.DoctorId, opt => opt.Ignore())
                .ForMember(dest => dest.LicenseNumber, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                
            //Patch DoctorDto to Doctor(for PATCH)
            CreateMap<PatchDoctorDto, Doctor>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
