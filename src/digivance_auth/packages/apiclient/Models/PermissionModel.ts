import RoleModel from "./RoleModel";
import ScopeModel from "./ScopeModel";

export interface PermissionModel {
    /**
     * User friendly description of this permission
     */
    description?: string;

    /**
     * This is a string representing the access that this permission allows. Can
     * be one of the EntityAccess system permissions or custom
     */
    entityAccess: string;

    /**
     * This is the unique id of the entity that this permission is granting access to.
     * If null, it is granting this EntityAccess to ALL entities of this type. If null,
     * this permission may be limited to ScopeId.
     */
    entityId?: string; 
    /**
     * The type name of the entity that this permission applies to (such as the name
     * of the model that this permission applies to)
     */
    entityType: string;

    /**
     * Optional list of roles that contain this permission
     */
    roles?: RoleModel[];

    /**
     * The scope that this permission grants access to. Always present but only applicable 
     * when EntityId is null, See Scope
     */
    scope: ScopeModel;

    /**
     * Unique id of the scope that this permission belongs to. Always present but only 
     * applicable when EntityId is null, See Scope
     */
    scopeId: string;
}
