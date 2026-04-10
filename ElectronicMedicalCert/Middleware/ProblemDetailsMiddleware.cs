using System.Net.Mime;
using System.Text.Json;
using ElectronicMedicalCert.Application.Common.Exceptions;
using ElectronicMedicalCert.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ElectronicMedicalCert.Middleware;

public sealed class ProblemDetailsMiddleware(RequestDelegate next, IStringLocalizer<SharedResource> localizer, ILogger<ProblemDetailsMiddleware> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);

            if (context.Response.HasStarted)
            {
                return;
            }

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                await WriteProblem(context, StatusCodes.Status401Unauthorized,
                    type: "https://api.elp.cz/errors/unauthorized",
                    title: localizer["Problem.Unauthorized.Title"],
                    detail: localizer["Problem.Unauthorized.Detail"]);
            }

            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                await WriteProblem(context, StatusCodes.Status403Forbidden,
                    type: "https://api.elp.cz/errors/forbidden",
                    title: localizer["Problem.Forbidden.Title"],
                    detail: localizer["Problem.Forbidden.Detail"]);
            }
        }
        catch (ValidationException ex)
        {
            await WriteProblem(context, StatusCodes.Status400BadRequest,
                type: "https://api.elp.cz/errors/validation",
                title: localizer["Problem.Validation.Title"],
                detail: localizer["Problem.Validation.Detail"],
                errors: ex.Errors.Select(e => new ApiProblemFieldError { Field = e.PropertyName, Message = e.ErrorMessage }).ToList());
        }
        catch (NotFoundException ex)
        {
            await WriteProblem(context, StatusCodes.Status404NotFound,
                type: "https://api.elp.cz/errors/not-found",
                title: localizer["Problem.NotFound.Title"],
                detail: ex.Message);
        }
        catch (ConflictException ex)
        {
            await WriteProblem(context, StatusCodes.Status409Conflict,
                type: "https://api.elp.cz/errors/conflict",
                title: localizer["Problem.Conflict.Title"],
                detail: ex.Message);
        }
        catch (ForbiddenException ex)
        {
            await WriteProblem(context, StatusCodes.Status403Forbidden,
                type: "https://api.elp.cz/errors/forbidden",
                title: localizer["Problem.Forbidden.Title"],
                detail: ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteProblem(context, StatusCodes.Status500InternalServerError,
                type: "https://api.elp.cz/errors/internal",
                title: localizer["Problem.Internal.Title"],
                detail: localizer["Problem.Internal.Detail"]);
        }
    }

    private async Task WriteProblem(
        HttpContext context,
        int status,
        string type,
        string title,
        string detail,
        IReadOnlyList<ApiProblemFieldError>? errors = null)
    {
        var correlationId = context.Items.TryGetValue(CorrelationIdMiddleware.ItemKey, out var cid)
            ? cid?.ToString()
            : null;

        context.Response.Clear();
        context.Response.StatusCode = status;
        context.Response.ContentType = MediaTypeNames.Application.ProblemJson;
        if (!string.IsNullOrWhiteSpace(correlationId))
        {
            context.Response.Headers[CorrelationIdMiddleware.HeaderName] = correlationId;
        }

        var problem = new ApiProblemDetails
        {
            Type = type,
            Title = title,
            Status = status,
            Detail = detail,
            Instance = context.Request.Path,
            CorrelationId = correlationId,
            Errors = errors,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem, JsonOptions));
    }
}

public sealed class SharedResource { }

