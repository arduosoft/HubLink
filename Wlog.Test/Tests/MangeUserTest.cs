using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Web.Mvc;
using Wlog.BLL.Classes;
using Wlog.BLL.Entities;
using Wlog.Library.BLL.Reporitories;
using Wlog.Web.Code.Mappings;
using Wlog.Web.Controllers;
using Wlog.Web.Models.User;
using Xunit;
using Xunit.Abstractions;

namespace Wlog.Test.Tests
{
    public class MangeUserTest
    {

        public MangeUserTest()
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new ApplicationProfile()));
        }


        [Fact]
        public void MangeUser()
        {
            NewUser user = new NewUser();
            user.Email = "unittest@unittest.com";
            user.Password = "test";
            user.UserName = "TestUser";
            user.Profile = RepositoryContext.Current.Profiles.GetProfileByName(Constants.Profiles.StandardUser).Id;
            PrivateController controller = new PrivateController();
            ActionResult result = controller.NewUser(user);

            controller.DeleteUser(RepositoryContext.Current.Users.GetByEmail("unittest@unittest.com").Id);

        }
    }
}
