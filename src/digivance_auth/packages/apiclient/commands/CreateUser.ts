/**
 * Command to create a new user account
 */
export default interface CreateUser {
    /**
     * Optional display name that the user can choose to identify themselves as
     */
    displayName?: string;

    /**
     * Required unique email address of this user account
     */
    emailAddress: string;

    /**
     * Clear text password to set
     */
    password: string;

    /**
     * Unique id of the tenant that this user account exists in (can be null for default)
     */
    tenantId?: string; // Actually a Guid

    /**
     * Optional, unique per tenant if provided, custom username of this user account
     */
    username?: string;
}