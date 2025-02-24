using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SWGW.WebApp.Models;
using System.Text.Json;

namespace SWGW.WebApp.Platform.PageModelHelpers;

public static class TempDataExtensions
{
    private static void Set<T>(this ITempDataDictionary tempData, string key, T value) where T : class =>
        tempData[key] = JsonSerializer.Serialize(value);

    private static T? Get<T>(this ITempDataDictionary tempData, string key) where T : class
    {
        tempData.TryGetValue(key, out var o);
        return o is null ? null : JsonSerializer.Deserialize<T>((string)o);
    }

    public static void SetDisplayMessage(this ITempDataDictionary tempData, DisplayMessage.AlertContext context,
        string message, string detail) => tempData.SetDisplayMessage(context, message, [detail]);

    public static void SetDisplayMessage(this ITempDataDictionary tempData, DisplayMessage.AlertContext context,
        string message, List<string>? details = null) =>
        tempData.Set(nameof(DisplayMessage), new DisplayMessage(context, message, details ?? []));

    public static DisplayMessage? GetDisplayMessage(this ITempDataDictionary tempData) =>
        tempData.Get<DisplayMessage>(nameof(DisplayMessage));
}
