using System.Net;
using System.Text.Json;
using FluentValidation;
using transactions.portal.services.Exceptions;

namespace transactions.portal.services.Middleware;

public class ErrorResponse
{
    public int Status { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public IDictionary<string, string[]>? Errors { get; set; }
}

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate _next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        this._next = _next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ErrorResponse();

        switch (exception)
        {
            case ValidationException valEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Status = (int)HttpStatusCode.BadRequest;
                errorResponse.Title = "Validation Failed";
                errorResponse.Message = "One or more validation errors occurred.";
                errorResponse.Errors = valEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
                break;

            case BusinessRuleException busEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Status = (int)HttpStatusCode.BadRequest;
                errorResponse.Title = "Business Rule Violation";
                errorResponse.Message = busEx.Message;
                break;

            case NotFoundException notFoundEx:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Status = (int)HttpStatusCode.NotFound;
                errorResponse.Title = "Not Found";
                errorResponse.Message = notFoundEx.Message;
                break;

            case KeyNotFoundException keyEx:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Status = (int)HttpStatusCode.NotFound;
                errorResponse.Title = "Not Found";
                errorResponse.Message = keyEx.Message;
                break;

            default:
                _logger.LogError(exception, "Unhandled exception occurred");
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Status = (int)HttpStatusCode.InternalServerError;
                errorResponse.Title = "Internal Server Error";
                errorResponse.Message = "An unexpected error occurred.";
                break;
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var result = JsonSerializer.Serialize(errorResponse, jsonOptions);
        await response.WriteAsync(result);
    }
}

