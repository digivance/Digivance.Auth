namespace Digivance.Auth.Data.Commands
{
    public record CreateTenant
    {
        /// <summary>
        /// User friendly description of this tenant
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// User friendly name of this tenant
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The permissions that apply within this tenant (may not be loaded when
        /// tenant is a child)
        /// </summary>
        public ICollection<Guid>? Permissions { get; set; }

        /// <summary>
        /// The roles that exist within this tenant (may not be loaded when tenant
        /// is a child)
        /// </summary>
        public ICollection<Guid>? Roles { get; set; }

        /// <summary>
        /// The scopes that exist within this tenant (may not be loaded when tenant is 
        /// a child)
        /// </summary>
        public ICollection<Guid>? Scopes { get; set; }
    }
}
