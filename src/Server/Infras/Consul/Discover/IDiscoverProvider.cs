namespace Hkust.Infras.Consul.Discover;

public interface IDiscoverProvider
{
    Task<IList<string>> GetAllHealthServicesAsync();
    Task<string> GetSingleHealthServiceAsync();
}
