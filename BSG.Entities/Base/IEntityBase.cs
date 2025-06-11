namespace BSG.Entities.Base;

public interface IEntityBase: IConfigurableEntity
{
    long Id { get; set; }
    long? CreatedById { get; set; }
    DateTime CreatedOn { get; set; }
    long? ModifiedById { get; set; }
    DateTime? ModifiedOn { get; set; }
}