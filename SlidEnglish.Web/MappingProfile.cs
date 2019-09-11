using AutoMapper;
using SlidEnglish.App;
using SlidEnglish.Domain;
using SlidEnglish.Infrastructure;
using System.Linq;

namespace SlidEnglish.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile(ApplicationDbContext context)
        {
            CreateMap<RegisterBindingModel, User>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<User, Dto.User>();

            CreateMap<App.Dto.LexicalUnit, LexicalUnit>()
                .ForMember(dest => dest.Text,
                    opt => opt.MapFrom(src => src.Text ?? ""))
                .ForMember(dest => dest.Association,
                    opt => opt.MapFrom(src => src.Association ?? ""))
                .ForMember(dest => dest.Notes,
                    opt => opt.MapFrom(src => src.Notes ?? ""))
                .ForMember(dest => dest.Translation,
                    opt => opt.MapFrom(src => src.Translation ?? ""))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<LexicalUnit, App.Dto.LexicalUnit>()
                .ForMember(dest => dest.RelatedLexicalUnits,
                    opt => opt.MapFrom(src => src.AllRelatedLexicalUnits.Select(x => new LexicalUnitRelation(x.LexicalUnit, x.Attribute)).ToArray()));


            CreateMap<LexicalUnitRelation, App.Dto.LexicalUnitRelation>()
                .ForMember(dest => dest.LexicalUnitId,
                    opt => opt.MapFrom(src => src.LexicalUnit.Id));

            CreateMap<ExampleOfUse, App.Dto.ExampleOfUse>();
            CreateMap<App.Dto.ExampleOfUse, ExampleOfUse>()
                .ForMember(dest => dest.LexicalUnitId, opt => opt.Ignore())
                .ForMember(dest => dest.LexicalUnit, opt => opt.Ignore());
        }
    }
}
