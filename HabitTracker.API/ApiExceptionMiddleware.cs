using FluentValidation;

public sealed class ApiExceptionMiddleware
{
    private readonly RequestDelegate next;
    public ApiExceptionMiddleware(RequestDelegate next) => this.next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            await context.Response.WriteAsJsonAsync(new
            {
                isSuccess = false,
                errorMessage = "Validation failed.",
                errors
            });
        }
    }
}
