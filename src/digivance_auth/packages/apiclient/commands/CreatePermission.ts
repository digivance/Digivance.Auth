/**
 * Command to create a new permission
 */
export default interface CreatePermission {
    /**
     * Optional user friendly description to create this permission with
     */
    description?: string;

    /**
    * The access this permission allows, such as Create, Read, Update or Delete
    */
    entityAccess: string;

    /**
    * The type of entity this permission applies to (Database or custom entity type name)
    */
    entityType: string;

    /**
     * The unique id of the entity this permission applies to, or null for entities of this
     * type in this scope. (Like global access)
     */
    entityId: string; // Actually a guid...

    /**
     * Optional unique id of the scope this permission exists in (null represents
     * global for this tenant)
     */
    scopeId: string; // Actually a guid
}