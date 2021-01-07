using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using 部落格實作.Models;

namespace 部落格實作.Services
{
    public class MessageDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["Blog"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);

        //根據分頁以及搜尋來取得資料陣列的方法
        public List<Message> GetDataList(ForPaging Paging, int A_Id)
        {
            List<Message> DataList = new List<Message>();
            SetMaxPaging(Paging, A_Id);
            DataList = GetAllDataList(Paging, A_Id);
            return DataList;
        }
        public void SetMaxPaging(ForPaging Paging, int A_Id)
        {
            //計算列數
            int Row = 0;
            string sql = $@" SELECT * FROM Message WHERE A_Id = {A_Id}; ";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Row++;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            Paging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Row) / Paging.ItemNum));
            Paging.SetRightPage();
        }
        public List<Message> GetAllDataList(ForPaging paging, int A_Id)
        {
            //宣告要回傳的搜尋資料為資料庫中的Message資料表
            List<Message> DataList = new List<Message>();
            string sql = $@" SELECT m.*, d.Name FROM (SELECT row_Number() OVER(ORDER BY M_Id) AS sort,* FROM Message 
                WHERE A_Id = {A_Id}) m INNER JOIN Members d ON m.Account = d.Account WHERE m.sort BETWEEN {(paging.NowPage - 1) * paging.ItemNum + 1} AND {paging.NowPage * paging.ItemNum};";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Message Data = new Message();
                    Data.M_Id = Convert.ToInt32(dr["M_Id"]);
                    Data.A_Id = Convert.ToInt32(dr["A_Id"]);
                    Data.Account = dr["Account"].ToString();
                    Data.Content = dr["Content"].ToString();
                    Data.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                    Data.Member.Name = dr["Name"].ToString();
                    DataList.Add(Data);
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return DataList;
        }
        public void InsertMessage(Message newData)
        {
            newData.M_Id = LastMessageFinder(newData.A_Id);
            string sql = $@" INSERT INTO Message (A_Id,M_Id,Account,Content,CreateTime) VALUES ( '{newData.A_Id}','{newData.M_Id}','{newData.Account}','{newData.Content}','{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}');";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public int LastMessageFinder(int A_Id)
        {
            //宣告要回傳的值
            int Id;
            string sql = $@" SELECT TOP 1 * FROM Message WHERE A_Id = {A_Id} ORDER BY M_Id DESC;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Id = Convert.ToInt32(dr["M_Id"]);
            }
            catch(Exception e)
            {
                Id = 0;
            }
            finally
            {
                conn.Close();
            }
            return Id + 1;
        }
        public void UpdateMessage(Message UpdateData)
        {
            string sql = $@" UPDATE Message SET Content = '{UpdateData.Content}' WHERE A_Id = {UpdateData.A_Id} AND M_Id = {UpdateData.M_Id};";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public void DeleteMessage(int A_Id, int M_Id)
        {
            string DeleteMessage = $@" DELETE FROM Message WHERE A_Id = {A_Id} AND M_Id ={M_Id};";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(DeleteMessage, conn);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
    }
    


}