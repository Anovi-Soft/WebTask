using System;

namespace WebTask.Models
{
    public class AddCommentModel
    {
        public Guid CommentId { get; set; }
        public Guid NewState { get; set; }
    }
}