You're on the right track with your middleware. To fully support **localized/translatable error messages** and dynamic extensibility (e.g., allowing new untranslated error messages), here's a step-by-step plan to **refactor and optimize** your middleware and related structure.

---

### ✅ Requirements Breakdown

1. **Translate known errors using `.resx` files** via `IStringLocalizer`.
2. **Accept language via `Accept-Language` header**.
3. **Allow dynamic (untranslated) error messages for unknown errors**.
4. **Log and notify with error code + localized message**.
5. **Ensure the response is always JSON and doesn't fail silently if headers were already sent.**

---

## 🔧 Step-by-Step Optimization

---

### 1. **Improve `ErrorResponse` for extensibility**

```csharp
public class ErrorResponse
{
    [JsonIgnore]
    public int Status { get; }

    [JsonPropertyName("code")]
    public string Code { get; }

    [JsonPropertyName("message")]
    public string Message { get; }

    public ErrorResponse(int status, string code, string message)
    {
        Status = status;
        Code = code;
        Message = message;
    }
}
```

---

### 2. **Add localization support**

You already inject `IStringLocalizer<SharedResource> L`. Now read `Accept-Language` and set `CultureInfo.CurrentCulture` accordingly.

Add this **before calling `next(context)`** in `Invoke`:

```csharp
var culture = context.Request.Headers["Accept-Language"].ToString();
if (!string.IsNullOrWhiteSpace(culture))
{
    try
    {
        var cultureInfo = new CultureInfo(culture);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
    }
    catch (CultureNotFoundException)
    {
        // Optional: log unsupported culture, fallback to default
    }
}
```

---

### 3. **Refactor `HandleExceptionAsync` for clarity and translation**

```csharp
private async Task HandleExceptionAsync(HttpContext context, Exception exception)
{
    var activity = Activity.Current;
    activity?.RecordException(exception);
    activity?.SetStatus(ActivityStatusCode.Error);

    ErrorResponse response = exception switch
    {
        MyUzException ex => new ErrorResponse(
            400,
            $"{ex.Service}---{ex.Method}---{ex.Message}",
            @L[ex.Message] ?? ex.Message),

        MultiException e when e.MultiErrorWrapper?.Error != null =>
            new ErrorResponse(400, e.MultiErrorWrapper.Error.Code, e.MultiErrorWrapper.Error.Details),

        MultiException =>
            new ErrorResponse(500, "500", "MultiException without details"),

        _ => BuildGenericError(exception)
    };

    logger.LogError(exception, response.Code);

    if (!context.Response.HasStarted)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.Status;

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
    else
    {
        logger.LogWarning("Response already started; error not returned to client.");
    }

    if (env.IsProduction())
    {
        var shortMessage = response.Message.Length > 4096
            ? response.Message[..4096]
            : response.Message;

        await alertaGram.NotifyErrorAsync(shortMessage, $"{response.Status}", "Simkomat");
    }
}

private ErrorResponse BuildGenericError(Exception exception)
{
    var defaultMessage = exception.Message ?? "Unknown error";

    return defaultMessage switch
    {
        "004" => new ErrorResponse(400, "004", @L["IdentificationFaceSpoofing"] ?? "Identification face spoofing"),
        "005" => new ErrorResponse(400, "005", @L["IdentificationFace"] ?? "Identification face"),
        "006" => new ErrorResponse(400, "006", @L["IdentificationData"] ?? "Identification data"),
        "401" => new ErrorResponse(401, "401", @L["InvalidServiceResponse"] ?? "Service returned invalid response"),
        "5000" => new ErrorResponse(400, "5000", @L["LimitSimCard"] ?? "Limit sim card"),
        "5009" => new ErrorResponse(400, "5009", @L["FinancialObligations"] ?? "Financial obligations"),
        _ => new ErrorResponse(400, "400", @L[defaultMessage] ?? defaultMessage)
    };
}
```

---

### 4. **Create `.resx` Files for Localization**

* Add `SharedResource.resx`, `SharedResource.uz.resx`, `SharedResource.ru.resx`, etc.
* Use meaningful keys:

  ```xml
  <data name="IdentificationFaceSpoofing" xml:space="preserve">
    <value>Face spoofing detected</value>
  </data>
  ```

---

### 5. **Register Localization in `Program.cs`**

```csharp
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = new[] { new CultureInfo("en"), new CultureInfo("uz"), new CultureInfo("ru") },
    SupportedUICultures = new[] { new CultureInfo("en"), new CultureInfo("uz"), new CultureInfo("ru") }
});
```

---

## ✅ Summary

With these improvements:

| Feature                                       | Result                                |
| --------------------------------------------- | ------------------------------------- |
| `Accept-Language` parsing                     | ✅ Done                                |
| `resx` localization                           | ✅ Used via `IStringLocalizer`         |
| Dynamic fallback for unknown errors           | ✅ Used default message                |
| Centralized and extendable error construction | ✅ Refactored into `BuildGenericError` |
| Logging & alerting                            | ✅ Kept intact                         |

---

If you'd like, I can generate a starter `.resx` file or extend this for validation/model binding errors as well.
