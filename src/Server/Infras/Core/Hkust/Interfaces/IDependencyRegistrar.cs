namespace Hkust.Infras.Core.Interfaces;

public interface IDependencyRegistrar
{
    public string Name { get; }

    public void AddHkust();
}