using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure;
using Azure.Data.Tables;

namespace Company.Function;

public class CounterEntity : ITableEntity
{
    public string PartitionKey { get; set; } = "1";
    public string RowKey { get; set; } = "1";
    public int Counter { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}

public class GetResumeCounter
{
    private readonly ILogger<GetResumeCounter> _logger;

    public GetResumeCounter(ILogger<GetResumeCounter> logger)
    {
        _logger = logger;
    }

    [Function("GetResumeCounter")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var table = new TableClient(
            Environment.GetEnvironmentVariable("AzureResumeConnectionString"),
            "AzureResume");

        var counter = await table.GetEntityAsync<CounterEntity>("1", "1");
        counter.Value.Counter++;
        await table.UpdateEntityAsync(counter.Value, counter.Value.ETag, TableUpdateMode.Replace);

        return new OkObjectResult(new { count = counter.Value.Counter });
    }
}