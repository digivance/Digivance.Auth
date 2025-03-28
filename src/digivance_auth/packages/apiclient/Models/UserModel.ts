import { PermissionModel } from "./PermissionModel";
import RoleModel from "./RoleModel";
import TenantModel from "./TenantModel";

export default interface UserModel {
    /**
     * Optional display name that the user can choose to identify themselves as
     */
    displayName?: string;

    /**
     * Required unique email address of this user account
     */
    emailAddress: string;

    /**
     * UTC Date and Time when this user verified this email address
     */
    emailVerifiedOn?: Date;

    /**
     * Identifies if this user account has verified their email address
     */
    isEmailVerified: boolean;

    /**
     * Permissions this user has (including those granted via roles)
     */
    permissions: PermissionModel[];

    /**
     * Roles this user is assigned to
     */
    roles: RoleModel[];

    /**
     * The tenant that this user account exists in
     */
    tenant?: TenantModel;

    /**
     * Unique id of the tenant that this user account exists in
     */
    tenantId?: string;

    /**
     * Optional, unique per tenant if provided, custom username of this user account
     */
    username?: string;
}