using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using WhiteBlackList.Web.Middleware;

namespace WhiteBlackList.Web.Filters
{
    // filterlar sayesinde bir controllerdaki herhangi bir metoda istek gelmeden önce veya sonra, filterlar sayesinde gelen istekleri yakalayabiliyoruz.
    // metoda veya controllera girmeden gelen isteği yakalıcam
    public class CheckWhiteList:ActionFilterAttribute
    {
        private readonly IPList _ipList;

        public CheckWhiteList(IOptions<IPList> ipList)
        {
            _ipList = ipList.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var reqIpAdress = context.HttpContext.Connection.RemoteIpAddress;
            var isWhiteList = _ipList.WhiteList.Where(x => IPAddress.Parse(x).Equals(reqIpAdress)).Any();

            if (!isWhiteList)
            {
                context.Result = new StatusCodeResult((int) HttpStatusCode.Forbidden);
                return;
            }


            base.OnActionExecuting(context);
        }
    }
}
