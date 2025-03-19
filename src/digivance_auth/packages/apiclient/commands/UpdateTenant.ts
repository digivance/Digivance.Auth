/**
 * Command to update an existing tenant
 */
export default interface UpdateTenant {
    /**
     * User friendly description of this tenant
     */
    description?: string;

    /**
     * Unique id of the tenant we want to update
     */
    id: string; // Actually a Guid

    /**
     * User friendly name of this tenant
     */
    name: string;
};
