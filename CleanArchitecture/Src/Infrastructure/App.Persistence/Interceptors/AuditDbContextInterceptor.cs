using App.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace App.Persistence.Interceptors {
    public class AuditDbContextInterceptor : SaveChangesInterceptor {
        private static readonly Dictionary<EntityState, Action<DbContext, IAuditEntity>> _stateBehaviors =
            new() {
                { EntityState.Added, AddBehavior },
                { EntityState.Modified, ModifiedBehavior }
            };

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default) {
            var context = eventData.Context;

            if (context != null) {
                foreach (var contextEvent in context.ChangeTracker.Entries().ToList()) {
                    if (contextEvent.Entity is not IAuditEntity auditEntity) continue;

                    if (_stateBehaviors.TryGetValue(contextEvent.State, out var behavior)) {
                        behavior(context, auditEntity);
                    }
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void AddBehavior(DbContext dbContext, IAuditEntity auditEntity) {
            auditEntity.CreatedAt = DateTime.UtcNow;
            dbContext.Entry(auditEntity).Property(x => x.UpdatedAt).IsModified = false;
        }

        private static void ModifiedBehavior(DbContext dbContext, IAuditEntity auditEntity) {
            auditEntity.UpdatedAt = DateTime.UtcNow;
            dbContext.Entry(auditEntity).Property(x => x.CreatedAt).IsModified = false;
        }
    }
}