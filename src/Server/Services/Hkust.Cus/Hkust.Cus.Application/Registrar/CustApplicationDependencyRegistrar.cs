using Hkust.Common.Application.Registrar;

namespace Hkust.Cus.Application.Registrar;

public sealed class CustApplicationDependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => typeof(ICustomerAppService).Assembly;

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public CustApplicationDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddHkust()
    {
        AddApplicaitonDefault();

        //rpc-restclient
        var restPolicies = this.GenerateDefaultRefitPolicies();
        AddRestClient<IAuthRestClient>(RpcConsts.UsrService, restPolicies);
        AddRestClient<IUsrRestClient>(RpcConsts.UsrService, restPolicies);
        AddRestClient<IMaintRestClient>(RpcConsts.MaintService, restPolicies);
        //AddRestClient<IWhseRestClient>(RpcConsts.WhseService, restPolicies);//mark by garfield 20230309
        //rpc-grpcclient
        var gprcPolicies = this.GenerateDefaultGrpcPolicies();
        AddGrpcClient<AuthGrpc.AuthGrpcClient>(RpcConsts.UsrService, gprcPolicies);
        AddGrpcClient<UsrGrpc.UsrGrpcClient>(RpcConsts.UsrService, gprcPolicies);
        AddGrpcClient<MaintGrpc.MaintGrpcClient>(RpcConsts.MaintService, gprcPolicies);
        //AddGrpcClient<WhseGrpc.WhseGrpcClient>(RpcConsts.WhseService, gprcPolicies);//mark by garfield 20230309

        //rpc-even
        //AddCapEventBus<CapEventSubscriber>();//依赖于RabbitMq服务 mark by garfield 20230309
    }
}