namespace Hkust.Platform.Application.Contracts.Dtos
{
    public class UserSetRoleDto : InputDto
    {
        public long[] RoleIds { get; set; }
    }
}