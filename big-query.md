Great! Here's a **clean, production-ready** setup to calculate Firebase statistics in a **C# app using Dependency Injection (DI)** — suitable for **ASP.NET Core or Console applications**.

---

## ✅ Overview

### Features:

* Enum for `FirebaseStatType`
* Interface `IFirebaseStatisticsService`
* Implementation `FirebaseStatisticsService` using BigQuery
* Full dependency injection setup
* One-time setup instructions

---

## 🧱 Step-by-Step Guide (from scratch)

---

### 📦 Step 1: Install Required NuGet Packages

```bash
dotnet add package Google.Cloud.BigQuery.V2
dotnet add package Google.Apis.Auth
```

---

### 🧾 Step 2: Create the Enum

```csharp
public enum FirebaseStatType
{
    RegisteredUsersTotal, // Placeholder — not handled in BigQuery
    ActiveUsersDaily,
    ActiveUsersMonthly,
    AppOpenUsersDaily,
    AppOpenUsersMonthly
}
```

---

### 🧩 Step 3: Create the Interface

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IFirebaseStatisticsService
{
    Task<Dictionary<string, long>> GetStatisticAsync(FirebaseStatType statType, DateTime from, DateTime to);
}
```

---

### 🔧 Step 4: Create the Implementation

```csharp
using Google.Cloud.BigQuery.V2;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FirebaseStatisticsOptions
{
    public string ProjectId { get; set; } = default!;
    public string Dataset { get; set; } = default!;
    public string CredentialsPath { get; set; } = default!;
}

public class FirebaseStatisticsService : IFirebaseStatisticsService
{
    private readonly BigQueryClient _client;
    private readonly FirebaseStatisticsOptions _options;

    public FirebaseStatisticsService(IOptions<FirebaseStatisticsOptions> options)
    {
        _options = options.Value;
        _client = BigQueryClient.Create(_options.ProjectId, GoogleCredential.FromFile(_options.CredentialsPath));
    }

    public async Task<Dictionary<string, long>> GetStatisticAsync(FirebaseStatType statType, DateTime from, DateTime to)
    {
        string suffixFrom = from.ToString("yyyyMMdd");
        string suffixTo = to.ToString("yyyyMMdd");

        string selectClause, groupByClause, eventName;

        switch (statType)
        {
            case FirebaseStatType.ActiveUsersDaily:
                selectClause = "DATE(TIMESTAMP_MICROS(event_timestamp)) AS day";
                groupByClause = "day";
                eventName = "user_engagement";
                break;

            case FirebaseStatType.ActiveUsersMonthly:
                selectClause = "FORMAT_DATE('%Y-%m', DATE(TIMESTAMP_MICROS(event_timestamp))) AS month";
                groupByClause = "month";
                eventName = "user_engagement";
                break;

            case FirebaseStatType.AppOpenUsersDaily:
                selectClause = "DATE(TIMESTAMP_MICROS(event_timestamp)) AS day";
                groupByClause = "day";
                eventName = "session_start";
                break;

            case FirebaseStatType.AppOpenUsersMonthly:
                selectClause = "FORMAT_DATE('%Y-%m', DATE(TIMESTAMP_MICROS(event_timestamp))) AS month";
                groupByClause = "month";
                eventName = "session_start";
                break;

            default:
                throw new NotSupportedException($"Stat type '{statType}' not supported in BigQuery.");
        }

        string query = $@"
SELECT
  {selectClause},
  COUNT(DISTINCT user_pseudo_id) AS count
FROM
  `{_options.ProjectId}.{_options.Dataset}.events_*`
WHERE
  _TABLE_SUFFIX BETWEEN '{suffixFrom}' AND '{suffixTo}'
  AND event_name = '{eventName}'
GROUP BY {groupByClause}
ORDER BY {groupByClause}";

        var result = await _client.ExecuteQueryAsync(query, parameters: null);

        var stats = new Dictionary<string, long>();
        foreach (var row in result)
        {
            stats[row[0].ToString()] = (long)row["count"];
        }

        return stats;
    }
}
```

---

### 🧰 Step 5: Register in `Startup.cs` or Program.cs

#### For ASP.NET Core (Minimal API or MVC):

```csharp
builder.Services.Configure<FirebaseStatisticsOptions>(builder.Configuration.GetSection("Firebase"));
builder.Services.AddSingleton<IFirebaseStatisticsService, FirebaseStatisticsService>();
```

#### For Console App:

```csharp
var services = new ServiceCollection();

services.Configure<FirebaseStatisticsOptions>(config =>
{
    config.ProjectId = "your-project-id";
    config.Dataset = "your_dataset";
    config.CredentialsPath = "path-to-service-account.json";
});
services.AddSingleton<IFirebaseStatisticsService, FirebaseStatisticsService>();

var provider = services.BuildServiceProvider();
```

---

### ⚙️ Step 6: AppSettings (Optional)

In `appsettings.json`:

```json
"Firebase": {
  "ProjectId": "your-project-id",
  "Dataset": "your_dataset",
  "CredentialsPath": "path/to/service-account.json"
}
```

---

### ▶️ Step 7: Use the Service

```csharp
var service = provider.GetRequiredService<IFirebaseStatisticsService>();

var stats = await service.GetStatisticAsync(
    FirebaseStatType.ActiveUsersDaily,
    new DateTime(2025, 7, 1),
    new DateTime(2025, 7, 24)
);

foreach (var entry in stats)
{
    Console.WriteLine($"{entry.Key}: {entry.Value}");
}
```

---

## ✅ Output Sample

```
2025-07-01: 530
2025-07-02: 602
...
2025-07-24: 590
```

---

Would you like:

* Support for Firebase registered users (`FirebaseAuth`) added as a separate service?
* REST API controller to expose this in ASP.NET Core?

Let me know!
