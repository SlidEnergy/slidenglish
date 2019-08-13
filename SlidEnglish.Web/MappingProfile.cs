﻿using AutoMapper;
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

            CreateMap<Dto.Word, Word>()
				.ForMember(dest => dest.Association,
					opt => opt.MapFrom(src => src.Association ?? ""))
				.ForMember(dest => dest.Description,
					opt => opt.MapFrom(src => src.Description ?? ""))
                .ForMember(dest => dest.Sinonyms,
                    opt => opt.Ignore())
                .ForMember(dest => dest.SinonymOf,
                    opt => opt.Ignore())
                .ForMember(dest => dest.User,
                    opt => opt.Ignore());

            CreateMap<Word, Dto.Word>();

            CreateMap<Word, Dto.DetailsWord>()
				.ForMember(dest => dest.Sinonyms,
					opt => opt.MapFrom(src => src.AllSinonyms));

            CreateMap<Dto.DetailsWord, Word>()
                .ForMember(dest => dest.Sinonyms, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.SinonymOf,
                    opt => opt.Ignore())
                .ForMember(dest => dest.User,
                    opt => opt.Ignore());
		}
    }
}
