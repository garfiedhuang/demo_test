using Hkust.Infra.IRepositories;

namespace Hkust.Infra.Entities;

public interface IEntityInfo
{
    Operater GetOperater();

    void OnModelCreating(dynamic modelBuilder);
}