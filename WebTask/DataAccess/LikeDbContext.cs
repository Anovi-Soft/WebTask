using System.Data.Entity;

namespace WebTask.DataAccess
{
    public class LikeDbContext: BaseDbContext
    {
        public LikeDbContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<LikeDbContext>());
        }
        public DbSet<LikeModel> LikeSet { get; set; }
        protected override void FillBaseModels()
        {
            FillBaseModel<LikeModel>();
        }
    }
}