using System.Linq.Expressions;
using dotnetlearner.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetlearner.Data;


public class AppDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var user = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            var now = DateTime.UtcNow;

            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = user;
                    break;

                case EntityState.Modified:
                    entry.Entity.ModifiedAt = now;
                    entry.Entity.ModifiedBy = user;
                    break;

                case EntityState.Deleted:
                    // Soft delete
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = now;
                    entry.Entity.ModifiedBy = user;
                    entry.Entity.ModifiedAt = now;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(ConvertFilterExpression(entityType.ClrType));
            }
        }
    }

    private static LambdaExpression ConvertFilterExpression(Type entityType)
    {
        var param = Expression.Parameter(entityType, "e");
        var prop = Expression.Property(param, "IsDeleted");
        var condition = Expression.Equal(prop, Expression.Constant(false));
        var lambda = Expression.Lambda(condition, param);
        return lambda;
    }

    public DbSet<User> Users { get; set; }


}
