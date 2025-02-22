using Digivance.Auth.Data.Services;
using FluentValidation;
using System.Net.Mail;

namespace Digivance.Auth.Data.Commands
{
    public record CreateUser
    {
        /// <summary>
        /// Optional display name that the user can choose to identify themselves as
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Required unique email address of this user account
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Clear text password to set
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Unique id of the tenant that this user account exists in (can be null for default)
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Optional, unique per tenant if provided, custom username of this user account
        /// </summary>
        public string? Username { get; set; }
    }

    /// <summary>
    /// Fluent validations for our CreateUser command
    /// </summary>
    public class CreateUserValidator : AbstractValidator<CreateUser>
    {
        public const string ERR_DISPLAYNAME_LENGTH = "Display name must be 255 characters or less";

        public const string ERR_EMAIL_INVALID = "Please provide a valid email address";
        public const string ERR_EMAIL_REQUIRED = "A valid email address is required";
        public const string ERR_EMAIL_UNIQUE = "An account already exists for this email address";

        public const string ERR_PASSWORD_LOWERCASE = "Password must contain at least one lowercase letter";
        public const string ERR_PASSWORD_LENGTH = "Insecure password, please enter at least 8 characters";
        public const string ERR_PASSWORD_NUMBER = "Password must contain at least one number";
        public const string ERR_PASSWORD_REQUIRED = "Password is required";
        public const string ERR_PASSWORD_SPECIAL = "Password must contain at least one special character";
        public const string ERR_PASSWORD_UPPERCASE = "Password must contain at least one uppercase letter";

        public const string ERR_USERNAME_LENGTH = "Username must be 100 characters or less";
        public const string ERR_USERNAME_UNIQUE = "An account already exists for this username";

        private readonly IUserService userService;

        /// <summary>
        /// Standard constructor
        /// </summary>
        /// <param name="userService">The user service to validate with</param>
        public CreateUserValidator(IUserService userService)
        {
            this.userService = userService;

            RuleFor(x => x.DisplayName)
                .MaximumLength(255)
                    .WithMessage(ERR_DISPLAYNAME_LENGTH);

            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                    .WithMessage(ERR_EMAIL_REQUIRED)
                .Must(BeValidEmailAddress)
                    .WithMessage(ERR_EMAIL_INVALID)
                .MustAsync(BeUniqueEmailAddress)
                    .WithMessage(ERR_EMAIL_UNIQUE);

            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage(ERR_PASSWORD_REQUIRED)
                .MinimumLength(8)
                    .WithMessage(ERR_PASSWORD_LENGTH)
                .Matches(@"[A-Z]")
                    .WithMessage(ERR_PASSWORD_UPPERCASE)
                .Matches(@"[a-z]")
                    .WithMessage(ERR_PASSWORD_LOWERCASE)
                .Matches(@"\d")
                    .WithMessage(ERR_PASSWORD_NUMBER)
                .Matches(@"[#?!@$%^&*-]")
                    .WithMessage(ERR_PASSWORD_SPECIAL);

            RuleFor(x => x.Username)
                .MaximumLength(100)
                    .WithMessage(ERR_USERNAME_LENGTH)
                .MustAsync(BeUniqueUsername)
                    .WithMessage(ERR_USERNAME_UNIQUE);
        }

        /// <summary>
        /// Checks to see if this email address is already in use
        /// </summary>
        /// <param name="command">The create user command we are validating</param>
        /// <param name="emailAddress">The email address to check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if this is a unique email address</returns>
        public async Task<bool> BeUniqueEmailAddress(CreateUser command, string emailAddress, CancellationToken cancellationToken)
            => !await userService.ExistsByEmailAsync(command.TenantId, emailAddress, cancellationToken);

        /// <summary>
        /// Checks to see if this username is already in use
        /// </summary>
        /// <param name="command">The create user command we are validating</param>
        /// <param name="username">The username to check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if this is a unique username</returns>
        public async Task<bool> BeUniqueUsername(CreateUser command, string username, CancellationToken cancellationToken)
            => !await userService.ExistsByUsernameAsync(command.TenantId, username, cancellationToken);

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
