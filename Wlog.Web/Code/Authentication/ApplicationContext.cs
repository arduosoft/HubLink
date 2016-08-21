﻿//******************************************************************************
// <copyright file="license.md" company="Wlog project  (https://github.com/arduosoft/wlog)">
// Copyright (c) 2016 Wlog project  (https://github.com/arduosoft/wlog)
// Wlog project is released under LGPL terms, see license.md file.
// </copyright>
// <author>Daniele Fontani, Emanuele Bucaelli</author>
// <autogenerated>true</autogenerated>
//******************************************************************************
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Wlog.BLL.Entities;
using Wlog.Library.BLL.Reporitories;

namespace Wlog.Web.Code.Authentication
{
    public class ApplicationContext
    {
        public ApplicationEntity ApplicationEntity { get; set; }
        public List<RolesEntity> Roles { get; set; }


        public ApplicationContext(Guid idApplication)
        {

            this.ApplicationEntity = RepositoryContext.Current.Applications.GetById(idApplication);
            this.Roles = RepositoryContext.Current.Roles.GetAllApplicationRoles(this.ApplicationEntity);
        }

        public static ApplicationContext Current
        {
            get
            {

                ApplicationContext current = HttpContext.Current.Cache["ApplicationContext" + HttpContext.Current.Session.SessionID] as ApplicationContext;
                if (current != null)
                {
                    return current;
                }

                Guid idSessionApp;
                if (Guid.TryParse(HttpContext.Current.Cache["AppId_" + HttpContext.Current.Session.SessionID] as string, out idSessionApp))
                {
                    current = new ApplicationContext(idSessionApp);
                    HttpContext.Current.Cache["ApplicationContext" + HttpContext.Current.Session.SessionID] = current;
                }
                else
                {
                    throw new Exception("Session App not Set");
                }
                return current;
            }
        }


    }
}