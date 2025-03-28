import BaseModel from "./BaseModel";
import { PermissionModel } from "./PermissionModel";
import ScopeModel from "./ScopeModel";
import TenantModel from "./TenantModel";

export default interface RoleModel extends BaseModel {
    /**
     * User friendly description of this permission
     */
    description: string;

    /**
     * Unique per scope name of this role
     */
    name: string;

    /**
     * Collection of the permissions this role assigns
     */
    permissions?: PermissionModel[];

    /**
     * The scope that this role exists in
     */
    scope: ScopeModel;

    /**
     * Unique id of the scope that this role exists in
     */
    scopeId: string;

    /**
     * The tenant this role belongs to
     */
    tenant?: TenantModel;

    /**
     * Unique id of the tenant this role belongs to
     */
    tenantId?: string;
}