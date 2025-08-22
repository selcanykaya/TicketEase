using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using TicketEase.Business.Exceptions;

namespace TicketEase.WebApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // 401 ve 403 response'larını yakala ve JSON formatına çevir
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(
                        JsonSerializer.Serialize(new { Message = "Unauthorized: Please login." })
                    );
                }
                else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(
                        JsonSerializer.Serialize(new { Message = "Forbidden: You do not have permission." })
                    );
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            string message = exception.Message;

            switch (exception)
            {
                case NotFoundException:
                    status = HttpStatusCode.NotFound; // 404
                    break;
                case ConflictException:
                    status = HttpStatusCode.Conflict; // 409
                    break;
                case ForbiddenException:
                    status = HttpStatusCode.Forbidden; // 403
                    break;
                case UnauthorizedException:
                    status = HttpStatusCode.Unauthorized; // 401
                    break;
                case ValidationException:
                    status = HttpStatusCode.BadRequest; // 400
                    break;
                case InternalServerException:
                    status = HttpStatusCode.InternalServerError; // 500
                    break;
                default:
                    status = HttpStatusCode.InternalServerError; // 500
                    break;
            }

            var response = new { Message = message };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
