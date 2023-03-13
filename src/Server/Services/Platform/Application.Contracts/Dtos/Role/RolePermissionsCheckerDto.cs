namespace Hkust.Platform.Application.Contracts.Dtos;

public class RolePermissionsCheckerDto : IDto
{
    public IEnumerable<long> RoleIds { get; set; }
    public IEnumerable<string> Permissions { get; set; }
}