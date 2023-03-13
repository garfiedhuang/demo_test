namespace Hkust.Platform.Application.BloomFilters;

public class CacheKeyBloomFilter : AbstractBloomFilter
{
    private readonly Lazy<IServiceProvider> _serviceProvider;
    private readonly Lazy<IOptions<CacheOptions>> _cacheOptions;

    public CacheKeyBloomFilter(
        Lazy<IOptions<CacheOptions>> cacheOptions,
        Lazy<IRedisProvider> redisProvider,
        Lazy<IDistributedLocker> distributedLocker,
        Lazy<IServiceProvider> serviceProvider)
        : base(redisProvider, distributedLocker)
    {
        _serviceProvider = serviceProvider;
        _cacheOptions = cacheOptions;
    }

    public override string Name => _cacheOptions.Value.Value.PenetrationSetting.BloomFilterSetting.Name;

    public override double ErrorRate => _cacheOptions.Value.Value.PenetrationSetting.BloomFilterSetting.ErrorRate;

    public override int Capacity => _cacheOptions.Value.Value.PenetrationSetting.BloomFilterSetting.Capacity;

    public override async Task InitAsync()
    {
        var exists = await ExistsBloomFilterAsync();
        if (!exists)
        {
            var values = new List<string>()
            {
                UsrCachingConsts.MenuListCacheKey,
                UsrCachingConsts.MenuTreeListCacheKey,
                UsrCachingConsts.MenuRelationCacheKey,
                UsrCachingConsts.MenuCodesCacheKey,
                UsrCachingConsts.DetpListCacheKey,
                UsrCachingConsts.DetpTreeListCacheKey,
                UsrCachingConsts.DetpSimpleTreeListCacheKey,
                UsrCachingConsts.RoleListCacheKey
            };

            using var scope = _serviceProvider.Value.CreateScope();

            #region USR模块

            var repository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysUser>>();
            var ids = await repository
                                                    .GetAll()
                                                    .Select(x => x.Id)
                                                    .ToListAsync();
            if (ids.IsNotNullOrEmpty())
                values.AddRange(ids.Select(x => string.Concat(UsrCachingConsts.UserValidatedInfoKeyPrefix, UsrCachingConsts.LinkChar, x)));
            #endregion


            #region MAINT模块

            var dictRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysDict>>();
            var dictIds = await dictRepository
                .Where(x => x.Pid == 0)
                .Select(x => x.Id)
                .ToListAsync();
            if (dictIds.IsNotNullOrEmpty())
                values.AddRange(dictIds.Select(x => string.Concat(MaintCachingConsts.DictSingleKeyPrefix, MaintCachingConsts.LinkChar, x)));

            var cfgRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysDict>>();
            var cfgIds = await dictRepository
                .GetAll()
                .Select(x => x.Id)
                .ToListAsync();
            if (cfgIds.IsNotNullOrEmpty())
                values.AddRange(cfgIds.Select(x => string.Concat(MaintCachingConsts.CfgSingleKeyPrefix, MaintCachingConsts.LinkChar, x)));
            #endregion

            await InitAsync(values);
        }
    }
}