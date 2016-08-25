﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wlog.Web.Models;
using Wlog.Web.Code.Helpers;
using System.Web.Security;
using PagedList;
using Wlog.Web.Models.User;
using Wlog.Web.Code.Authentication;
using Wlog.Web.Models.Application;
using Wlog.BLL.Entities;
using Wlog.Library.BLL.Reporitories;
using Wlog.BLL.Classes;
using Wlog.Library.BLL.Classes;
using Wlog.Library.BLL.Enums;
using Wlog.Library.BLL.Helpers;
using Wlog.Library.BLL.Configuration;

namespace Wlog.Web.Controllers
{
    public class PrivateController : Controller
    {
        public ActionResult Index()
        {
            string username = Membership.GetUser().UserName;
            List<Guid> apps = UserHelper.GetAppsIdsForUser(username);
            LogsSearchSettings logSearch = new LogsSearchSettings()
            {
                Applications = apps,
                PageNumber = 1,
                PageSize = 10

            };
            IPagedList<LogEntity> lastestLog = RepositoryContext.Current.Logs.SeachLog(logSearch);



            DashboardModel dm = new DashboardModel();
            dm.ErrorCount = RepositoryContext.Current.Logs.CountByLevel(StandardLogLevels.ERROR );
            dm.InfoCount = RepositoryContext.Current.Logs.CountByLevel(StandardLogLevels.ERROR);
            dm.LogCount = RepositoryContext.Current.Logs.CountByLevel(StandardLogLevels.ALL_LEVELS);
            dm.WarnCount = RepositoryContext.Current.Logs.CountByLevel(StandardLogLevels.WARNING);



            dm.LastTen = ConversionHelper.ConvertLogEntityToMessage(lastestLog.ToList());
                dm.QueueLoad = LogQueue.Current.QueueLoad;
                dm.AppLastTen = new List<MessagesListModel>();

            IPagedList<LogEntity> logOfCurrentApp;
                foreach (ApplicationEntity app in UserHelper.GetAppsForUser(username))
                {

                        logSearch = new LogsSearchSettings()
                        {
                            PageNumber = 1,
                            PageSize = 10
                           
                        };
                     logSearch.Applications.Add(app.IdApplication);
                     logOfCurrentApp = RepositoryContext.Current.Logs.SeachLog(logSearch);
                    MessagesListModel list = new MessagesListModel();
                    list.ApplicationName = app.ApplicationName;
                    list.IdApplication = app.IdApplication;

                    list.Messages= ConversionHelper.ConvertLogEntityToMessage(logOfCurrentApp.ToList());
                    dm.AppLastTen.Add(list);

                }

                return View(dm);
            
        }


        public ActionResult Logs(Guid? applicationId,string level, string sortOrder, string sortBy, string serchMessage, int? page, int? pageSize)
        {
            LogListModel mm = new LogListModel()
            {
                ApplicationId =applicationId,
                Level = level,
                SortOrder = sortOrder,
                SortBy = sortBy,
                SerchMessage = serchMessage,               
                
            };
            MembershipUser current = Membership.GetUser();
            mm.Apps = UserHelper.GetAppsForUser(current.UserName);
            mm.Apps.Insert(0, new ApplicationEntity() {                
                ApplicationName="All application"
            });


            mm.Items = LogHelper.GetLogs(mm.ApplicationId, mm.SortOrder, mm.SortBy, mm.SerchMessage, 30, page??1);
           
            return View(mm);
        }

        public ActionResult LogDetail(Guid id)
        {
            var log = LogHelper.GetById(id);
            return View(log);
        }

        // Get  /Private/ListUsers
        // TODO: set role [Authorize(Roles="ADMIN")]
        public ActionResult ListUsers(string serchMessage, int? page, int? pageSize)
        {
            ListUser model = new ListUser
            {
                SerchMessage = serchMessage
            };


            model.UserList = UserHelper.FilterUserList(serchMessage, page ?? 1, pageSize ?? 30);

            return View(model);
        }

        //Get Private/EditUser/1
        [HttpGet]
        public ActionResult EditUser(Guid Id)
        {
            UserEntity user = UserHelper.GetById(Id);
            ViewBag.Title = user.Username;
            EditUser model = new EditUser();
            model.DataUser = user;
            model.Apps = UserHelper.GetApp(Id);
            return View(model);
        }


        //Post Private/EditUser/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(EditUser model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserHelper.UpdateUser(model.DataUser);

                 
                    List<AppUserRoleEntity> newRoleList = new List<AppUserRoleEntity>();
                    foreach (UserApps app in model.Apps)
                    {
                           
                        if (app.RoleId != Guid.Empty)
                        {
                                newRoleList.Add(new AppUserRoleEntity { UserId = model.DataUser.Id, ApplicationId = app.IdApplication, RoleId = app.RoleId });
                        }
                    }

                    RepositoryContext.Current.Applications.ResetUserRoles(model.DataUser,newRoleList);
                       

