using Hkust.Common.WebApi.Registrar;
using Hkust.Usr.WebApi.Authentication;
using Hkust.Usr.WebApi.Authorization;

namespace Hkust.Usr.WebApi.Registrar;

public sealed class UsrWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public UsrWebApiDependencyRegistrar(IServiceCollection services)
        : base(services)
    {//动态创建实例
    }

    public UsrWebApiDependencyRegistrar(IApplicationBuilder app)
        : base(app)
    {
    }

    public override void AddHkust()//统一注册服务
    {
        AddWebApiDefault<BearerAuthenticationLocalProcessor, PermissionLocalHandler>();//Authentication 认证  ==>BearerAuthenticationLocalProcessor ---> Authorization 授权==>PermissionLocalHandler
        AddHealthChecks(true, true, true, false);//健康检查服务,可选项 add by garfield 20230308
        Services.AddGrpc();//GRPC
    }

    public override void UseHkust()
    {
        UseWebApiDefault(endpointRoute: endpoint =>
        {
            endpoint.MapGrpcService<Grpc.AuthGrpcServer>();//暴露GRPC服务 mark by garfield 20230308
            endpoint.MapGrpcService<Grpc.UsrGrpcServer>();
        });
    }
}