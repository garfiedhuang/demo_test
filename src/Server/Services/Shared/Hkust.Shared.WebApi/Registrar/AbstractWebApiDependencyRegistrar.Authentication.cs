﻿using Hkust.Shared.WebApi.Authentication;
using Hkust.Shared.WebApi.Authentication.Basic;
using Hkust.Shared.WebApi.Authentication.Hybrid;

namespace Hkust.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// <summary>
    /// 注册身份认证组件
    /// </summary>
    protected virtual void AddAuthentication<TAuthenticationHandler>()
        where TAuthenticationHandler : AbstractAuthenticationProcessor
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();//清除映射 mark by garfield 20221019
        Services
            .AddScoped<AbstractAuthenticationProcessor, TAuthenticationHandler>();//认证服务注册IOC
        Services
            .AddAuthentication(HybridDefaults.AuthenticationScheme)//混合身份认证 mark by garfield 20221019
            .AddHybrid()
            .AddBasic(options => options.Events.OnTokenValidated = (context) =>
            {
                var userContext = context.HttpContext.RequestServices.GetService<UserContext>();
                var claims = context.Principal.Claims;
                userContext.Id = long.Parse(claims.First(x => x.Type == BasicDefaults.NameId).Value);
                userContext.Account = claims.First(x => x.Type == BasicDefaults.UniqueName).Value;
                userContext.Name = claims.First(x => x.Type == BasicDefaults.Name).Value;
                userContext.RemoteIpAddress = context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                return Task.CompletedTask;
            })
            .AddBearer(options => options.Events.OnTokenValidated = (context) =>
            {
                var userContext = context.HttpContext.RequestServices.GetService<UserContext>();
                var claims = context.Principal.Claims;
                userContext.Id = long.Parse(claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value);
                userContext.Account = claims.First(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value;
                userContext.Name = claims.First(x => x.Type == JwtRegisteredClaimNames.Name).Value;
                userContext.RoleIds = claims.First(x => x.Type == "roleids").Value;
                userContext.RemoteIpAddress = context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                return Task.CompletedTask;
            })
            //.AddJwtBearer(options =>
            //{
            //    var jwtConfig = Configuration.GetJWTSection().Get<JwtConfig>();
            //    options.TokenValidationParameters = JwtSecurityTokenHandlerExtension.GenarateTokenValidationParameters(jwtConfig);
            //    options.Events = JwtSecurityTokenHandlerExtension.GenarateJwtBearerEvents();
            //})
            ;
    }
}
