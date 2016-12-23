using System;
using System.Collections.Generic;
using WebTask.DataAccess;

namespace WebTask.Models
{
    public class CommentsViewModel
    {
        public List<List<Comment>> Comment { get; set; }
        public List<Guid> States { get; set; }
    }
}