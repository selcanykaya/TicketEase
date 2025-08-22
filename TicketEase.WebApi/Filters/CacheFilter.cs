using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace TicketEase.WebApi.Filters
{
    public class CacheFilter : Attribute, IAsyncActionFilter
    {
        private readonly int _duration;

        public CacheFilter(int duration = 60)
        {
            _duration = duration;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Check if the response is already cached
            if (context.HttpContext.Request.Method != "GET")
            {
                await next();
                return;
            }
           
            var cache = context.HttpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;
            if (cache == null) 
            { 
                await next(); 
                return; 
            }

            var cacheKey = GenerateCacheKeyFromRequest(context);

            if (cache.TryGetValue(cacheKey, out var cachedValue))
            {
                context.Result = new OkObjectResult(cachedValue);
                return;
            }

            var executedContext = await next();

            if (executedContext.Result is ObjectResult objectResult && objectResult.StatusCode == 200)
            {
                cache.Set(cacheKey, objectResult.Value, TimeSpan.FromSeconds(_duration));
            }
        }
        private string GenerateCacheKeyFromRequest(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var key = request.Path.ToString();

            if (request.QueryString.HasValue)
                key += request.QueryString.Value;

            return key;
        }
    }
    }

