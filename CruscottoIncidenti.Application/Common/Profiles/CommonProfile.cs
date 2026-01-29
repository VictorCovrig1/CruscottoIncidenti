using System.Linq;
using AutoMapper;
using CruscottoIncidenti.Application.User.Commands;
using CruscottoIncidenti.Application.User.ViewModels;

namespace CruscottoIncidenti.Application.Common.Profiles
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<UserViewModel, UpdateUserCommand>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom
                    (src => src.Roles.Where(r => r.IsSelected).Select(r => r.Id)));
        }
    }
}
