using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using 部落格實作.Models;
using 部落格實作.Services;

namespace 部落格實作.ViewModels
{
    public class ArticleIndexViewModel
    {
        [DisplayName("搜尋:")]
        public string Search { get; set; }

        public List<Article> DataList { get; set; }
        public ForPaging Paging { get;set; }
        public string Account { get; set; }
    }
}