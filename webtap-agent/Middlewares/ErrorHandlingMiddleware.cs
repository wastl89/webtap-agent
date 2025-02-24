using System.Xml;
using webtap_agent.Exceptions;
using OpenTap;

namespace webtap_agent.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try {
            await next(context);
        }
        catch (NotFoundException ex) {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(ex.Message);
            logger.LogWarning(ex, "Not found");
        }
        catch(TestPlan.PlanLoadException ex) {
            context.Response.StatusCode = StatusCodes.Status424FailedDependency;
            await context.Response.WriteAsJsonAsync(ex.Message.Split('\n'));
            logger.LogWarning(ex, "Plan load error");
        }
        catch (Exception ex) {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(ex.Message);
            logger.LogError(ex, "Internal server error");
        }
    }
}