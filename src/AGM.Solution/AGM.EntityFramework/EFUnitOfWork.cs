using AGM.Database.Context;
using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AGM.EntityFramework
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly AGMDBContext _dbContext;

        public EFUnitOfWork(AGMDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task SaveChanchesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditableEntities();
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
        private void UpdateAuditableEntities()
        {
            IEnumerable<EntityEntry<IAuditableEntity>> entries =
                _dbContext
                    .ChangeTracker
                    .Entries<IAuditableEntity>();

            foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(a => a.CreatedOn)
                        .CurrentValue = DateTime.UtcNow;
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(a => a.ModifiedOn)
                        .CurrentValue = DateTime.UtcNow;
                }
            }
        }
    }
}
