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
    }
}
