using Hkust.Shared.WebApi.Authentication.Basic;
using Hkust.Shared.WebApi.Authorization;

namespace Hkust.Shared.WebApi.Authentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
[Obsolete("please use HkustAuthorizeAttribute")]
public class PermissionAttribute : AuthorizeAttribute
{
    public const string JwtWithBasicSchemes = $"{JwtBearerDefaults.AuthenticationScheme},{BasicDefaults.AuthenticationScheme}";

    public string[] Codes { get; set; }

    public PermissionAttribute(string code, string schemes = JwtBearerDefaults.AuthenticationScheme)
        : this(new string[] { code }, schemes)
    {
    }

    public PermissionAttribute(string[] codes, string schemes = JwtBearerDefaults.AuthenticationScheme)
    {
        Codes = codes;
        Policy = AuthorizePolicy.Default;
        if (schemes.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(schemes));
        else
            AuthenticationSchemes = schemes;
    }
}