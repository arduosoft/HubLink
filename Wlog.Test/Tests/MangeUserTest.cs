using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Web.Mvc;
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
        public void CreateUser()
        {
            NewUser user = new NewUser();
            user.Email = "test123123@test.com";
            user.Password = "test";

            PrivateController controller = new PrivateController();
            ActionResult result = controller.NewUser(user);
        }
    }
}
