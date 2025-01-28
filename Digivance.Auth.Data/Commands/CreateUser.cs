using Digivance.Auth.Data.Services;
using FluentValidation;
using System.Net.Mail;
using System.Security;

namespace Digivance.Auth.Data.Commands
{
    public record CreateUser
    {
        /// <summary>
        /// Optional display name that the user can choose to identify themselves as
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Required unique email address of this user account
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Clear text password to set
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Unique id of the tenant that this user account exists in
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// Optional, unique per tenant if provided, custom username of this user account
        /// </summary>
        public string Username { get; set; }
    }

    /// <summary>
    /// CreateUser command validation
    /// </summary>
    public class CreateUserValidator : AbstractValidator<CreateUser>
    {

        /// <summary>
        /// Standard constructor
        /// </summary>
        public CreateUserValidator()
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("A valid email address is required")
                .Must(IsValidFormat).WithMessage("Please provide a valid email address");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Insecure password, please enter at least 8 characters")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"\d").WithMessage("Password must contain at least one number")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character");
        }

        /// <summary>
        /// Ensures that the provided email address looks to be real
        /// </summary>
        /// <param name="emailAddress">Email address to test</param>
        /// <returns>True if this email address seems valid</returns>
        public bool IsValidFormat(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return false;
            }

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
