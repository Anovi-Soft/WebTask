using System.Data.Entity;

namespace WebTask.DataAccess
{
    public class UserSessionDbContext: BaseDbContext
    {
        public UserSessionDbContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<UserSessionDbContext>());
        }
        public DbSet<UserSession> UserSessions { get; set; }

        protected override void FillBaseModels()
        {
            FillBaseModel<UserSession>();
        }
    }
}