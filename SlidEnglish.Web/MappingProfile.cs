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
				.ForMember(dest => dest.Association,
					opt => opt.MapFrom(src => src.Association ?? ""))
				.ForMember(dest => dest.Description,
					opt => opt.MapFrom(src => src.Description ?? ""))
				.ForMember(dest => dest.Sinonyms,
					opt => opt.Ignore())
    //            .ForMember(dest => dest.Sinonyms,
    //                opt => opt.MapFrom<WordSinonym[]>((from, to) => {
				//		var currentWord = context.Words.Find(from.Id);
				//		var synonyms = context.Words.Where(x => from.Synonyms.Contains(x.Id)).ToArray();
				//		var links = synonyms.Select(x => new WordSinonym(currentWord, x)).ToArray();
				//		return links;
				//	})
				//)
                .ForMember(dest => dest.SinonymOf,
                    opt => opt.Ignore())
                .ForMember(dest => dest.User,
                    opt => opt.Ignore());

			CreateMap<App.Dto.EditWordDto, Word>()
				.ForMember(dest => dest.Association,
					opt => opt.MapFrom(src => src.Association ?? ""))
				.ForMember(dest => dest.Description,
					opt => opt.MapFrom(src => src.Description ?? ""))
				.ForMember(dest => dest.Sinonyms,
					opt => opt.Ignore())
				//            .ForMember(dest => dest.Sinonyms,
				//                opt => opt.MapFrom<WordSinonym[]>((from, to) => {
				//		var currentWord = context.Words.Find(from.Id);
				//		var synonyms = context.Words.Where(x => from.Synonyms.Contains(x.Id)).ToArray();
				//		var links = synonyms.Select(x => new WordSinonym(currentWord, x)).ToArray();
				//		return links;
				//	})
				//)
				.ForMember(dest => dest.SinonymOf,
					opt => opt.Ignore())
				.ForMember(dest => dest.User,
					opt => opt.Ignore());

			CreateMap<Word, App.Dto.Word>()
				.ForMember(dest => dest.Synonyms,
					opt => opt.MapFrom(src => src.AllSinonyms.Select(x => x.Id).ToArray()));
		}
    }
}
