using Digivance.Auth.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Unique id of the tenant that this user account exists in
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// Optional, unique per tenant if provided, custom username of this user account
        /// </summary>
        public string? Username { get; set; }
    }
}
