using System.Web.Mvc;
using CruscottoIncidenti.Application.Interfaces;

namespace CruscottoIncidenti.Utils
{
    public class CustomPageBase : WebViewPage
    {
        public ICurrentUserService CurrentUserService { get; set; }

        public override void Execute()
        {
            
        }
    }
}