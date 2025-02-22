using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digivance.Auth.Data.Commands
{
    public record UpdateScope
    {
        /// <summary>
        /// Optional user friendly description of this scope
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Required unique per tenant name of this scope
        /// </summary>
        public string Name { get; set; }
    }
}
