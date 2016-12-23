using System;
using System.Collections.Generic;

namespace WebTask.Models
{
    public class XmlAlbumModel
    {
        public string DownloadTime { get; set; }
        public List<XmlPhotoModel> Photos { get; set; }
    }
}