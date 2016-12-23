using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebTask.DataAccess;
using WebTask.Models;

namespace WebTask.Controllers.Access
{
    public class CommentsAccess
    {
        public CommentsViewModel GetAll(int imgCount)
        {
            using (var context = new PhotoDbContext())
            {
                var history = context.CommentsHistory
                    .Include(x=>x.Comment)
                    .GroupBy(x => x.PhotoId)
                    .ToDictionary(x=>x.Key, x=>x.OrderBy(y=>y.Modify));
                var changes = new List<CommentsChange>[imgCount];
                for (var i = 0; i < imgCount; i++)
                {
                    if (history.ContainsKey(i))
                        changes[i] = history[i].ToList();
                    else
                        changes[i] = new List<CommentsChange>();
                }
                return new CommentsViewModel
                {
                    Comment = changes.Select(ConcatChanges).ToList(),
                    States = changes.Select(GetLastChangeId).ToList()
                };
            }
        }

        private static List<Comment> ConcatChanges(List<CommentsChange> changes)
        {
            var result = new List<Comment>();
            foreach (var change in changes)
            {
                switch (change.Status)
                {
                    case ChangeStatus.Add:
                        result.Add(change.Comment);
                        break;
                    case ChangeStatus.Remove:
                        result.RemoveAll(x => x.Id == change.Comment.Id);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return result;
        }

        private Guid GetLastChangeId(List<CommentsChange> changes)
        {
            return changes.LastOrDefault()?.Id ?? Guid.Empty;
        }

        public async Task<List<CommentsChange>> GetChangesAsync(int photoId, Guid from)
        {
            using (var context = new PhotoDbContext())
            {
                var dateFrom = DateTime.MinValue;
                if (from != Guid.Empty)
                {
                    var commentsChange = await context.CommentsHistory.FindAsync(from);
                    dateFrom = commentsChange.Modify;
                }
                return await context.CommentsHistory
                    .Where(x => x.PhotoId == photoId && x.Modify > dateFrom && x.Id != from)
                    .Include(x=>x.Comment)
                    .ToListAsync();
            }
        }

        public async Task<AddCommentModel> AddComment(int photoId, Guid authorId, string name, string text)
        {
            var comment = new Comment
            {
                Name = name,
                Text = text,
                AuthorId = authorId
            };
            var change = new CommentsChange
            {
                PhotoId = photoId,
                Comment = comment,
                Status = ChangeStatus.Add
            };
            using (var context = new PhotoDbContext())
            {
                context.Comments.Add(comment);
                context.CommentsHistory.Add(change);
                await context.SaveChangesAsync();
                return new AddCommentModel
                {
                    CommentId = comment.Id,
                    NewState = change.Id
                };
            }
        }

        public async Task<Guid> RemoveComment(int photoId, Guid authorId, Guid commentId)
        {
            using (var context = new PhotoDbContext())
            {
                var comment = await context.Comments.FindAsync(commentId);
                if (comment.AuthorId != authorId)
                    throw new MethodAccessException();
                var change = new CommentsChange
                {
                    PhotoId = photoId,
                    Comment = comment,
                    Status = ChangeStatus.Remove
                };
                context.Comments.Add(comment);
                context.CommentsHistory.Add(change);
                await context.SaveChangesAsync();
                return change.Id;
            }
        }
    }
}