using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TicketEase.WebApi.Filters
{
    public class TimeControlFilter : ActionFilterAttribute
    {
        public string StartTime { get; set; } 
        public string EndTime { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var now = DateTime.Now.TimeOfDay;

            StartTime = "00:00";
            EndTime = "05:00";

            if (now >= TimeSpan.Parse(StartTime) && now <= TimeSpan.Parse(EndTime))
            {
               
                context.Result = new ContentResult
                {
                    Content = "This action cannot be executed at this time.",
                    StatusCode = 403
                };
            }
            else
            {
               
                base.OnActionExecuting(context);
            }

        }
    }

}
