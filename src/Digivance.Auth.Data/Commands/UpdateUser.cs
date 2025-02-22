using Digivance.Auth.Data.Services;
using FluentValidation;
using System.Net.Mail;

namespace Digivance.Auth.Data.Commands
{
    /// <summary>
    /// Command to update a user account
    /// </summary>
    public record UpdateUser
    {
        /// <summary>
        /// Optional display name that the user can choose to identify themselves as
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Unique id of the user account we want to update
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Optional, unique per tenant if provided, custom username of this user account
        /// </summary>
        public string Username { get; set; }
    }

    /// <summary>
    /// Fluent validations for our UpdateUser command
    /// </summary>
    public class UpdateUserValidator : AbstractValidator<UpdateUser>
    {
        public const string ERR_DISPLAYNAME_LENGTH = "Display name must be 255 characters or less";
        public const string ERR_USERID_NOTFOUND = "No user account found for this user id";
        public const string ERR_USERNAME_LENGTH = "Username must be 100 characters or less";
        public const string ERR_USERNAME_UNIQUE = "An account already exists for this username";

        private readonly IUserService userService;

        /// <summary>
        /// Standard constructor
        /// </summary>
        /// <param name="userService">The user service to validate with</param>
        public UpdateUserValidator(IUserService userService)
        {
            this.userService = userService;

            RuleFor(x => x.DisplayName)
                .MaximumLength(255)
                    .WithMessage(ERR_DISPLAYNAME_LENGTH);

            RuleFor(x => x.Id)
                .MustAsync(BeExistingUser)
                    .WithMessage(ERR_USERID_NOTFOUND);

            RuleFor(x => x.Username)
                .MaximumLength(100)
                    .WithMessage(ERR_USERNAME_LENGTH)
                .MustAsync(BeUniqueUsername)
                    .WithMessage(ERR_USERNAME_UNIQUE);
        }

        /// <summary>
        /// Ensures the provided user id exists
        /// </summary>
        /// <param name="userId">Unique id of the user we are validating this command for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if this user exists</returns>
        public Task<bool> BeExistingUser(Guid userId, CancellationToken cancellationToken)
            => userService.ExistsAsync(userId, cancellationToken);

        /// <summary>
        /// Ensures this is a unique email address per tenant (or is the same)
        /// </summary>
        /// <param name="command">The UpdateUser command we are validating</param>
        /// <param name="emailAddress">The email address to check for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if this is a usable email address</returns>
        public async Task<bool> BeUniqueEmailAddress(UpdateUser command, string emailAddress, CancellationToken cancellationToken)
        {
            // Need to get the user to compute
            var user = await userService.GetByIdAsync(command.Id, cancellationToken);
            if (user == null)
                return false;

            // It's already mine it's ok
            if (user.EmailAddress.Equals(emailAddress, StringComparison.OrdinalIgnoreCase))
                return true;

            return !await userService.ExistsByEmailAsync(user.TenantId, emailAddress, cancellationToken);
        }

        /// <summary>
        /// Ensures this is a unique username per tenant (or is the same)
        /// </summary>
        /// <param name="command">The UpdateUser command we are validating</param>
        /// <param name="username">The username to check for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if this is a usable username</returns>
        public async Task<bool> BeUniqueUsername(UpdateUser command, string username, CancellationToken cancellationToken)
        {
            // Need to get the user to compute
            var user = await userService.GetByIdAsync(command.Id, cancellationToken);
            if (user == null)
                return false;

            // It's already mine it's ok
            if (user.Username != null && user.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                return true;

            return !await userService.ExistsByUsernameAsync(user.TenantId, username, cancellationToken);
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

