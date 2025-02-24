using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digivance.Auth.Data.Commands
{
    public class UpdateRole
    {
        /// <summary>
        /// User friendly description of the role to create
        /// </summary>
        public string Description { get; set; }

        public Guid Id { get; set; }

        /// <summary>
        /// Required unique per scope name of this role
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Optional collection of unique id's of permissions to assign to this role
        /// </summary>
        public ICollection<Guid>? PermissionIds { get; set; }
    }
}
