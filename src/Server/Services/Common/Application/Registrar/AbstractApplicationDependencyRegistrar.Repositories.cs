using Hkust.Infras.Repository.EfCore.MySql.Configurations;
using Hkust.Infras.Repository.Mongo;
using Hkust.Infras.Repository.Mongo.Configuration;
using Hkust.Infras.Repository.Mongo.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Hkust.Common.Application.Registrar;

public abstract partial class AbstractApplicationDependencyRegistrar
{
    /// <summary>
    /// 注册EFCoreContext与仓储
    /// </summary>
    protected virtual void AddEfCoreContextWithRepositories()
    {
        var serviceType = typeof(IEntityInfo);
        var implType = RepositoryOrDomainLayerAssembly.ExportedTypes.FirstOrDefault(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true));
        if (implType is null)
            throw new NotImplementedException(nameof(IEntityInfo));
        else
            Services.AddScoped(serviceType, implType);//所有实体类,即：Entities目录下的 mark by garfield 20230308

        AddEfCoreContext();
    }

    /// <summary>
    /// 注册EFCoreContext
    /// </summary>
    protected virtual void AddEfCoreContext()
    {
        var mysqlConfig = MysqlSection.Get<MysqlOptions>();
        var serverVersion = new MariaDbServerVersion(new Version(10, 5, 4));
        Services.AddHkustInfraEfCoreMySql(options =>
        {
            options.UseLowerCaseNamingConvention();
            options.UseMySql(mysqlConfig.ConnectionString, serverVersion, optionsBuilder =>
            {
                optionsBuilder.MinBatchSize(4)
                              .MigrationsAssembly(ServiceInfo.StartAssembly.GetName().Name.Replace("WebApi", "Migrations"))
                              .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            if (this.IsDevelopment())
            {
                //options.AddInterceptors(new DefaultDbCommandInterceptor())
                options.LogTo(Console.WriteLine, LogLevel.Information)
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors();
            }
            //替换默认查询sql生成器,如果通过mycat中间件实现读写分离需要替换默认SQL工厂。逻辑未实现 mark by garfield 20230309
            //options.ReplaceService<IQuerySqlGeneratorFactory, HkustMySqlQuerySqlGeneratorFactory>();
        });

        //TODO: 考虑使用ORACLE数据库 mark by garfield 20230308
    }

    /// <summary>
    /// 注册MongoContext与仓储
    /// </summary>
    protected virtual void AddMongoContextWithRepositries(Action<IServiceCollection> action = null)
    {
        action?.Invoke(Services);

        var mongoConfig = MongoDbSection.Get<MongoOptions>();
        Services.AddHkustInfraMongo<MongoContext>(options =>
        {
            options.ConnectionString = mongoConfig.ConnectionString;
            options.PluralizeCollectionNames = mongoConfig.PluralizeCollectionNames;
            options.CollectionNamingConvention = (NamingConvention)mongoConfig.CollectionNamingConvention;
        });
    }
}