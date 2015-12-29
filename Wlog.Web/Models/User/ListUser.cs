using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wlog.Web.Models.User
{
    public class ListUser
    {
        public IPagedList<UserData> UserList { get; set; }
        public string SerchMessage { get; set; }
    }
}