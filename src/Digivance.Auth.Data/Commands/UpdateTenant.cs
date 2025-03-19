namespace Digivance.Auth.Data.Commands
{
    public record UpdateTenant
    {
        /// <summary>
        /// User friendly description of this tenant
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Unique id of the tenant to update
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User friendly name of this tenant
        /// </summary>
        public string Name { get; set; }
    }
}
