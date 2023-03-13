namespace Hkust.Platform.Application.Contracts.Services;

/// <summary>
/// 字典管理
/// </summary>
public interface IDictAppService : IAppService
{
    /// <summary>
    /// 新增字典
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "新增字典")]
    Task<AppSrvResult<long>> CreateAsync(DictCreationDto input);

    /// <summary>
    /// 修改字典
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "修改字典")]
    [CachingEvict(CacheKeyPrefix = MaintCachingConsts.DictSingleKeyPrefix)]
    [UnitOfWork]
    Task<AppSrvResult> UpdateAsync([CachingParam] long id, DictUpdationDto input);

    /// <summary>
    /// 删除字典
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [OperateLog(LogName = "删除字典")]
    [CachingEvict(CacheKeyPrefix = MaintCachingConsts.DictSingleKeyPrefix)]
    Task<AppSrvResult> DeleteAsync([CachingParam] long id);

    /// <summary>
    /// 字典列表
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    Task<List<DictDto>> GetListAsync(DictSearchDto search);

    /// <summary>
    /// 获取单个字典
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [CachingAble(CacheKeyPrefix = MaintCachingConsts.DictSingleKeyPrefix,Expiration = MaintCachingConsts.OneMonth)]
    Task<DictDto> GetAsync([CachingParam] long id);
}