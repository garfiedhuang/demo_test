namespace Hkust.Platform.WebApi.Authentication;

/// <summary>
/// 身份认证
/// </summary>
public class BearerAuthenticationLocalProcessor : AbstractAuthenticationProcessor
{
    private readonly IAccountAppService _accountAppService;

    public BearerAuthenticationLocalProcessor(IAccountAppService accountAppService) => _accountAppService = accountAppService;

    protected override async Task<(string ValidationVersion, int Status)> GetValidatedInfoAsync(long userId)//获取验证信息 mark by garfield 20221019
    {
        var validatedInfo = await _accountAppService.GetUserValidatedInfoAsync(userId);//从REDIS缓存中读取用户登录信息 mark by garfield 20221019
        if (validatedInfo is null)
            return (null, 0);

        return (validatedInfo.ValidationVersion, validatedInfo.Status);
    }
}
