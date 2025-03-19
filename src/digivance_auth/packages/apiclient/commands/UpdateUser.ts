/**
 * Command to update a user account
 */
export default interface UpdateUser {
    /**
     * Optional display name that the user can choose to identify themselves as
     */
    displayName?: string;

    /**
     * Unique id of the user account we want to update
     */
    id: string; // Actually a Guid

    /**
     * Optional, unique per tenant if provided, custom username of this user account
     */
    username: string;
};
