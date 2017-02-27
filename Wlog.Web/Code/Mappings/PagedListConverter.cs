using AutoMapper;
using PagedList;
using System.Linq;

namespace Wlog.Web.Code.Mappings
{
    public class PagedListConverter<TSource, TDestination> : ITypeConverter<IPagedList<TSource>, IPagedList<TDestination>>
    {
        public IPagedList<TDestination> Convert(IPagedList<TSource> source, IPagedList<TDestination> destination, ResolutionContext context)
        {
            var models = source.Select(p => Mapper.Map<TSource, TDestination>(p)).ToList();
            return new PagedList<TDestination>(models, source.PageNumber, source.PageSize);
        }
    }
}