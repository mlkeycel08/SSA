namespace SSA.Data.Core
{
    public interface IDeletableEntity
    {
        bool IsDeleted { get; set; }
    }
}