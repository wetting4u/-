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
    public class ArticleController : Controller
    {
        private readonly ArticleDBService articleService = new ArticleDBService();
        private readonly MessageDBService messageService = new MessageDBService();

        public ActionResult Index()
        {
            return View();
        }
        //文章列表，將頁數預設為1
        public ActionResult List(string Search, string Account, int Page = 1)
        {
            //宣告一個新頁面模型
            ArticleIndexViewModel Data = new ArticleIndexViewModel();
            //將傳入值Search(搜尋)放入頁面模型中
            Data.Search = Search;
            //新增頁面模型中的分頁
            Data.Paging = new ForPaging(Page);
            //將此文章的擁有者的Account放入頁面模型中
            Data.Account = Account;
            //從Service中取得頁面所需陣列資料
            Data.DataList = articleService.GetDataList(Data.Paging, Data.Search, Data.Account);
            return PartialView(Data);
        }
        public ActionResult Article(int A_Id)
        {
            //新開首頁則取最後一筆
            ArticleViewModel Data = new ArticleViewModel();
            //增加觀看數
            articleService.AddWatch(A_Id);
            Data.article = articleService.GetArticleDataById(A_Id);
            ForPaging paging = new ForPaging(0);//確定是否有留言資料預設0
            Data.DataList = messageService.GetDataList(paging, A_Id);
            return View(Data);
        }
        [Authorize]
        //新增文章一開始載入頁面
        public ActionResult Create()
        {
            return PartialView();
        }
        //新增文章傳入資料時的Action
        [Authorize]
        [HttpPost]
        public ActionResult Create([Bind(Include = "Title,Content")] Article Data)
        {
            Data.Account = User.Identity.Name;
            articleService.InsertArticle(Data);
            return RedirectToAction("Index", "Blog", new { Account = User.Identity.Name });
        }
        //修改文章要根據傳入的文章編號決定要修改的資料
        [Authorize]
        public ActionResult EditPage(int A_Id)
        {
            Article Data = new Article();
            Data = articleService.GetArticleDataById(A_Id);
            return PartialView(Data);
        }
        //修改文章傳入資料的Action
        [Authorize]
        [HttpPost]
        public ActionResult EditPage(int A_Id, Article Data)
        {
            //判斷是否可以修改文章，有回文則不行
            if (articleService.CheckUpdate(A_Id))
            {
                articleService.UpdateArticle(Data);
            }
            return RedirectToAction("Article", new { A_Id = A_Id });
        }
        //刪除文章要根據傳入的文章刪除資料
        [Authorize]
        public ActionResult Delete(int A_Id)
        {
            articleService.DeleteArticle(A_Id);
            return RedirectToAction("Index", "Blog", new { Account = User.Identity.Name });
        }
        public ActionResult ShowPopularity(string Account)
        {
            ArticleIndexViewModel Data = new ArticleIndexViewModel();
            //取得頁面所需的人氣資料陣列
            Data.DataList = articleService.GetPopularList(Account);
            return View(Data);
        }
    }
}