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
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace FunctionApp
{
  public class HttpTrigger
  {
    private readonly IConfiguration configuration_;
    private readonly IHttpClientFactory clientFactory_;
    public HttpTrigger(IConfiguration configuration, IHttpClientFactory clientFactory)
    {
      configuration_ = configuration;
      clientFactory_ = clientFactory;
    }

    [FunctionName("HttpTrigger")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "message")] HttpRequestMessage req,
        ClaimsPrincipal identity,
        ILogger log)
    {
      log.LogInformation("C# HTTP trigger function processed a request.");

      string name = "World!"; // req.Query["name"];

      var dictionary = new Dictionary<string, string>()
      {
        ["name"] = name,
        ["message"] = "Hello, world! This function was called programatically.",
      };

      foreach (var claim in identity.Claims)
        dictionary.Add(claim.Type, claim.Value);

      var context = (DefaultHttpContext)req.Properties["HttpContext"];
      var host = new Uri(context.Request.Scheme + "://" + context.Request.Host.Value, UriKind.Absolute);
      var auth = new Uri(host, "/.auth/me");

      var client = clientFactory_.CreateClient("client");
      var result = await client.GetAsync(auth);
      var response = await result.Content.ReadAsStringAsync();

      dictionary.Add("host", host.OriginalString);
      dictionary.Add("auth", auth.OriginalString);
      dictionary.Add(".auth/me", response);

      await Task.CompletedTask;
      return new OkObjectResult(dictionary);
      //return new StatusCodeResult(StatusCodes.Status401Unauthorized);
    }
  }
}
