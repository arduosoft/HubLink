namespace Wlog.Library.DAL.Nhibernate.Mappings
{
    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;
    using Wlog.BLL.Entities;

    public  class JobDefinitionMap : ClassMapping<JobDefinitionEntity>
    {
        public JobDefinitionMap()
        {
            Table("WL_JobDefinition");
            Schema("dbo");
            Id(x => x.Id, map => { map.Generator(Generators.Guid); });
            Property(x => x.Name);
            Property(x => x.System);
            Property(x => x.Description);
            Property(x => x.FullClassname);
            Property(x => x.Instantiable);
        }
    }
}
