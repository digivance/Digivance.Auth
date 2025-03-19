/**
 * Command to authenticate a user account via user credentials
 */
export default interface AuthenticateUserCredentials {
    /**
     * Email address of the account to authenticate with
     */
    emailAddress: string;

    /**
     * Clear text password to authenticate with
     */
    password: string;
};
