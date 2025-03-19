/**
 * Command to update an existing Scope
 */
export default interface UpdateScope {
    /**
     * Optional user friendly description of this scope
     */
    description?: string;

    /**
     * Unique id of the scope to update
     */
    id: string; // Actually a Guid

    /**
     * Required unique per tenant name of this scope
     */
    name: string;
};
