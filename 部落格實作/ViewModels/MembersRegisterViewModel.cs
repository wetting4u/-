using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using 部落格實作.Models;
namespace 部落格實作.ViewModels
{
    public class MembersRegisterViewModel
    {
        [DisplayName("大頭照")]
        //HttpPostedFileBase為上傳檔案的操作類別
        public HttpPostedFileBase MembersImage { get; set; }

        public Members newMember { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage ="請輸入密碼")]
        public string Password { get; set; }

        [DisplayName("確認密碼")]
        [Compare("Password", ErrorMessage ="兩次密碼輸入不一致")]
        [Required(ErrorMessage ="請輸入確認密碼")]
        public string PasswordCheck { get; set; }
    }
}