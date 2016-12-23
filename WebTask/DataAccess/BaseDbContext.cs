using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace WebTask.DataAccess
{
    public abstract class BaseDbContext: DbContext
    {
        protected BaseDbContext() : base("RinaDb")
        {
        }

        public override int SaveChanges()
        {
            FillBaseModels();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            FillBaseModels();
            return base.SaveChangesAsync();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            FillBaseModels();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected abstract void FillBaseModels();

        protected void FillBaseModel<T>() where T : BaseModel
        {
            ChangeTracker.DetectChanges();
            foreach (var entityEntry in ChangeTracker.Entries<T>())
            {
                switch(entityEntry.State)
                {
                    case EntityState.Added:
                        entityEntry.Entity.Id = Guid.NewGuid();
                        goto case EntityState.Modified;
                    case EntityState.Modified:
                        entityEntry.Entity.Modify = DateTime.Now;
                        break;
                    default: continue;
                }
            }
        }
    }
}