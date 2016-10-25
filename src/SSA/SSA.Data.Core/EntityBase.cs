using System;
using System.ComponentModel.DataAnnotations;

namespace SSA.Data.Core
{
    public class EntityBaseLong : EntityBase<long>
    {

    }

    public class EntityBase : EntityBase<int>
    {
        
    }
    public class EntityBase<TKey> : EntityObject
    {
        [Key]
        public TKey Id { get; set; }
    }

    public class EntityObject
    {
        
    }
}