using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
using Microsoft.AspNet.Identity;
using WebTask.Controllers.Access;
using WebTask.Controllers.Format;
using WebTask.Controllers.Provider;
using WebTask.Models;

namespace WebTask.Controllers
{
    public class PhotoController: Controller
    {
        private readonly CommentsAccess commentsAccess = new CommentsAccess();
        private readonly LikeAccess likeAccess = new LikeAccess();
        private readonly XmlProvider xmlProvider = new XmlProvider();
        private readonly HtmlMessageFormat htmlMessageFormat = new HtmlMessageFormat();
        private const int ImageCount = 18;
        private Guid AuthorId => User.Identity.IsAuthenticated
            ? Guid.Parse(User.Identity.GetUserId())
            : Guid.Empty;

        public async Task<ActionResult> Index()
        {
            var model = new AlbumViewModel
            {
                Comments = commentsAccess.GetAll(ImageCount),
                AllLike = await likeAccess.GetAllLike(AuthorId, ImageCount)
            };
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> GetLike(int photoId)
        {
            var likeModel = await likeAccess.GetLikeModel(AuthorId, photoId);
            return Json(likeModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeLike(int photoId, bool isSelected)
        {
            if (isSelected)
                await likeAccess.Like(AuthorId, photoId);
            else
                await likeAccess.Dislike(AuthorId, photoId);

            return await GetLike(photoId);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> SendComment(int photoId, string text)
        {
            var name = User.Identity.Name;
            text = htmlMessageFormat.Format(text).Trim();
            if (text.IsEmpty())
                throw new ArgumentException("string empty");
            var addCommentModel = await commentsAccess.AddComment(photoId, AuthorId, name, text);
            return Json(addCommentModel);
        }

        [HttpPost]
        public async Task<ActionResult> RemoveComment(int photoId, Guid commentId)
        {
            var stateId = await commentsAccess.RemoveComment(photoId, AuthorId, commentId);
            return Json(stateId);
        }

        [HttpGet]
        public async Task<ActionResult> UpdateComments(int photoId, Guid state)
        {
            var changes = await commentsAccess.GetChangesAsync(photoId, state);
            return Json(changes, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> DownloadXml()
        {
            var comments = commentsAccess.GetAll(ImageCount).Comment;
            var likes = await likeAccess.GetAllLike(AuthorId, ImageCount);
            var result = xmlProvider.Get(comments, likes);
            return File(result, "text/xml");
        }
    }
}