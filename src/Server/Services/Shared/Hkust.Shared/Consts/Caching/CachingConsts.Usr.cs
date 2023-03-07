namespace Hkust.Shared.Consts.Caching.Usr;

public class CachingConsts : Common.CachingConsts
{
    //cache key
    public const string MenuListCacheKey = "hkust:usr:menus:list";

    public const string MenuTreeListCacheKey = "hkust:usr:menus:tree";
    public const string MenuRelationCacheKey = "hkust:usr:menus:relation";
    public const string MenuCodesCacheKey = "hkust:usr:menus:codes";

    public const string DetpListCacheKey = "hkust:usr:depts:list";
    public const string DetpTreeListCacheKey = "hkust:usr:depts:tree";
    public const string DetpSimpleTreeListCacheKey = "hkust:usr:depts:tree:simple";

    public const string RoleListCacheKey = "hkust:usr:roles:list";

    //cache prefix
    public const string UserValidatedInfoKeyPrefix = "hkust:usr:users:validatedinfo";

    //bloomfilter
    public const string BloomfilterOfAccountsKey = "hkust:usr:bloomfilter:accouts";
    public const string BloomfilterOfCacheKey = "hkust:usr:bloomfilter:cachekeys";
}