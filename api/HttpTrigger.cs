using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FunctionApp
{
  public static class HttpTrigger
  {
    [FunctionName("HttpTrigger")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "message")] HttpRequest req,
        ILogger log)
    {
      log.LogInformation("C# HTTP trigger function processed a request.");

      string name = req.Query["name"];

      var dictionary = new Dictionary<string, string>()
      {
        ["name"] = name,
        ["message"] = "Hello, world! This function was called programatically.",
      };

      await Task.CompletedTask;
      return new OkObjectResult(dictionary);
    }
  }
}
