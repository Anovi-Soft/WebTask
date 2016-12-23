using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebTask.DataAccess;
using WebTask.Models;

namespace WebTask.Controllers.Access
{
    public class LikeAccess
    {
        public async Task<LikeViewModel> GetLikeModel(Guid userId, int photoId)
        {
            using (var context = new LikeDbContext())
            {
                var likes = await context.LikeSet
                    .Where(x => x.PhotoId == photoId)
                    .ToListAsync();
                return new LikeViewModel
                {
                    Count = likes.Count,
                    IsMy = likes.Any(y => y.AuthorId == userId)
                };
            }
        }

        public async Task<List<LikeViewModel>> GetAllLike(Guid userId, int photoCount)
        {
            using (var context = new LikeDbContext())
            {
                var quered = await context.LikeSet
                    .GroupBy(x => x.PhotoId)
                    .OrderBy(x => x.Key)
                    .ToDictionaryAsync(x => x.Key, x=>x.ToList());
                var buffer = new List<LikeModel>[photoCount];
                for (int i = 0; i < photoCount; i++)
                {
                    if (quered.ContainsKey(i))
                        buffer[i] = quered[i];
                    else
                        buffer[i] = new List<LikeModel>();
                }
                return buffer.Select((x,i)=> new LikeViewModel
                    {
                        PhotoId = i,
                        Count = x.Count,
                        IsMy = x.Any(y => y.AuthorId == userId)
                    })
                    .ToList();
            }
        }

        public async Task Like(Guid userId, int photoId)
        {
            using (var context = new LikeDbContext())
            {
                var like = await context.LikeSet
                    .FirstOrDefaultAsync(x => x.PhotoId == photoId && x.AuthorId == userId);
                if (like != null) return;
                context.LikeSet.Add(new LikeModel
                {
                    AuthorId = userId,
                    PhotoId = photoId
                });
                await context.SaveChangesAsync();
            }
        }
        public async Task Dislike(Guid userId, int photoId)
        {
            using (var context = new LikeDbContext())
            {
                var like = await context.LikeSet
                    .FirstOrDefaultAsync(x => x.PhotoId == photoId && x.AuthorId == userId);
                if (like == null) return;
                context.LikeSet.Remove(like);
                await context.SaveChangesAsync();
            }
        }
    }
}