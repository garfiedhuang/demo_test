using Hkust.Shared.WebApi.Registrar;

namespace Hkust.Cus.WebApi.Registrar;

public sealed class CustWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public CustWebApiDependencyRegistrar(IServiceCollection services)
        : base(services)
    {
    }

    public CustWebApiDependencyRegistrar(IApplicationBuilder app)
       : base(app)
    {
    }

    public override void AddHkust()
    {
        AddWebApiDefault();
        AddHealthChecks(true, true, true, true);
    }

    public override void UseHkust()
    {
        UseWebApiDefault();
    }
}