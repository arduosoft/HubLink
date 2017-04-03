namespace Wlog.Library.DAL.Nhibernate.Mappings
{
    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;
    using Wlog.BLL.Entities;

    public class JobInstanceMap : ClassMapping<JobInstanceEntity>
    {
        public JobInstanceMap()
        {
            Table("wl_jobinstance");
            //Schema("dbo");

            Id(x => x.Id, map => { map.Generator(Generators.Guid); });
            Property(x => x.Active);
            Property(x => x.CronExpression);
            Property(x => x.JobDefinitionID);
            Property(x => x.ActivationDate);
            Property(x => x.DeactivationDate);
        }
    }
}
