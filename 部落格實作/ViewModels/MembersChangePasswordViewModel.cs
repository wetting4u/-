using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace 部落格實作.ViewModels
{
    public class MembersChangePasswordViewModel
    {
        [DisplayName("舊密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }

        [DisplayName("新密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string NewPassword { get; set; }

        [DisplayName("密碼確認")]
        [Required(ErrorMessage = "請輸入密碼")]
        [Compare("Password", ErrorMessage = "兩次密碼輸入不一致")]
        public string NewPasswordCheck { get; set; }
    }
}