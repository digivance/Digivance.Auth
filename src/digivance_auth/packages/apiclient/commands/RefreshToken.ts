/**
 * Simple command so clients can send a bearer token refresh code
 */
export default interface RefreshToken {
    /**
     * The code to refresh an expired bearer token with
     */
    code: string;
}