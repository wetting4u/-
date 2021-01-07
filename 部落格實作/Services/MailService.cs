using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace 部落格實作.Services
{
    public class MailService
    {
        private string gmail_account = "wettingforyou";
        private string gmail_password = "baloneyone";
        private string gmail_mail = "wettingforyou@gmail.com";
        //寄會員驗證信，產生驗證碼方法
        public string GetValidateCode()
        {
            //設定驗證字元的陣列
            string[] Code = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V","W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9",
                "a","b","c","d","e","f","g","h","i","j","k","l","m","n","p","q","r","s","t","u","v","w","x","y","z" };
            //宣告初始為空的驗證碼字串
            string ValidateCode = string.Empty;
            //宣告可產生隨機數值的物件
            Random rd = new Random();
            //使用迴圈產生出驗證碼
            for (int i = 0; i < 10; i++)
            {
                ValidateCode += Code[rd.Next(Code.Count())];
            }
            //回傳驗證碼
            return ValidateCode;
        }
        //將使用者資料填入驗證信範本中
        public string GetRegisterMailBody(String TempString, string UserName, string ValidateUrl)
        {
            //將使用者資料填入
            TempString = TempString.Replace("{{UserName}}", UserName);
            TempString = TempString.Replace("{{ValidateUrl}}", ValidateUrl);
            //回傳最後結果
            return TempString;
        }
        //寄驗證信的方法
        public void SendRegisterMailstring(string MailBody, string ToEmail)
        {
            //建立寄信用Smtp物件，這裡使用Gmail為例
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            //設定使用的Port，這裡設定Gmail所使用的
            smtpServer.Port = 587;
            //建立使用者憑據，這裡要設定您的Gmai帳戶
            smtpServer.Credentials = new System.Net.NetworkCredential(gmail_account, gmail_password);
            //開啟SSL
            smtpServer.EnableSsl = true;
            //宣告信件內容物件
            MailMessage mail = new MailMessage();
            //設定來源信箱
            mail.From = new MailAddress(gmail_mail);
            //設定收信者信箱
            mail.To.Add(ToEmail);
            //設定信件主旨
            mail.Subject = "會員註冊確認信";
            //設定信件內容
            mail.Body = MailBody;
            //設定信件內容為Html格式
            mail.IsBodyHtml = true;
            //送出信件
            smtpServer.Send(mail);
        }
    }
}