using ElectronicMedicalCert.Middleware;
using ElectronicMedicalCert.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;

namespace ElectronicMedicalCert.Filters;

public sealed class ModelStateValidationFilter(IStringLocalizer<SharedResource> localizer) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var correlationId = context.HttpContext.Items.TryGetValue(CorrelationIdMiddleware.ItemKey, out var cid)
                ? cid?.ToString()
                : null;

            var errors = context.ModelState
                .Where(kvp => kvp.Value?.Errors?.Count > 0)
                .SelectMany(kvp => kvp.Value!.Errors.Select(e => new ApiProblemFieldError
                {
                    Field = kvp.Key,
                    Message = string.IsNullOrWhiteSpace(e.ErrorMessage) ? "Invalid value." : e.ErrorMessage
                }))
                .ToList();

            var payload = new ApiProblemDetails
            {
                Type = "https://api.elp.cz/errors/validation",
                Title = localizer["Problem.Validation.Title"],
                Status = StatusCodes.Status400BadRequest,
                Detail = localizer["Problem.Validation.Detail"],
                Instance = context.HttpContext.Request.Path,
                CorrelationId = correlationId,
                Errors = errors,
            };

            context.Result = new ObjectResult(payload)
            {
                StatusCode = StatusCodes.Status400BadRequest,
                DeclaredType = typeof(ApiProblemDetails),
                ContentTypes = { "application/problem+json" },
            };
            return;
        }

        await next();
    }
}

