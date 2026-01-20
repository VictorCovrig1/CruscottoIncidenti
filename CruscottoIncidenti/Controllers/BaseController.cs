using System.Web.Mvc;
using AutoMapper;
using MediatR;

namespace CruscottoIncidenti.Controllers
{
    public class BaseController : Controller
    {
        protected IMediator Mediator => DependencyResolver.Current.GetService<IMediator>();

        protected IMapper Mapper => DependencyResolver.Current.GetService<IMapper>();
    }
}