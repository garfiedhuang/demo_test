namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    /// <summary>
    ///  统一注册Hkust.WebApi通用服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="startupAssembly"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public static IServiceCollection AddHkust(this IServiceCollection services, IServiceInfo serviceInfo)
    {
        if (serviceInfo?.StartAssembly is null)
            throw new ArgumentNullException(nameof(serviceInfo));

        var webApiRegistarType = serviceInfo.StartAssembly.ExportedTypes.FirstOrDefault(m => m.IsAssignableTo(typeof(IDependencyRegistrar)) && m.IsNotAbstractClass(true));//取当前启用工程依赖注入类UsrWebApiDependencyRegistrar
        if (webApiRegistarType is null)
            throw new NullReferenceException(nameof(IDependencyRegistrar));

        var webapiRegistar = Activator.CreateInstance(webApiRegistarType, services)  as IDependencyRegistrar;//动态创建当前工程下依赖注入实例， services构造函数参数
        webapiRegistar.AddHkust();//统一注册服务

        return services;
    }
}