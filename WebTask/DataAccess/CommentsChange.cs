using System;
using System.Collections.Generic;

namespace WebTask.DataAccess
{
    public class CommentsChange: BaseModel
    {
        public int PhotoId { get; set; }
        public ChangeStatus Status { get; set; }
        public virtual Comment Comment { get; set; }
    }

    public enum ChangeStatus
    {
        Add,
        Remove
    }
}