namespace Digivance.Auth.Data.Commands
{
    /// <summary>
    /// Command to create a new scope
    /// </summary>
    public record CreateScope
    {
        /// <summary>
        /// Optional user friendly description of this scope
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Required unique per tenant name of this scope
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Required unique id of the tenant this scope exists in
        /// </summary>
        public Guid TenantId { get; set; }
    }
}