                    return RedirectToAction("ListUsers");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Error");
                }
            }
            ModelState.AddModelError("", "Error");
            return View(model);
        }

        //Get Private/NewUser
        [HttpGet]
        public ActionResult NewUser()
        {
            return View(new NewUser());
        }

        //Post Private/NewUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewUser(NewUser user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MembershipCreateStatus status;
                    WLogMembershipProvider provider = new WLogMembershipProvider();
                    provider.CreateUser(user.UserName, user.Password, user.Email, null, null, true, null, out status);
                    if (status == MembershipCreateStatus.Success)
                    {
                        UserEntity entity = UserHelper.GetByUsername(user.UserName);
                        entity.IsAdmin = user.IsAdmin;
                        UserHelper.UpdateUser(entity);
                        return RedirectToAction("EditUser", "Private", new { Id = entity.Id });
                    }
                    else
                    {
                        ModelState.AddModelError("", ErrorCodeToString(status));
                    }

                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // Se si arriva a questo punto, significa che si è verificato un errore, rivisualizzare il form
            return View(user);
        }

        //Get Private/DeleteUser/1
        [HttpGet]
        public ActionResult DeleteUser(Guid Id)
        {

            UserEntity User = UserHelper.GetById(Id);
            UserData result = new UserData
            {
                Id = User.Id,
                Email = User.Email,
                Username = User.Username,
                CreationDate = User.CreationDate,
                IsAdmin = User.IsAdmin,
                IsOnLine = User.IsOnLine,
                LastLoginDate = User.LastLoginDate
            };
            return View(result);
        }

        //Post Private/DeleteUser/1
        [HttpPost]
        public ActionResult DeleteUser(UserData User)
        {
            if (UserHelper.DeleteById(User.Id))
            {
                return RedirectToAction("ListUsers");
            }
            else
            {
                ModelState.AddModelError("", "Si è Verificato un Errore.");
            }
            return View(User);
        }


        #region Application
        // Get  /Private/ListApps
        public ActionResult ListApps(string serchMessage, int? page, int? pageSize)
        {
            ApplicationList model = new ApplicationList
            {
                SerchMessage = serchMessage
            };
            model.AppList = ApplicationHelper.FilterApplicationList(serchMessage, page ?? 1, pageSize ?? 30);
            return View(model);
        }

        //Get Private/NewApp
        [HttpGet]
        public ActionResult NewApp()
        {
      
            return View(new ApplicationModel());
        }

        //Post Private/NewApp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewApp(ApplicationModel model)
        {
            if (ModelState.IsValid)
            {
                
                  
                    ApplicationEntity entity = new ApplicationEntity();
                    entity.ApplicationName = model.ApplicationName;
                    entity.IsActive = true;
                    entity.StartDate = model.StartDate;
                    entity.PublicKey = model.PublicKey;
                    entity.PublicKey = Guid.NewGuid();
                RepositoryContext.Current.Applications.Save(entity);

                return RedirectToAction("ListApps");
            }
            else
            {
                ModelState.AddModelError("", "error");
            }

            return View(model);
        }


        //Get Private/EditApp/1
        [HttpGet]
        public ActionResult EditApp(Guid Id)
        {
            return View(ApplicationHelper.GetById(Id));
        }


        //Post Private/EditApp/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditApp(ApplicationModel model)
        {
            if (ModelState.IsValid)
            {



                ApplicationEntity entity = RepositoryContext.Current.Applications.GetById(model.IdApplication);
                    entity.ApplicationName=model.ApplicationName;
                    entity.IsActive=model.IsActive;
                    entity.StartDate=model.StartDate;
                    entity.EndDate=model.EndDate;
                //entity.PublicKey=model.PublicKey; Do not change this is not editable
                RepositoryContext.Current.Applications.Save(entity);
                return RedirectToAction("ListApps");
            }
            else
            {
                ModelState.AddModelError("", "Error");
            }
            return View(model);
        }

        //Get Private/DeleteApp/1
        [HttpGet]
        public ActionResult DeleteApp(Guid Id)
        {
            return View(ApplicationHelper.GetById(Id));
        }

        //Post Private/DeleteApp/1
        [HttpPost]
        public ActionResult DeleteApp(ApplicationModel model)
        {
            if (ApplicationHelper.DeleteById(model.IdApplication))
            {
                return RedirectToAction("ListApps");
            }
            else
            {
                ModelState.AddModelError("", "Si è Verificato un Errore.");
            }
            return View(model);
        }

#endregion

        #region HElper
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // Vedere http://go.microsoft.com/fwlink/?LinkID=177550 per
            // un elenco completo di codici di stato.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Il nome utente esiste già. Immettere un nome utente differente.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Un nome utente per l'indirizzo di posta elettronica esiste già. Immettere un nome utente differente.";

                case MembershipCreateStatus.InvalidPassword:
                    return "La password fornita non è valida. Immettere un valore valido per la password.";

                case MembershipCreateStatus.InvalidEmail:
                    return "L'indirizzo di posta elettronica fornito non è valido. Controllare il valore e riprovare.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "La risposa fornita per il recupero della password non è valida. Controllare il valore e riprovare.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "La domanda fornita per il recupero della password non è valida. Controllare il valore e riprovare.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Il nome utente fornito non è valido. Controllare il valore e riprovare.";

                case MembershipCreateStatus.ProviderError:
                    return "Il provider di autenticazione ha restituito un errore. Verificare l'immissione e riprovare. Se il problema persiste, contattare l'amministratore di sistema.";

                case MembershipCreateStatus.UserRejected:
                    return "La richiesta di creazione dell'utente è stata annullata. Verificare l'immissione e riprovare. Se il problema persiste, contattare l'amministratore di sistema.";

                default:
                    return "Si è verificato un errore sconosciuto. Verificare l'immissione e riprovare. Se il problema persiste, contattare l'amministratore di sistema.";
            }
        }
        #endregion


        #region Info

        //Get Private/info
    
        public ActionResult Info()
        {
            var model = InfoHelper.GetInfoPage(InfoPageConfigurator.Configuration);
            return View(model);
        }
        #endregion
    }
}
