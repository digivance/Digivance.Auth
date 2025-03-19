/**
 * Command to create a new tenant
 */
export default interface CreateTenant {
    /**
     * User friendly description of this tenant
     */
    description?: string;

    /**
     * User friendly name of this tenant
     */
    name: string;
};
