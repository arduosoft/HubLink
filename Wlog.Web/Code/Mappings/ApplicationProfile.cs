using AutoMapper;
using PagedList;
using Wlog.BLL.Classes;
using Wlog.BLL.Entities;
using Wlog.Web.Models;
using Wlog.Web.Models.User;

namespace Wlog.Web.Code.Mappings
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<UserEntity, UserData>();
            CreateMap<UserData, UserEntity>();
            CreateMap<IPagedList<UserEntity>, IPagedList<UserData>>()
                .ConvertUsing<PagedListConverter<UserEntity, UserData>>();

            CreateMap<LogMessage, LogEntity>();
            CreateMap<LogEntity, LogMessage>();

            CreateMap<ApplicationModel, ApplicationEntity>();
            CreateMap<ApplicationEntity, ApplicationModel>();
        }
    }
}