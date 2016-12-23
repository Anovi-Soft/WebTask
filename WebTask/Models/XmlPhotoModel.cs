using System.Collections.Generic;

namespace WebTask.Models
{
    public class XmlPhotoModel
    {
        public int PhotoId { get; set; }
        public int LikeCount { get; set; }
        public List<XmlComment> Comments { get; set; }
    }
}