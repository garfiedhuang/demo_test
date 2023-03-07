using FluentValidation;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Hkust.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// Controllers 注册
    /// Sytem.Text.Json 配置
    /// FluentValidation 注册
    /// ApiBehaviorOptions 配置
    /// </summary>
    protected virtual void AddControllers()
    {
        Services
            .AddControllers(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))//异常拦截器 拦截 StatusCode>=500 的异常
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());//时间格式处理yyyy-MM-dd HH:mm:ss
                options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
                options.JsonSerializerOptions.Encoder = SystemTextJson.GetHkustDefaultEncoder();
                //该值指示是否允许、不允许或跳过注释。
                options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                //dynamic与匿名类型序列化设置
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                //dynamic
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                //匿名类型
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            })//JSON序列化配置 add by garfield 20221019
            .AddFluentValidation(cfg =>
            {
                //Continue 验证失败，继续验证其他项
                ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Continue;
                //cfg.ValidatorOptions.DefaultClassLevelCascadeMode = FluentValidation.CascadeMode.Continue;
                // Optionally set validator factory if you have problems with scope resolve inside validators.
                // cfg.ValidatorFactoryType = typeof(HttpContextServiceProviderValidatorFactory);
            });//小型业务对象验证组件 add by garfield 20221019

        Services
            .Configure<ApiBehaviorOptions>(options => //统一模型验证配置 add by garfield 20221019
            {
                //调整参数验证返回信息格式
                //关闭自动验证
                //options.SuppressModelStateInvalidFilter = true;
                //格式化验证信息
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var problemDetails = new ProblemDetails
                    {
                        Detail = context.ModelState.GetValidationSummary("<br>"),
                        Title = "参数错误",
                        Status = (int)HttpStatusCode.BadRequest,
                        Type = "https://httpstatuses.com/400",
                        Instance = context.HttpContext.Request.Path
                    };

                    return new ObjectResult(problemDetails)
                    {
                        StatusCode = problemDetails.Status
                    };
                };
            });
        //add skyamp
        //_services.AddSkyApmExtensions().AddCaching();
    }
}
