namespace Hkust.Infras.Repository.EfCore.SqlServer;

public class SqlServerDbContext : HkustDbContext
{
    public SqlServerDbContext(
        DbContextOptions options,
        IEntityInfo entityInfo)
        : base(options, entityInfo)
    {
    }
}