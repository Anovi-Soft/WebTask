using System.Threading.Tasks;
using System.Web.Mvc;
using WebTask.DataAccess;

namespace WebTask.Controllers
{
    public class RestartController: Controller
    {
        public async Task<ActionResult> Index()
        {
            using (var context = new UserSessionDbContext())
            {
                context.UserSessions.RemoveRange(context.UserSessions);
                await context.SaveChangesAsync();
            }
            using (var context = new PhotoDbContext())
            {
                context.Comments.RemoveRange(context.Comments);
                context.CommentsHistory.RemoveRange(context.CommentsHistory);
                await context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}