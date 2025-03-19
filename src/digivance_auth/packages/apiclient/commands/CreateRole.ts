/**
 * Command to create a new role
 */
export default interface CreateRole {
    /**
     * User friendly description of the role to create
     */
    description?: string;

    /**
     * Required unique per scope name of this role
     */
    name: string;

    /**
     * Required, the unique if of the scope to create this role in
     */
    scopeId: string; // Actually a Guid

    /**
     * Unique id of the tenant this role exists in
     */
    tenantId?: string; // Actually a Guid
}
