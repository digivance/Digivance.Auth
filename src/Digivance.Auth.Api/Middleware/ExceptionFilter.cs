using FluentValidation;
using Serilog;

namespace Digivance.Auth.Api.Middleware
{
    /// <summary>
    /// Our custom endpoint filter that is applied to the group handler. This filter will catch
    /// any exceptions thrown by filters or route handlers after it is added. It will log and
    /// convert exceptions to HTTP friendly responses.
    /// </summary>
    public class ExceptionFilter : IMiddleware
    {
        /// <summary>
        /// Called by ASP, this invokes our filter middleware
        /// </summary>
        /// <param name="context">The HttpContext we are catching for</param>
        /// <param name="next">The next handler to execute</param>
        /// <returns>Task that the pipeline will await</returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (BadHttpRequestException err)
            {
                // Warning because not expected...
                Log.Warning($"Bad Request: {err.Message}");

                await Results
                    .Problem(title: err.Message)
                    .ExecuteAsync(context);
            }
            catch (UnauthorizedAccessException err)
            {
                Log.Information(err.Message);

                await Results
                    .Unauthorized()
                    .ExecuteAsync(context);
            }
            catch (ValidationException err)
            {
                Log.Information($"Validation failure: {err.Message}");

                var errors = err.Errors
                    .GroupBy(x => x.PropertyName)
                    .Select(g => new KeyValuePair<string, string[]>(g.Key, g.Select(x => x.ErrorMessage).ToArray()));

                await Results
                    .ValidationProblem(errors, title: err.Message)
                    .ExecuteAsync(context);
            }
            catch (Exception err)
            {
                Log.Error(err.ToString());
                await Results
                    .Problem(title: err.Message)
                    .ExecuteAsync(context);
            }
        }
    }
}
