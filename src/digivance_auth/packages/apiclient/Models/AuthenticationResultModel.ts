/**
 * Model representing a successful authentication result
 */
export default interface AuthenticationResultModel {
    /**
     * UTC Date and time when this bearer token will expire
     */
    bearerExpires: string;

    /**
     * The JWT Bearer token for the authenticated user
     */
    bearerToken: string;

    /**
     * Refresh code for getting a new bearer token
     */
    refreshCode: string;

    /**
     * UTC Date and time when this refresh token will expire
     */
    refreshExpires: string;
}