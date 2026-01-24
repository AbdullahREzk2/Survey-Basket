namespace SurveyBasket.DAL.Persistence;
public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options, ICurrentUserService currentUserService) :
    IdentityDbContext<ApplicationUser>(options)
{
    private readonly ICurrentUserService? _currentUserService = currentUserService;


    public DbSet<Poll> Polls { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var cascadeFKs = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<AuditableEntity>();

        foreach (var entityEntry in entries)
        {
            var currentUserId = _currentUserService!.UserId;

            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(x => x.createdById).CurrentValue = currentUserId!;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(x => x.updatedById).CurrentValue = currentUserId;
                entityEntry.Property(x => x.updatedOn).CurrentValue = DateTime.UtcNow;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }


}
