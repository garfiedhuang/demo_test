using Hkust.Infras.Core.Interfaces;

namespace Hkust.Platform.Application.Caching;

public sealed class CacheService : AbstractCacheService, ICachePreheatable
{
    private Lazy<IDistributedLocker> _dictributeLocker;
    private ILogger<CacheService> _logger;
    private readonly Lazy<IOptions<JWTOptions>> _jwtConfig;

    public CacheService(
        Lazy<ICacheProvider> cacheProvider,
        Lazy<IServiceProvider> serviceProvider,
        Lazy<IDistributedLocker> dictributeLocker,
        Lazy<IOptions<JWTOptions>> jwtConfig,
        ILogger<CacheService> logger)
        : base(cacheProvider, serviceProvider)
    {
        _dictributeLocker = dictributeLocker;
        _logger = logger;
        _jwtConfig = jwtConfig;
    }

    public override async Task PreheatAsync()
    {
        #region USR模块

        await GetAllDeptsFromCacheAsync();
        await GetAllRelationsFromCacheAsync();
        await GetAllMenusFromCacheAsync();
        await GetAllRolesFromCacheAsync();
        await GetAllMenuCodesFromCacheAsync();
        await GetDeptSimpleTreeListAsync();

        #endregion

        #region MAINT模块

        await PreheatAllDictsAsync();
        await PreheatAllCfgsAsync();

        #endregion
    }

    #region USR模块

    internal int GetRefreshTokenExpires() =>
        _jwtConfig.Value.Value.RefreshTokenExpire * 60 + _jwtConfig.Value.Value.ClockSkew;

    internal async Task SetValidateInfoToCacheAsync(UserValidatedInfoDto value)
    {
        var cacheKey = ConcatCacheKey(UsrCachingConsts.UserValidatedInfoKeyPrefix, value.Id);
        await CacheProvider.Value.SetAsync(cacheKey, value, TimeSpan.FromSeconds(GetRefreshTokenExpires()));
    }

    internal async Task<UserValidatedInfoDto> GetUserValidateInfoFromCacheAsync(long id)
    {
        var cacheKey = ConcatCacheKey(UsrCachingConsts.UserValidatedInfoKeyPrefix, id.ToString());
        //var cacheValue = await CacheProvider.Value.GetAsync(cacheKey, async () =>
        //{
        //    using var scope = ServiceProvider.Value.CreateScope();
        //    var userRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysUser>>();
        //    return await userRepository.FetchAsync(x => new UserValidatedInfoDto(x.Id, x.Account, x.Name, x.RoleIds, x.Status, x.Password), x => x.Id == Id && x.Status == 1);
        //}, GetRefreshTokenExpires());
        var cacheValue = await CacheProvider.Value.GetAsync<UserValidatedInfoDto>(cacheKey);
        return cacheValue.Value;
    }

    internal async Task ChangeUserValidateInfoCacheExpiresDtAsync(long id)
    {
        var cacheKey = ConcatCacheKey(UsrCachingConsts.UserValidatedInfoKeyPrefix, id);
        await CacheProvider.Value.KeyExpireAsync(new string[] { cacheKey }, GetRefreshTokenExpires());
    }

    internal async Task<List<DeptDto>> GetAllDeptsFromCacheAsync()
    {
        var cahceValue = await CacheProvider.Value.GetAsync(UsrCachingConsts.DetpListCacheKey, async () =>
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var deptRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysDept>>();
            var allDepts = await deptRepository.GetAll(writeDb: true).OrderBy(x => x.Ordinal).ToListAsync();
            return Mapper.Value.Map<List<DeptDto>>(allDepts);
        }, TimeSpan.FromSeconds(UsrCachingConsts.OneYear));

