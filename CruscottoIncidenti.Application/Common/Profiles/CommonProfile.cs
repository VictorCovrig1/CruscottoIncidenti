using System;
using System.Linq;
using AutoMapper;
using CruscottoIncidenti.Application.Incidents.ViewModels;
using CruscottoIncidenti.Application.Users.ViewModels;
using CruscottoIncidenti.Common;

namespace CruscottoIncidenti.Application.Common.Profiles
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            //CreateMap<UserViewModel, UpdateUserViewModel>()
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Roles, opt => opt.MapFrom
            //        (src => src.Roles.Where(r => r.IsSelected).Select(r => r.Id)));

            //RequestType type;
            //Urgency urgency;
            //CreateMap<IncidentViewModel, UpdateIncidentViewModel>()
            //    .ForMember(dest => dest.IncidentId, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.TryParse(src.Type, out type) ? type : 0))
            //    .ForMember(dest => dest.Urgency, opt => opt.MapFrom(src => Enum.TryParse(src.Urgency, out urgency) ? urgency : 0));
        }
    }
}
