using System;

namespace WebTask.DataAccess
{
    public class LikeModel: BaseModel
    {
        public Guid AuthorId { get; set; }
        public int PhotoId { get; set; }
    }
}