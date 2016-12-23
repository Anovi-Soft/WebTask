using System.Data.Entity;

namespace WebTask.DataAccess
{
    public class PhotoDbContext: BaseDbContext
    {
        public PhotoDbContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<PhotoDbContext>());
        }

        public DbSet<CommentsChange> CommentsHistory { get; set; }
        public DbSet<Comment> Comments { get; set; }
        protected override void FillBaseModels()
        {
            FillBaseModel<CommentsChange>();
            FillBaseModel<Comment>();
        }
    }
}