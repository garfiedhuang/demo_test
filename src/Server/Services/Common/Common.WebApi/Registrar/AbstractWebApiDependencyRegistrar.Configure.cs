using Hkust.Common.Consts.AppSettings;

namespace Hkust.Common.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// 注册配置类到IOC容器
    /// </summary>
    protected virtual void Configure()
    {
        Services
            .Configure<JWTOptions>(Configuration.GetSection(NodeConsts.JWT))//JWT配置 mark by garfield 20221019
            .Configure<ThreadPoolSettings>(Configuration.GetSection(NodeConsts.ThreadPoolSettings))//线程池配置 mark by garfield 20221019
            .Configure<KestrelOptions>(Configuration.GetSection(NodeConsts.Kestrel)) //[ASP.NET Core 项目模板中的 Web 服务器，默认处于启用状态]Kestrel服务器配置 mark by garfield 20221019
            ;
    }
}
