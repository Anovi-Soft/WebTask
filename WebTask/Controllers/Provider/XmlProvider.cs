using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using WebTask.DataAccess;
using WebTask.Models;
using XmlComment = WebTask.Models.XmlComment;

namespace WebTask.Controllers.Provider
{
    public class XmlProvider
    {
        public byte[] Get(List<List<Comment>> comments, List<LikeViewModel> likes)
        {
            var photoModels = comments.Zip(likes, ResultSelector)
                .Select((x, i) =>
                {
                    x.PhotoId = i;
                    return x;
                })
                .ToList();
            var albumModel = new XmlAlbumModel
            {
                DownloadTime = DateTime.Now.ToString("f"),
                Photos = photoModels
            };
            var xml = Serialize(albumModel);
            return Encoding.ASCII.GetBytes(xml);
        }

        private static XmlPhotoModel ResultSelector(List<Comment> comments, LikeViewModel likes)
        {
            return new XmlPhotoModel
            {
                Comments = comments.Select(x=>new XmlComment {Login = x.Name, Text = x.Text}).ToList(),
                LikeCount = likes.Count
            };
        }

        private static string Serialize(XmlAlbumModel albumModel)
        {
            var xsSubmit = new XmlSerializer(typeof(XmlAlbumModel));
            using (var sww = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sww))
            {
                xsSubmit.Serialize(writer, albumModel);
                return sww.ToString();
            }
        }
    }
}