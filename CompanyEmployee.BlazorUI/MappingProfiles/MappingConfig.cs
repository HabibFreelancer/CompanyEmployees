using AutoMapper;
using CompanyEmployee.BlazorUI.Models.Company;
using CompanyEmployee.BlazorUI.Services.Base;

namespace CompanyEmployee.BlazorUI.MappingProfiles
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<CompanyDto, CompanyVM>().ReverseMap();
            CreateMap<CompanyForCreationDto, CompanyVM>().ReverseMap();
            CreateMap<CompanyForUpdateDto, CompanyVM>().ReverseMap();

        }
    }
}
