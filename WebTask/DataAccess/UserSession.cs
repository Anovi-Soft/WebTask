using System;

namespace WebTask.DataAccess
{
    public class UserSession : BaseModel
    {
        public Guid? UserId { get; set; }
        public string UserIp { get; set; }
        public string Path { get; set; }
    }
}