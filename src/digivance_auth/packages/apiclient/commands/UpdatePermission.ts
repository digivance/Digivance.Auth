/**
 * Command to update a permission record
 */
export default interface UpdatePermission {
    /**
     * Human friendly description of this permission
     */
    description?: string;

    /**
     * Id of the permission to update
     */
    id: string; // Actually a guid
};
