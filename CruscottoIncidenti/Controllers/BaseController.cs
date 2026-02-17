using System.Web.Mvc;
using MediatR;

namespace CruscottoIncidenti.Controllers
{
    public class BaseController : Controller
    {
        protected IMediator Mediator => DependencyResolver.Current.GetService<IMediator>();
    }
}