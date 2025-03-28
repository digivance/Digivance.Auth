import { PermissionModel } from "./PermissionModel";
import RoleModel from "./RoleModel";
import ScopeModel from "./ScopeModel";
import UserModel from "./UserModel";
import BaseModel from "./BaseModel";

/**
 * A tenant is a collection of auth related entities such as scopes, user accounts,
 * roles, permissions, oauth applications and more
 */
export default interface TenantModel extends BaseModel{
    /**
     * User friendly description of this tenant
     */
    description?: string;

    /**
     * User friendly name of this tenant
     */
    name: string;

    /**
     * The permissions that apply within this tenant
     */
    permissions?: PermissionModel[];

    /**
     * The roles that exist within this tenant
     */
    roles?: RoleModel[];

    /**
     * The scopes that exist within this tenant
     */
    scopes?: ScopeModel[];

    /**
     * The user accounts associated with this tenant
     */
    userAccounts?: UserModel[];
}