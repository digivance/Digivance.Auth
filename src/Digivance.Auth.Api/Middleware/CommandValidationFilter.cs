using FluentValidation;
using Serilog;
using System.Reflection;
using System.Text.Json;

namespace Digivance.Auth.Api.Middleware
{
    /// <summary>
    /// Custom filter that performs validation via FluentValidation validators
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandValidationFilter<T> : IEndpointFilter
        where T : class
    {
        /// <summary>
        /// The validator that will be used to validate the model
        /// </summary>
        private readonly IValidator<T> validator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="validator">Validator that will be used by the filter to validate incoming requests</param>
        public CommandValidationFilter(IValidator<T> validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// Called from the ASP pipeline before entering an endpoint with a validatable command.
        /// </summary>
        /// <param name="context">Filter context</param>
        /// <param name="next">Next handler in the pipeline</param>
        /// <returns>Calls the next delegate if the validator succeeds</returns>
        /// <exception cref="ValidationException">Thrown if validation fails (ExceptionFilter should catch and convert to 400)</exception>
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var command = context.GetArgument<T>(0);
            var result = await validator.ValidateAsync(command, context.HttpContext.RequestAborted);

            Log.Verbose("Request {RequestID} validation filter result: {validationResult}", context.HttpContext.TraceIdentifier, result.IsValid);

            if (!result.IsValid)
            {
                var commandName = typeof(T).Name;
                Log.Debug($"Invalid {commandName} request: {JsonSerializer.Serialize(result.Errors)}");
                throw new ValidationException($"Invalid {commandName} request", result.Errors);
            }

            return await next(context);
        }
    }

    /// <summary>
    /// Extensions for validation system
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Registers an endpoint filter that will validate <typeparam name="T"></typeparam>
        /// on the incoming request.  If validation fails, we will throw an exception and
        /// expect the ExceptionFilter to return problem details 400 - Bad Request. Otherwise, 
        /// control will be passed to next handler
        /// </summary>
        /// <param name="builder">The route builder</param>
        /// <typeparam name="T">The model type to validate</typeparam>
        /// <returns>The route builder again</returns>
        public static RouteHandlerBuilder Validate<T>(this RouteHandlerBuilder builder)
            where T : class
            => builder.AddEndpointFilter<CommandValidationFilter<T>>();
    }
}
