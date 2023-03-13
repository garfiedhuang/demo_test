using Hkust.Common.Application.Registrar;

namespace Hkust.Platform.Application.Registrar;

public sealed class SystemApplicationDependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => typeof(IUserAppService).Assembly;

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public SystemApplicationDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddHkust() => AddApplicaitonDefault();
}