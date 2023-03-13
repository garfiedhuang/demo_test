namespace Hkust.Platform.WebApi.Authorization;

/// <summary>
/// 授权
/// </summary>
public sealed class PermissionLocalHandler : AbstractPermissionHandler
{
    private readonly IUserAppService _userAppService;

    public PermissionLocalHandler(IUserAppService userAppService) => _userAppService = userAppService;

    protected override async Task<bool> CheckUserPermissions(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds)
    {
        var permissions = await _userAppService.GetPermissionsAsync(userId, requestPermissions, userBelongsRoleIds);//通过RoleId反查MenuCode，再与requestPermissions取交集返回 add by garfield 20230308
        return permissions.IsNotNullOrEmpty();
    }
}