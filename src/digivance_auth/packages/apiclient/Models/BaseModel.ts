export default interface BaseModel {
    /**
     * Unique id of the user that created this entity
     */
    createdBy?: string;

    /**
     * UTC DateTime when this entity was created
     */
    createdOn: Date;

    /**
     * Unique id of this entity
     */
    id: string;

    /**
     * Unique id of the user that last modified this entity
     */
    modifiedBy?: string;

    /**
     * UTC DateTime when this model was last modified
     */
    modifiedOn?: Date;
}

