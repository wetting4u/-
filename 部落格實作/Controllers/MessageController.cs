using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 部落格實作.Models;
using 部落格實作.Services;
using 部落格實作.ViewModels;

namespace 部落格實作.Controllers
{
    public class MessageController:Controller
    {
        private readonly MessageDBService messageService = new MessageDBService();

        public ActionResult Index(int A_Id = 1)
        {
            ViewData["A_Id"] = A_Id;
            return PartialView();
        }
        public ActionResult MessageList(int A_Id, int Page = 1)
        {
            MessageViewModel Data = new MessageViewModel();
            Data.Paging = new ForPaging(Page);
            Data.A_Id = A_Id;
            Data.DataList = messageService.GetDataList(Data.Paging, Data.A_Id);
            return PartialView(Data);
        }
        [Authorize]
        public ActionResult Create(int A_Id)
        {
            ViewData["A_Id"] = A_Id;
            return PartialView();
        }
        [Authorize]
        [HttpPost]
        //使用Bind的Include來定義只接受的欄位，用來避免傳入其他不相干值
        public ActionResult Add(int A_Id, [Bind(Include ="Content")]Message Data)
        {
            Data.A_Id = A_Id;
            Data.Account = User.Identity.Name;
            messageService.InsertMessage(Data);
            return RedirectToAction("MessageList", new { A_Id = A_Id });
        }
        [Authorize]
        public ActionResult UpdateMessage(int A_Id, int M_Id, string Content)
        {
            Message message = new Message();
            message.A_Id = A_Id;
            message.M_Id = M_Id;
            message.Content = Content;
            messageService.UpdateMessage(message);
            return RedirectToAction("Article", "Article", new { A_Id = A_Id });
        }
        [Authorize]
        public ActionResult DeleteMessage(int A_Id, int M_Id)
        {
            messageService.DeleteMessage(A_Id, M_Id);
            return RedirectToAction("Article", "Article", new { A_Id = A_Id });
        }
    }
}