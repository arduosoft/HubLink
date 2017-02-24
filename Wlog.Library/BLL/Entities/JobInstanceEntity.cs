namespace Wlog.BLL.Entities
{
    using System;
    using Library.BLL.Interfaces;

    public class JobInstanceEntity : IEntityBase
    {
        public virtual Guid JobDefinitionID { get; set; }

        public virtual bool Active{ get; set; }

        public virtual string CronExpression { get; set; }

        public virtual DateTime? ActivationDate { get; set; }

        public virtual DateTime? DeactivationDate { get; set; }
    }
}
