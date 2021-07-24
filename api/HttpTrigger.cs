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
using System.Linq;
using System.Text;
using System.Text.Json;

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

      var identity = Parse(req);
      foreach (var claim in identity.Claims)
      {
        log.LogTrace($"{claim.Type}: {claim.Value}");
        dictionary.Add(claim.Type, claim.Value);
      }

      await Task.CompletedTask;
      return new OkObjectResult(dictionary);
      //return new StatusCodeResult(StatusCodes.Status401Unauthorized);
    }
    private class ClientPrincipal
    {
      public string IdentityProvider { get; set; }
      public string UserId { get; set; }
      public string UserDetails { get; set; }
      public IEnumerable<string> UserRoles { get; set; }
    }

    public static ClaimsPrincipal Parse(HttpRequest req)
    {
      var principal = new ClientPrincipal();

      if (req.Headers.TryGetValue("x-ms-client-principal", out var header))
      {
        var data = header[0];
        var decoded = Convert.FromBase64String(data);
        var json = Encoding.ASCII.GetString(decoded);
        principal = System.Text.Json.JsonSerializer.Deserialize<ClientPrincipal>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
      }

      principal.UserRoles = principal.UserRoles?.Except(new string[] { "anonymous" }, StringComparer.CurrentCultureIgnoreCase);

      if (!principal.UserRoles?.Any() ?? true)
      {
        return new ClaimsPrincipal();
      }

      var identity = new ClaimsIdentity(principal.IdentityProvider);
      identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, principal.UserId));
      identity.AddClaim(new Claim(ClaimTypes.Name, principal.UserDetails));
      identity.AddClaims(principal.UserRoles.Select(r => new Claim(ClaimTypes.Role, r)));

      return new ClaimsPrincipal(identity);
    }
  }
}
