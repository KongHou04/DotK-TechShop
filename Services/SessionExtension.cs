using Newtonsoft.Json;

namespace DotK_TechShop.Services;

public static class SessionExtensions
{
    public static T? GetObject<T>(this ISession session, string key)
    {
        var jsonString = session.GetString(key);
        return jsonString == null ? default : JsonConvert.DeserializeObject<T>(jsonString);
    }

    public static void SetObject<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }
}