using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using 部落格實作.Models;
using 部落格實作.Services;

namespace 部落格實作.ViewModels
{
    public class MessageViewModel
    {
        public List<Message> DataList { get; set; }
        public ForPaging Paging { get; set; }
        public int A_Id { get; set;}
    }
}