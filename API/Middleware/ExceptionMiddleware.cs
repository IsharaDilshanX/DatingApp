using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, 
        ILogger<ExceptionMiddleware> logger, IHostEnvironment env) //RequestDelegate - is what's next in the "middleware pipeline", ILogger - so we can logout terminal, IHostEnvironment - to verify which environment we are working(in production/ in development)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) //HttpContext - so we have access to httprequests that's coming in.
        {
            try
            {
                await _next(context); // passing context to the next piece of middleware.
            }
            catch (Exception ex) // since this is the top middleware we need to catch the exceptions coming from the all the middleware.
            {
                _logger.LogError(ex, ex.Message); //if not this, then our exception is going to be silence in our terminal. 
                context.Response.ContentType = "application/json"; // specify what is the type for the error description
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError; //we are getting http status code

                var response = _env.IsDevelopment() //we check if this is development environment we are running on
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) //execute if this isDevelopment environment.
                    : new ApiException(context.Response.StatusCode, "Internal Server Error"); //execute if this isProduction environment.

                var options = new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase}; //options to enable serialize the response to json response (and in normal camelcase).

                var json = JsonSerializer.Serialize(response, options); 

                await context.Response.WriteAsync(json);
            }
        }
    }
} 