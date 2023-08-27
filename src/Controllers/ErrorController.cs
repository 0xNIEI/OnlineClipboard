using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineClipboard.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace OnlineClipboard.Controllers;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorController : Controller
{
    public IActionResult Status(int code) => HandleStatus(code, false);

    public IActionResult StatusDev(int code) => HandleStatus(code, true);

    private IActionResult HandleStatus(int code, bool isDevEnvironment)
    {
        var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
        string originalUrl = "";
        if (statusCodeReExecuteFeature != null)
        {
            originalUrl = statusCodeReExecuteFeature.OriginalPathBase + statusCodeReExecuteFeature.OriginalPath + statusCodeReExecuteFeature.OriginalQueryString;
        }
        var vm = new ErrorViewModel()
        {
            IsDevelopmentEnvironment = isDevEnvironment,
            RequestId = requestId,
            Path = originalUrl,
            FromException = false
        };

        return code switch
        {
            StatusCodes.Status400BadRequest => View("BadRequest", vm),
            StatusCodes.Status403Forbidden => View("Forbidden", vm),
            StatusCodes.Status404NotFound => View("NotFound", vm),
            StatusCodes.Status401Unauthorized => View("Unauthorized", vm),
            _ => View("Unexpected", vm),
        };
    }

    public IActionResult Exception() => HandleException(false);
    public IActionResult ExceptionDev() => HandleException(true);

    private IActionResult HandleException(bool isDevEnvironment)
    {
        var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        var path = exceptionHandlerPathFeature?.Path;
        var exception = exceptionHandlerPathFeature?.Error;
        var vm = new ErrorViewModel()
        {
            IsDevelopmentEnvironment = isDevEnvironment,
            RequestId = requestId,
            Path = path,
            Exception = exception,
            FromException = true
        };
        return View("Unexpected", vm);
    }
}