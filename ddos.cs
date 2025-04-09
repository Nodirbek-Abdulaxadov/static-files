using System.Diagnostics;
using System.Net.Http;
using System.Threading.Channels;

Random random = new();

// 🔥 List of target URLs
string[] urls =
[
    "http://localhost:7100/api/test/applications?page=",
    "http://localhost:7100/api/test/issues?page=",
    "http://localhost:7100/api/test/applicants?page="
];

int totalRequests = 50000;
int concurrency = 5000;

var responseTimes = Channel.CreateUnbounded<long>();
int successCount = 0;
int failureCount = 0;

var httpClient = new HttpClient();
var stopwatch = Stopwatch.StartNew();
var throttler = new SemaphoreSlim(concurrency);

var tasks = new List<Task>();

for (int i = 0; i < totalRequests; i++)
{
    await throttler.WaitAsync();

    var task = Task.Run(async () =>
    {
        var sw = Stopwatch.StartNew();

        try
        {
            // 🔄 Randomly choose a URL
            var targetUrl = urls[random.Next(urls.Length)] + random.Next(1, 100);
            var response = await httpClient.GetAsync(targetUrl);
            sw.Stop();

            if (response.IsSuccessStatusCode)
            {
                Interlocked.Increment(ref successCount);
                await responseTimes.Writer.WriteAsync(sw.ElapsedMilliseconds);
            }
            else
            {
                Interlocked.Increment(ref failureCount);
                _ = Task.Run(() =>
                {
                    Console.WriteLine($"Request failed: {targetUrl} - Status Code: {response.StatusCode} - Elapsed Time: {sw.ElapsedMilliseconds} ms");
                });
            }
        }
        catch
        {
            Interlocked.Increment(ref failureCount);
        }
        finally
        {
            throttler.Release();
        }
    });

    tasks.Add(task);
}

await Task.WhenAll(tasks);

stopwatch.Stop();
responseTimes.Writer.Complete();

var allTimes = new List<long>();
await foreach (var time in responseTimes.Reader.ReadAllAsync())
{
    allTimes.Add(time);
}

var min = allTimes.Min();
var max = allTimes.Max();
var avg = allTimes.Average();
var totalSeconds = stopwatch.Elapsed.TotalSeconds;
var rps = successCount / totalSeconds;

Console.WriteLine("------ Stress Test Results ------");
Console.WriteLine($"Total Requests: {totalRequests}");
Console.WriteLine($"Concurrency: {concurrency}");
Console.WriteLine($"Success: {successCount}");
Console.WriteLine($"Failures: {failureCount}");
Console.WriteLine($"Min Response Time: {min} ms");
Console.WriteLine($"Max Response Time: {max} ms");
Console.WriteLine($"Avg Response Time: {avg:F2} ms");
Console.WriteLine($"Requests Per Second (RPS): {rps:F2}");
