using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SpringComp.Functions.Extensions.StaticWebApps;

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
    public async Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "message")] HttpRequest req,
        [UserInfo] ClaimsPrincipal identity,
        ILogger log)
    {
      log.LogInformation("C# HTTP trigger function processed a request.");

      var dictionary = new Dictionary<string, object>();

      foreach (var claim in identity.Claims)
      {
        log.LogTrace($"{claim.Type}: {claim.Value}");
        if (dictionary.ContainsKey(claim.Type))
        {
          var items = dictionary[claim.Type] as List<string>;
          if (items == null)
            items = new List<string>(new[] { dictionary[claim.Type] as string });
          items.Add(claim.Value);
          dictionary[claim.Type] = items;
        }
        else
        {
          dictionary.Add(claim.Type, claim.Value);
        }
      }

      string output;
      using (var stream = new MemoryStream())
      {
        await System.Text.Json.JsonSerializer.SerializeAsync(stream, dictionary);
        output = Encoding.UTF8.GetString(stream.ToArray());
        log.LogTrace(output);
      }

      var response = new HttpResponseMessage();
      response.Content = new StringContent(output, Encoding.UTF8, "application/json");
      return response;
    }
  }
}
