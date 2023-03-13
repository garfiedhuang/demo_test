using Hkust.Common.WebApi.Registrar;
using Hkust.Platform.WebApi.Authentication;
using Hkust.Platform.WebApi.Authorization;

namespace Hkust.Platform.WebApi.Registrar;

public sealed class SystemWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public SystemWebApiDependencyRegistrar(IServiceCollection services)
        : base(services)
    {//动态创建实例
    }

    public SystemWebApiDependencyRegistrar(IApplicationBuilder app)
        : base(app)
    {
    }

    public override void AddHkust()//统一注册服务
    {
        AddWebApiDefault<BearerAuthenticationLocalProcessor, PermissionLocalHandler>();//Authentication 认证  ==>BearerAuthenticationLocalProcessor ---> Authorization 授权==>PermissionLocalHandler
        AddHealthChecks(true, true, true, false);//健康检查服务,可选项 add by garfield 20230308
        Services.AddGrpc();//注册GRPC服务至容器中
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