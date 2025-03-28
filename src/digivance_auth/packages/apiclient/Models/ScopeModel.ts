import { PermissionModel } from "./PermissionModel";
import RoleModel from "./RoleModel";
import TenantModel from "./TenantModel";

export default interface ScopeModel {
    /**
     * User friendly description of this scope
     */
    description?: string;

    /**
     * User friendly name of this scope
     */
    name: string;

    /**
     * The permissions that apply to this scope
     */
    permissions?: PermissionModel[];

    /**
     * The roles that apply to this scope
     */
    roles?: RoleModel[];

    /**
     * Unique id of the Tenant this scope belongs to
     */
    tenantId: string;

    /**
     * The tenant that this scope belongs to
     */
    tenant?: TenantModel;
}