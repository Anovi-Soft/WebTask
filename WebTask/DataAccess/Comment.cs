using System;

namespace WebTask.DataAccess
{
    public class Comment: BaseModel
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public Guid AuthorId { get; set; }
    }
}