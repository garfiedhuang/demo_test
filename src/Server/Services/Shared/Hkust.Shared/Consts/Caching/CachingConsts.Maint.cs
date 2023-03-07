namespace Hkust.Shared.Consts.Caching.Maint;

public class CachingConsts : Common.CachingConsts
{
    public const string DictPreheatedKey = "hkust:maint:dicts:preheated";
    public const string CfgPreheatedKey = "hkust:maint:cfgs:preheated";

    public const string DictSingleKeyPrefix = "hkust:maint:dicts:single";
    public const string CfgSingleKeyPrefix = "hkust:maint:cfgs:single";

    public const string BloomfilterOfCacheKey = "hkust:maint:bloomfilter:cachekeys";
}