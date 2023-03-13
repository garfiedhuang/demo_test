using Hkust.Common.WebApi.Authentication;
using Hkust.Common.WebApi.Authorization;

namespace Hkust.Common.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar : IDependencyRegistrar
{
    public string Name => "webapi";
    protected IConfiguration Configuration { get; init; }
    protected IServiceCollection Services { get; init; }
    protected IServiceInfo ServiceInfo { get; init; }

    /// <summary>
    /// 服务注册与系统配置
    /// </summary>
    /// <param name="services"><see cref="IServiceInfo"/></param>
    protected AbstractWebApiDependencyRegistrar(IServiceCollection services)
    {
        Services = services;
        Configuration = services.GetConfiguration();
        ServiceInfo = services.GetServiceInfo();
    }

    /// <summary>
    /// 注册服务入口方法
    /// </summary>
    public abstract void AddHkust();

    /// <summary>
    /// 注册Webapi通用的服务
    /// </summary>
    /// <typeparam name="THandler"></typeparam>
    protected virtual void AddWebApiDefault() =>
        AddWebApiDefault<BearerAuthenticationRemoteProcessor, PermissionRemoteHandler>();

    /// <summary>
    /// 注册Webapi通用的服务
    /// </summary>
    /// <typeparam name="TAuthenticationProcessor"><see cref="AbstractAuthenticationProcessor"/></typeparam>
    /// <typeparam name="TAuthorizationHandler"><see cref="AbstractPermissionHandler"/></typeparam>
    protected virtual void AddWebApiDefault<TAuthenticationProcessor, TAuthorizationHandler>()
        where TAuthenticationProcessor : AbstractAuthenticationProcessor
        where TAuthorizationHandler : AbstractPermissionHandler
    {
        Services
            .AddHttpContextAccessor()//获取上下文实例HttpContext
            .AddMemoryCache();//启用内存缓存（IMemoryCache）服务
        Configure();//注册配置类到IOC容器：JWT配置，线程池配置，Kestrel配置 注入IOC mark by garfield 20221019
        AddControllers();//控制器注入IOC mark by garfield 20221019
        AddAuthentication<TAuthenticationProcessor>();//添加权限认证服务至IOC mark by garfield 20221019
        AddAuthorization<TAuthorizationHandler>();//添加授权服务
        AddCors();//添加跨域服务
        AddSwaggerGen();//添加Swagger服务,特殊点：需要将application层的xml文件+application.contracts的xml文件加载到swagger里面，供生成swagger文档 mark by garfield 20221019
        AddMiniProfiler();//添加MiniProfiler性能监控组件服务 mark by garfield 20221019
        AddApplicationServices();//添加应用层服务至IOC mark by garfield 20221019
    }
}