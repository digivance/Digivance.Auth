/**
 * Command to update an existing role
 */
export default interface UpdateRole {
    /**
     * User friendly description of the role to Update
     */
    description ?: string;

    /**
     * Unique id of the role to update
     */
    id: string; // Actually a Guid

    /**
     * Required unique per scope name of this role
     */
    name: string;

    /**
     * Optional collection of unique id's of permissions to assign to this role
     */
    permissionIds?: string[]; // Actually Guids
};
