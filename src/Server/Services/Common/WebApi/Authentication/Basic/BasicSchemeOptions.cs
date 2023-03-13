namespace Hkust.Common.WebApi.Authentication.Basic;

public class BasicSchemeOptions : AuthenticationSchemeOptions
{
    public BasicSchemeOptions()
    {
        Events = new BasicEvents();
    }

    public new BasicEvents Events
    {
        get { return (BasicEvents)base.Events; }
        set { base.Events = value; }
    }
}
