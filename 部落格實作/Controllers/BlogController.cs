using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 部落格實作.Services;
using 部落格實作.ViewModels;

namespace 部落格實作.Controllers
{
    public class BlogController:Controller
    {
        private readonly MembersDBService membersService = new MembersDBService();

        public ActionResult Index(string Account)
        {
            BlogViewModel Data = new BlogViewModel();
            Data.Member = membersService.GetDatabyAccount(Account);
            return View(Data);
        }
    }
}