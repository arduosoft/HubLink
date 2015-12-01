using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Wlog.Web.Code.Repository;
using Wlog.Web.Models;

namespace Wlog.Web.Controllers
{
    public class LogController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult List(int ApplicationId,string sortOrder,string currentFilter,string SerchMessage,int? page)
        {

            ViewBag.ApplicationId = ApplicationId;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.LevelSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            if (SerchMessage != null)
            {
                page = 1;
            }
            else
            {
                SerchMessage = currentFilter;
            }

            ViewBag.CurrentFilter = SerchMessage;

            ApplicationRepository repo = new ApplicationRepository();
            List<LogModel> model = repo.GetLog(ApplicationId, sortOrder, SerchMessage);

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

    }
}
