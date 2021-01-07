using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using Jose;

namespace 部落格實作.Security
{
    public class JwtService
    {
        //製作Token
        public string GenerateToken(string Account, string Role)
        {
            JwtObject jwtObject = new JwtObject
            {
                Account = Account,
                Role = Role,
                Expire = DateTime.Now.AddMinutes(Convert.ToInt32(WebConfigurationManager.AppSettings["ExpireMinutes"])).ToString()
            };
            //從Web.Config取得密鑰
            string SecretKey = WebConfigurationManager.AppSettings["SecretKey"].ToString();
            //JWT的內容
            var payload = jwtObject;
            //將資料加密為Yoken
            var token = JWT.Encode(payload, Encoding.UTF8.GetBytes(SecretKey), JwsAlgorithm.HS512);
            return token;
        }
    }
}