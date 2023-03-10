using Hkust.Common.Application.Registrar;

namespace Hkust.Maint.Application.Registrar;

public sealed class MaintApplicationDependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => typeof(IDictAppService).Assembly;

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public MaintApplicationDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddHkust()
    {
        AddApplicaitonDefault();
        //rpc-rest
        var restPolicies = this.GenerateDefaultRefitPolicies();
        AddRestClient<IAuthRestClient>(RpcConsts.UsrService, restPolicies);
        AddRestClient<IUsrRestClient>(RpcConsts.UsrService, restPolicies);

        //AddRabbitMqClient();//取消执行RabbitMq的消费者 mark by garfield 20230309
    }
}