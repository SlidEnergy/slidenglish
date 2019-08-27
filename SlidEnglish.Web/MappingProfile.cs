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

            CreateMap<App.Dto.Word, Word>()
                .ForMember(dest => dest.Text,
                    opt => opt.MapFrom(src => src.Text ?? ""))
                .ForMember(dest => dest.Association,
                    opt => opt.MapFrom(src => src.Association ?? ""))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description ?? ""))
                .ForAllOtherMembers(opt => opt.Ignore());

			CreateMap<Word, App.Dto.Word>()
				.ForMember(dest => dest.Synonyms,
					opt => opt.MapFrom(src => src.AllSynonyms.Select(x => x.Id).ToArray()));
		}
    }
}
