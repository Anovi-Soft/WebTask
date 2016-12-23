using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebTask.Controllers.Access;
using WebTask.Controllers.Format;
using WebTask.Controllers.Provider;
using WebTask.Extensions;

namespace WebTask.Controllers
{
    public class UserInfoController: Controller
    {
        private readonly UserInfoTextFormat formater = new UserInfoTextFormat();
        private readonly ImageProvider imageProvider = new ImageProvider();
        private readonly SessionAccess sessionAccess = new SessionAccess();

        public async Task<ActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                RedirectToAction("Index", "Home");
            }
            var userId = User.Identity.GetUserId();
            var sessions = await sessionAccess.GetUserSessions(userId);
            return View(sessions);
        }

        public ActionResult UsersCount()
        {
            var usersCount = sessionAccess.GetUsersCount();
            var text = formater.FormatUserCount(usersCount);
            var image = imageProvider.GetImage(text);
            return File(image, "image/png");
        }

        public ActionResult UsersCountExtended(string path)
        {
            var usersCount = sessionAccess.GetUsersCount();
            var id = Guid.Parse(User.Identity.GetUserId());
            var session = sessionAccess.GetLastVisit(id, path);
            var visit = formater.FormatLastVisit(session);
            var text = formater.FormatUserCount(usersCount, visit);
            var image = imageProvider.GetImage(text);
            return File(image, "image/png");
        }

        public ActionResult UsersInfo(int width, int height)
        {
            var clientIp = Request.GetClientIp();
            var browser = Request.Browser.Browser;
            var browserVersion = Request.Browser.MajorVersion;
            var text = formater.FormatBrowserInfo(clientIp, width, height, browser, browserVersion);
            var image = imageProvider.GetImage(text);
            return File(image, "image/png");
        }
        
    }
}