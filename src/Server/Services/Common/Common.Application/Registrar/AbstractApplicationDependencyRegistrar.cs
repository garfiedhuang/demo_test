using Hkust.Common.Application.Services.Trackers;
using Hkust.Common.Consts.AppSettings;

namespace Hkust.Common.Application.Registrar;

public abstract partial class AbstractApplicationDependencyRegistrar : IDependencyRegistrar
{
    public string Name => "application";
    public abstract Assembly ApplicationLayerAssembly { get; }//Application层
    public abstract Assembly ContractsLayerAssembly { get; }//契约层
    public abstract Assembly RepositoryOrDomainLayerAssembly { get; }//仓储和领域层
    protected SkyApmExtensions SkyApm { get; init; }
    protected List<Rpc.AddressNode> RpcAddressInfo { get; init; }
    protected IServiceCollection Services { get; init; }
    protected IConfiguration Configuration { get; init; }
    protected IServiceInfo ServiceInfo { get; init; }
    protected IConfigurationSection RedisSection { get; init; }
    protected IConfigurationSection CachingSection { get; init; }
    protected IConfigurationSection MysqlSection { get; init; }
    protected IConfigurationSection MongoDbSection { get; init; }
    protected IConfigurationSection ConsulSection { get; init; }
    protected IConfigurationSection RabbitMqSection { get; init; }

    protected AbstractApplicationDependencyRegistrar(IServiceCollection services)//读取第三方中间件配置 mark by garfield 20221019
    {
        Services = services ?? throw new ArgumentException("IServiceCollection is null.");
        Configuration = services.GetConfiguration() ?? throw new ArgumentException("Configuration is null.");
        ServiceInfo = services.GetServiceInfo() ?? throw new ArgumentException("ServiceInfo is null.");
        RedisSection = Configuration.GetSection(NodeConsts.Redis);
        CachingSection = Configuration.GetSection(NodeConsts.Caching);
        MongoDbSection = Configuration.GetSection(NodeConsts.MongoDb);
        MysqlSection = Configuration.GetSection(NodeConsts.Mysql);
        ConsulSection = Configuration.GetSection(NodeConsts.Consul);
        RabbitMqSection = Configuration.GetSection(NodeConsts.RabbitMq);
        SkyApm = Services.AddSkyApmExtensions();
        RpcAddressInfo = Configuration.GetSection(NodeConsts.RpcAddressInfo).Get<List<Rpc.AddressNode>>();
    }

    /// <summary>
    /// 注册所有服务
    /// </summary>
    public abstract void AddHkust();

    /// <summary>
    /// 注册hkust.application通用服务
    /// </summary>
    protected virtual void AddApplicaitonDefault()
    {
        Services
            .AddValidatorsFromAssembly(ContractsLayerAssembly, ServiceLifetime.Scoped)//添加验证模型至IOC by garfield 20221019
            .AddHkustInfraAutoMapper(ApplicationLayerAssembly)//添加dto映射关系至IOC,所有application层中继承Profile密封类  ===》 by garfield 20221019
            .AddHkustInfraYitterIdGenerater(RedisSection)//添加雪花ID生成服务 by garfield 20221019
            .AddHkustInfraConsul(ConsulSection)//添加Consual服务 by garfield 20221019
            .AddHkustInfraDapper();//添加dapper服务  by garfield 20221019

        AddApplicationSharedServices();//添加application层共享服务 by garfield 20221019
        AddAppliactionSerivcesWithInterceptors();
        AddApplicaitonHostedServices();
        AddEfCoreContextWithRepositories();
        AddMongoContextWithRepositries();
        AddRedisCaching();
        AddBloomFilters();
    }

    /// <summary>
    /// 注册application.shared层服务
    /// </summary>
    protected void AddApplicationSharedServices()
    {
        Services.AddSingleton(typeof(Lazy<>));//懒加载，单例
        Services.AddScoped<UserContext>();//用户会话数据,认证之后赋值（Hybrid,Basic,Beare）
        Services.AddScoped<OperateLogInterceptor>();//操作日志拦截器，AOP思想，通过Castle.DynamicProxy实现 by garfield 20221019 ==》 异步转同步
        Services.AddScoped<OperateLogAsyncInterceptor>();//异步
        Services.AddScoped<UowInterceptor>();//工作单元拦截器，AOP思想，通过Castle.DynamicProxy实现 by garfield 20221019 ==》 异步转同步  ==》 接口拦截，增加DB事务，同时可将事务共享给CAP关联服务
        Services.AddScoped<UowAsyncInterceptor>();//异步
        Services.AddSingleton<IBloomFilter, NullBloomFilter>();//空布隆过滤器
        Services.AddSingleton<BloomFilterFactory>();//布隆过滤器工厂
        Services.AddHostedService<CachingHostedService>();//缓存后台服务，定时删除过期后定时删除  by garfield 20221019
        Services.AddHostedService<BloomFilterHostedService>();//加载布隆过滤器后台服务，仅加载一次 by garfield 20221019
        Services.AddHostedService<Channels.ChannelConsumersHostedService>();//高性能后台通道服务，用于定时将登录日志&操作日志写入mongodb by garfield 20221019


        Services.AddScoped<IMessageTracker, DbMessageTrackerService>();//事件跟踪,DB类型
        Services.AddScoped<IMessageTracker, RedisMessageTrackerService>();//未实现逻辑
        Services.AddScoped<MessageTrackerFactory>();
    }
}