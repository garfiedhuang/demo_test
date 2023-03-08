namespace Hkust.Common.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// 注册Application层服务
    /// </summary>
    protected virtual void AddApplicationServices()
    {
        var appAssembly = ServiceInfo.GetApplicationAssembly();
        if (appAssembly is not null)
        {
            var applicationRegistrarType = appAssembly.ExportedTypes.FirstOrDefault(m => m.IsAssignableTo(typeof(IDependencyRegistrar)) && m.IsNotAbstractClass(true));//获取application服务注入入口UsrApplicationDependencyRegistrar mark by garfield 20221019
            if (applicationRegistrarType is not null)
            {
                var applicationRegistrar = Activator.CreateInstance(applicationRegistrarType, Services) as IDependencyRegistrar;
                applicationRegistrar?.AddHkust();
            }
        }
    }
}
