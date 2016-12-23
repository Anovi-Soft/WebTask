using System;

namespace WebTask.DataAccess
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        public DateTime Modify { get; set; }
    }
}