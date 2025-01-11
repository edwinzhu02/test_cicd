using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


public class LogActionFilter : IActionFilter
{
    private readonly ILogger<LogActionFilter> _logger;

    public LogActionFilter(ILogger<LogActionFilter> logger)
    {
        _logger = logger;
    }

    // 在 Action 执行前记录日志
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var controllerName = context.ActionDescriptor.RouteValues["controller"];
        var actionName = context.ActionDescriptor.RouteValues["action"];
        var method = context.HttpContext.Request.Method;

        // 获取请求路径
        var path = context.HttpContext.Request.Path;

        _logger.LogInformation("Executing action {Controller}/{Action} with HTTP method {Method} and path {Path}",
            controllerName, actionName, method, path);
    }

    // 在 Action 执行后记录日志
    public void OnActionExecuted(ActionExecutedContext context)
    {
        var controllerName = context.ActionDescriptor.RouteValues["controller"];
        var actionName = context.ActionDescriptor.RouteValues["action"];
        var statusCode = context.HttpContext.Response.StatusCode;

        // 获取返回结果
        string responseContent = null;
        if (context.Result is ObjectResult objectResult)
        {
            // 处理返回的是 ObjectResult 的情况
            responseContent = System.Text.Json.JsonSerializer.Serialize(objectResult.Value);
        }
        else if (context.Result is ContentResult contentResult)
        {
            // 处理返回的是 ContentResult 的情况
            responseContent = contentResult.Content;
        }
        else
        {
            // 其他返回类型可以根据需求扩展
            responseContent = "Response content could not be determined.";
        }

        _logger.LogInformation("Executed action {Controller}/{Action} with status code {StatusCode} and response: {ResponseContent}",
            controllerName, actionName, statusCode, responseContent);
    }

}
