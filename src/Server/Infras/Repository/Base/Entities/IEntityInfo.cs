using Hkust.Infras.IRepositories;

namespace Hkust.Infras.Entities;

public interface IEntityInfo
{
    Operater GetOperater();

    void OnModelCreating(dynamic modelBuilder);
}