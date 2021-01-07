using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace 部落格實作.ViewModels
{
    public class MembersLoginViewModel
    {
        [DisplayName("會員帳號")]
        [Required(ErrorMessage ="請輸入會員編號")]
        public string Account { get; set; }

        [DisplayName("會員密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }
    }
}