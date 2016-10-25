using System;

namespace SSA.Data.Core
{
    public interface IAuditedEntity
    {
        //bool IsDeleted { get; set; }
        string CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }
        string LastModifiedBy { get; set; }
        DateTime? LastModifiedAt { get; set; }
    }
}