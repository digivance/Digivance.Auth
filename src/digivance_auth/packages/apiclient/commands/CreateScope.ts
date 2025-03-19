/**
 * Command to create a new scope
 */
export default interface CreateScope {
    /**
     * Optional user friendly description of this scope
     */
    description?: string;

    /**
     * Required unique per tenant name of this scope
     */
    name: string;

    /**
     * Unique id of the tenant this scope exists in
     */
    tenantId?: string; // Actually a Guid
};
