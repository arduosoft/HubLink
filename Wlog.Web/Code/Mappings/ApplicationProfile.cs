using AutoMapper;
using PagedList;
using System.Web.Mvc;
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
            CreateMap<IPagedList<ApplicationEntity>, IPagedList<ApplicationModel>>()
               .ConvertUsing<PagedListConverter<ApplicationEntity, ApplicationModel>>();

            CreateMap<ProfilesEntity, SelectListItem>()
                .ForMember(dest => dest.Value, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opts => opts.MapFrom(src => src.ProfileName));
            CreateMap<SelectListItem, ProfilesEntity>()
               .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Value))
               .ForMember(dest => dest.ProfileName, opts => opts.MapFrom(src => src.Text));
        }
    }
}