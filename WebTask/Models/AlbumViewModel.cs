using System.Collections.Generic;
using WebTask.DataAccess;

namespace WebTask.Models
{
    public class AlbumViewModel
    {
        public CommentsViewModel Comments { get; set; }
        public List<LikeViewModel> AllLike { get; set; }
    }
}