        return cahceValue.Value;
    }

    internal async Task<List<RelationDto>> GetAllRelationsFromCacheAsync()
    {
        var cahceValue = await CacheProvider.Value.GetAsync(UsrCachingConsts.MenuRelationCacheKey, async () =>
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var relationRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysRelation>>();
            var allRelations = await relationRepository.GetAll(writeDb: true).ToListAsync();
            return Mapper.Value.Map<List<RelationDto>>(allRelations);
        }, TimeSpan.FromSeconds(UsrCachingConsts.OneYear));

        return cahceValue.Value;
    }

    internal async Task<List<MenuDto>> GetAllMenusFromCacheAsync()
    {
        var cahceValue = await CacheProvider.Value.GetAsync(UsrCachingConsts.MenuListCacheKey, async () =>
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var menuRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysMenu>>();
            var allMenus = await menuRepository.GetAll(writeDb: true).OrderBy(x => x.Ordinal).ToListAsync();
            return Mapper.Value.Map<List<MenuDto>>(allMenus);
        }, TimeSpan.FromSeconds(UsrCachingConsts.OneYear));

        return cahceValue.Value;
    }

    internal async Task<List<RoleDto>> GetAllRolesFromCacheAsync()
    {
        var cahceValue = await CacheProvider.Value.GetAsync(UsrCachingConsts.RoleListCacheKey, async () =>
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var roleRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysRole>>();
            var allRoles = await roleRepository.GetAll(writeDb: true).OrderBy(x => x.Ordinal).ToListAsync();
            return Mapper.Value.Map<List<RoleDto>>(allRoles);
        }, TimeSpan.FromSeconds(UsrCachingConsts.OneYear));

        return cahceValue.Value;
    }

    internal async Task<List<RoleMenuCodesDto>> GetAllMenuCodesFromCacheAsync()
    {
        var cahceValue = await CacheProvider.Value.GetAsync(UsrCachingConsts.MenuCodesCacheKey, async () =>
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var relationRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysRelation>>();
            var allMenus = await relationRepository.GetAll(writeDb: true)
                                                                            .Where(x => x.Menu.Status)
                                                                            .Select(x => new RoleMenuCodesDto { RoleId = x.RoleId, Code = x.Menu.Code })
                                                                            .ToListAsync();
            return allMenus.Distinct().ToList();
        }, TimeSpan.FromSeconds(UsrCachingConsts.OneYear));

        return cahceValue.Value;
    }

    internal async Task<List<DeptSimpleTreeDto>> GetDeptSimpleTreeListAsync()
    {
        var result = new List<DeptSimpleTreeDto>();

        var cacheValue = await CacheProvider.Value.GetAsync<List<DeptSimpleTreeDto>>(UsrCachingConsts.DetpSimpleTreeListCacheKey);
        if (cacheValue.HasValue)
            return cacheValue.Value;

        var depts = await GetAllDeptsFromCacheAsync();

        if (depts.IsNullOrEmpty())
            return result;

        var roots = depts.Where(d => d.Pid == 0)
                                    .OrderBy(d => d.Ordinal)
                                    .Select(x => new DeptSimpleTreeDto { Id = x.Id, Label = x.SimpleName })
                                    .ToList();
        foreach (var node in roots)
        {
            GetChildren(node, depts);
            result.Add(node);
        }

        void GetChildren(DeptSimpleTreeDto currentNode, List<DeptDto> depts)
        {
            var childrenNodes = depts.Where(d => d.Pid == currentNode.Id)
                                                       .OrderBy(d => d.Ordinal)
                                                       .Select(x => new DeptSimpleTreeDto() { Id = x.Id, Label = x.SimpleName })
                                                       .ToList();
            if (childrenNodes.IsNotNullOrEmpty())
            {
                currentNode.Children.AddRange(childrenNodes);
                foreach (var node in childrenNodes)
                {
                    GetChildren(node, depts);
                }
            }
        }

        await CacheProvider.Value.SetAsync(UsrCachingConsts.DetpSimpleTreeListCacheKey, result, TimeSpan.FromSeconds(UsrCachingConsts.OneYear));

        return result;
    }

    #endregion

    #region MAINT模块

    private async Task PreheatAllDictsAsync()
    {
        var exists = await CacheProvider.Value.ExistsAsync(MaintCachingConsts.DictPreheatedKey);
        if (exists)
            return;

        var flag = await _dictributeLocker.Value.LockAsync(MaintCachingConsts.DictPreheatedKey);
        if (!flag.Success)
        {
            await Task.Delay(500);
            await PreheatAllDictsAsync();
        }

        using var scope = ServiceProvider.Value.CreateScope();
        var dictRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysDict>>();
        var dictEntities = await dictRepository.GetAll().ToListAsync();
        if (dictEntities.IsNullOrEmpty())
            return;

        var parentEntities = dictEntities.Where(x => x.Pid == 0).ToList();
        var childrenEntities = dictEntities.Where(x => x.Pid > 0).ToList();
        var parentDtos = Mapper.Value.Map<List<DictDto>>(parentEntities);
        var childrenDtos = Mapper.Value.Map<List<DictDto>>(childrenEntities);

        _logger.LogInformation($"start preheat {MaintCachingConsts.DictSingleKeyPrefix}");
        var cahceDictionary = new Dictionary<string, DictDto>();
        for (var index = 1; index <= parentDtos.Count; index++)
        {
            var dto = parentDtos[index - 1];
            var subDtos = childrenDtos?.Where(x => x.Pid == dto.Id).ToList();
            if (subDtos.IsNotNullOrEmpty())
            {
                dto.Children = subDtos;
            }

            var cacheKey = ConcatCacheKey(MaintCachingConsts.DictSingleKeyPrefix, dto.Id);
            cahceDictionary.Add(cacheKey, dto);
            if (index % 50 == 0 || index == parentDtos.Count)
            {
                await CacheProvider.Value.SetAllAsync(cahceDictionary, TimeSpan.FromSeconds(MaintCachingConsts.OneMonth));
                cahceDictionary.Clear();
            }
        }

        var serverInfo = ServiceProvider.Value.GetService<IServiceInfo>();
        await CacheProvider.Value.SetAsync(MaintCachingConsts.DictPreheatedKey, serverInfo.Version, TimeSpan.FromSeconds(MaintCachingConsts.OneYear));
        _logger.LogInformation($"finished({parentDtos.Count}) preheat {MaintCachingConsts.DictSingleKeyPrefix}");
    }

    private async Task PreheatAllCfgsAsync()
    {
        var exists = await CacheProvider.Value.ExistsAsync(MaintCachingConsts.CfgPreheatedKey);
        if (exists)
            return;

        var flag = await _dictributeLocker.Value.LockAsync(MaintCachingConsts.CfgPreheatedKey);
        if (!flag.Success)
        {
            await Task.Delay(500);
            await PreheatAllCfgsAsync();
        }

        using var scope = ServiceProvider.Value.CreateScope();
        var cfgRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysCfg>>();
        var cfgEntities = await cfgRepository.GetAll().ToListAsync();
        if (cfgEntities.IsNullOrEmpty())
            return;

        var cfgDtos = Mapper.Value.Map<List<CfgDto>>(cfgEntities);
        _logger.LogInformation($"start preheat {MaintCachingConsts.CfgSingleKeyPrefix}");
        var cahceDictionary = new Dictionary<string, CfgDto>();
        for (var index = 1; index <= cfgDtos.Count; index++)
        {
            var dto = cfgDtos[index - 1];
            var cacheKey = ConcatCacheKey(MaintCachingConsts.CfgSingleKeyPrefix, dto.Id);
            cahceDictionary.Add(cacheKey, dto);
            if (index % 50 == 0 || index == cfgDtos.Count)
            {
                await CacheProvider.Value.SetAllAsync(cahceDictionary, TimeSpan.FromSeconds(MaintCachingConsts.OneMonth));
                cahceDictionary.Clear();
            }
        }

        var serverInfo = ServiceProvider.Value.GetService<IServiceInfo>();
        await CacheProvider.Value.SetAsync(MaintCachingConsts.CfgPreheatedKey, serverInfo.Version, TimeSpan.FromSeconds(MaintCachingConsts.OneYear));
        _logger.LogInformation($"finished({cfgDtos.Count}) preheat {MaintCachingConsts.CfgSingleKeyPrefix}");
    }

    #endregion
}