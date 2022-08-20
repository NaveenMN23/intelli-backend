using Microsoft.Extensions.Primitives;
using IntelliCRMAPIService.Utility;
using System.Net;

namespace IntelliCRMAPIService.Middleware
{

   public class AuthenticationMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;
        public string MiddlewareName { get; }

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            MiddlewareName = GetType().Name;
        }

        public async Task Invoke(HttpContext httpContext, IJwtUtils jwtUtils,IConfiguration configuration)
        {
            _logger.LogInformation("Authentication started");
            var allowAnonymous = httpContext.Request.Path.ToString().ToLower().Contains("healthcheck") || httpContext.Request.Path.ToString().ToLower().Contains("favicon.ico");

            if (!allowAnonymous)
            {
                var request = httpContext.Request;

                if (request.Headers.TryGetValue("Authorization", out StringValues headerValue))
                {
                    string httpAuthorization = headerValue.FirstOrDefault();
                    if (httpAuthorization != null && httpAuthorization.StartsWith("Basic") && httpAuthorization.Length > 6)
                    {
                        _logger.LogInformation("Basic authentication started");
                        var Authorization = Convert.FromBase64String(httpAuthorization.Substring(6, httpAuthorization.Length - 6));
                        
                        string credText = System.Text.Encoding.UTF8.GetString(Authorization);
                        var credentials = credText.Split(new[] { ":" }, StringSplitOptions.None);
                        
                        if (credentials != null && credentials.Length != 2)
                        {
                            await ReturnErrorResponse(httpContext);
                        }
                        string UserName = credentials.GetValue(0).ToString();
                        string Password = credentials.GetValue(1).ToString();

                        if (UserName.Equals(configuration["Auth:UserName"]) && Password.Equals(configuration["Auth:Password"]))
                            await _next(httpContext);
                        else
                        {
                            await ReturnErrorResponse(httpContext);
                            _logger.LogInformation("Basic authentication failed");
                        }
                        _logger.LogInformation("Basic authentication completed");
                    }
                    else if(httpAuthorization != null)
                    {
                        _logger.LogInformation("JWT token - authentication started");

                        var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                        var userId = jwtUtils.ValidateJwtToken(token);
                        if (userId != null)
                        {
                            _logger.LogInformation("JWT token - authentication completed");
                            // attach user to context on successful jwt validation
                            httpContext.Items["User"] = userId;

                            await _next(httpContext);
                        }
                        else
                        {
                            //GAuth.OAuth2.Web.AuthWebUtility.
                            //await _next(context);
                            await ReturnErrorResponse(httpContext);
                            _logger.LogInformation("JWT token - authentication Failed");
                        }
                    }
                    else
                    {
                        _logger.LogInformation("No authentication token supplied");
                        await ReturnErrorResponse(httpContext);
                    }
                }
                else
                {
                    _logger.LogInformation("No authentication token supplied");

                    await ReturnErrorResponse(httpContext);
                }
            }
            else
            {
                await _next(httpContext);
            }
        }

        private async Task ReturnErrorResponse(HttpContext context)
        {
            _logger.LogInformation("Token validation is failed and creating error reponse");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.StartAsync();
        }
    }
}
