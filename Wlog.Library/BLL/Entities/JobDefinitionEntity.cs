
namespace Wlog.BLL.Entities
{
    using Library.BLL.Interfaces;

    public class JobDefinitionEntity : IEntityBase
    {
        /// <summary>
        /// Name of the job
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Description of the job
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Name of class with namespace and assembly, needed to create an instance of the job
        /// </summary>
        public virtual string FullClassname { get; set; }

        /// <summary>
        /// Inform if we can add many instances of same kind
        /// </summary>
        public virtual bool Instantiable { get; set; }

        /// <summary>
        /// Inform if this job come with base bundle or it is custom
        /// </summary>
        public virtual bool System { get; set; }
    }
}
