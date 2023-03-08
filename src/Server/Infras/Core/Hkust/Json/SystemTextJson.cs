using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Hkust.Infras.Core.Json;

public static class SystemTextJson
{
    public static JsonSerializerOptions GetHkustDefaultOptions(Action<JsonSerializerOptions>? configOptions = null)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new DateTimeConverter());
        options.Converters.Add(new DateTimeNullableConverter());
        options.Encoder = GetHkustDefaultEncoder();
        //该值指示是否允许、不允许或跳过注释。
        options.ReadCommentHandling = JsonCommentHandling.Skip;
        //dynamic与匿名类型序列化设置
        options.PropertyNameCaseInsensitive = true;
        //dynamic
        options.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        //匿名类型
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        configOptions?.Invoke(options);
        return options;
    }

    public static JavaScriptEncoder GetHkustDefaultEncoder() => JavaScriptEncoder.Create(new TextEncoderSettings(UnicodeRanges.All));
}