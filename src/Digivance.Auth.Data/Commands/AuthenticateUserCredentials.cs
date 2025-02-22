using FluentValidation;
using System.Net.Mail;

namespace Digivance.Auth.Data.Commands
{
    /// <summary>
    /// Command to authenticate a user account via user credentials
    /// </summary>
    public record AuthenticateUserCredentials
    {
        /// <summary>
        /// Email address of the account to authenticate with
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Clear text password to authenticate with
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// Fluent validations for our AuthenticateUserCredentials command
    /// </summary>
    public class AuthenticateUserCredentialsValidator : AbstractValidator<AuthenticateUserCredentials>
    {
        public const string ERR_EMAIL_INVALID = "Please provide a valid email address";
        public const string ERR_EMAIL_REQUIRED = "A valid email address is required";

        /// <summary>
        /// Standard constructor
        /// </summary>
        public AuthenticateUserCredentialsValidator()
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                    .WithMessage(ERR_EMAIL_REQUIRED)
                .Must(BeValidEmailAddress)
                    .WithMessage(ERR_EMAIL_INVALID);
        }

        /// <summary>
        /// Ensures that the provided email address looks to be real
        /// </summary>
        /// <param name="emailAddress">Email address to test</param>
        /// <returns>True if this email address seems valid</returns>
        public static bool BeValidEmailAddress(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
                return false;

            try
            {
                var mail = new MailAddress(emailAddress);
                return mail.Address.Equals(emailAddress, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}
