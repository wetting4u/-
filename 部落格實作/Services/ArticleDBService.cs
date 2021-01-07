using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using 部落格實作.Models;

namespace 部落格實作.Services
{
    public class ArticleDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["Blog"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);

        //藉由編號取得單筆資料的方法
        public Article GetArticleDataById(int A_Id)
        {
            Article Data = new Article();
            string sql = $@" SELECT FROM m.*,d.Name,d.Image FROM Article m INNER JOIN Members d ON m.Account = d.Account WHERE m.A_Id = {A_Id};";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.A_Id = Convert.ToInt32(dr["A_Id"]);
                Data.Account = dr["Account"].ToString();
                Data.Title = dr["Title"].ToString();
                Data.Content = dr["Content"].ToString();
                Data.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                Data.Watch = Convert.ToInt32(dr["Watch"]);
                Data.Member.Name = dr["Name"].ToString();
                Data.Member.Image = dr["Image"].ToString();
            }
            catch(Exception e)
            {
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }
        //根據搜尋來取得資料陣列的方法
        public List<Article> GetDataList(ForPaging Paging, string Search, string Account)
        {
            //宣告要接受全部搜尋資料的物件
            List<Article> DataList = new List<Article>();
            if(!string.IsNullOrWhiteSpace(Search))
            {
                SetMaxPaging(Paging, Search, Account);
                DataList = GetAllDataList(Paging, Search, Account);
            }
            else
            {
                SetMaxPaging(Paging, Account);
                DataList = GetAllDataList(Paging, Account);
            }
            return DataList;
        }
        public List<Article> GetAllDataList(ForPaging paging, string Account)
        {
            List<Article> DataList = new List<Article>();
            string sql = $@" SELECT m.*, d.Name FROM (SELECT row_number() OVER(ORDER BY A_Id) AS sort,* FROM Article WHERE Account = '{Account}') m INNER JOIN Members d ON m.Account = d.Account WHERE m.sort BETWEEN {(paging.NowPage - 1) * paging.ItemNum + 1} AND {paging.NowPage * paging.ItemNum};";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Article Data = new Article();
                    Data.A_Id = Convert.ToInt32(dr["A_Id"]);
                    Data.Account = dr["Account"].ToString();
                    Data.Title = dr["Title"].ToString();
                    Data.Content = dr["Content"].ToString();
                    Data.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                    Data.Watch = Convert.ToInt32(dr["Watch"]);
                    Data.Member.Name = dr["Name"].ToString();
                    DataList.Add(Data);
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
            return DataList;
        }
        public List<Article> GetAllDataList(ForPaging paging, string Search, string Account)
        {
            List<Article> DataList = new List<Article>();
            string sql = $@" SELECT m.*,d.Name FROM (SELECT row_number() OVER(ORDER BY A_Id) AS Sort,* FROM Article WHERE (Title LIKE '%{Search}%' OR Content LIKE '%{Search}%') AND Account = '{Account}' )
                m INNER JOIN Members d ON m.Account = d.Account WHERE m.sort BETWEEN {(paging.NowPage - 1) * paging.ItemNum + 1} AND {paging.NowPage * paging.ItemNum};";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Article Data = new Article();
                    Data.A_Id = Convert.ToInt32(dr["A_Id"]);
                    Data.Account = dr["Account"].ToString();
                    Data.Title = dr["Title"].ToString();
                    Data.Content = dr["Content"].ToString();
                    Data.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                    Data.Watch = Convert.ToInt32(dr["Watch"]);
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
        public void SetMaxPaging(ForPaging Paging, string Account)
        {
            //計算列數
            int Row = 0;
            string sql = $@" SELECT * FROM Article WHERE Account = '{Account}';";
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
        public void SetMaxPaging(ForPaging Paging, string Search, string Account)
        {
            //計算列數
            int Row = 0;
            string sql = $@" SELECT * FROM Article WHERE ( Title LIKE '%{Search}%' OR Content LIKE '%{Search}%' ) AND Account = '{Account}'; ";
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
        //新增資料方法
        public void InsertArticle(Article newData)
        {
            newData.A_Id = LastArticleFinder();
            string sql = $@" INSERT INTO Article(A_Id,Title,Content,Account,CreateTime,Watch) VALUES ('{newData.A_Id}',{newData.Title},'{newData.Content}','{newData.Account}','{DateTime.Now.ToString("yyyy/MM/dd HH:mm;ss")}',0);";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        //計算目前文章最新一筆的A_Id
        public int LastArticleFinder()
        {
            //宣告要回傳的值
            int Id;
            string sql = $@" SELECT TOP 1 * FROM article ORDER BY A_Id DESC;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Id = Convert.ToInt32(dr["A_Id"]);
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
        //修改文章方法
        public void UpdateArticle(Article UpdateData)
        {
            string sql = $@" UPDATE Article SET Content = '{UpdateData.Content}' WHERE A_Id = {UpdateData.A_Id};";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public void DeleteArticle(int A_Id)
        {
            //必須將該文章的留言刪除
            string DeleteMessage = $@" DELETE FROM Message WHERE A_Id = {A_Id}; ";
            //再根據文章Id取得要刪除的文章
            string DeleteArticle = $@" DELETE FROM Article WHERE A_Id = {A_Id}; ";
            //將兩段SQL語法一起放入SQL執行，能避免一值開放資料庫連線，降低資料庫的負擔
            string combineSql = DeleteMessage + DeleteArticle;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(combineSql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public bool CheckUpdate(int A_Id)
        {
            Article Data = GetArticleDataById(A_Id);
            //留言筆數
            int MessageCount = 0;
            string sql = $@" SELECT * FROM Message WHERE A_Id = {A_Id};";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    MessageCount++;
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
            return (Data != null && MessageCount == 0);
        }
        public List<Article> GetPopularList(string Account)
        {
            List<Article> popularList = new List<Article>();
            //查詢TOP 5 watch
            string sql = $@" SELECT TOP 5 * FROM Article m INNER JOIN Members d ON m.Account = d.Account WHERE m.Account = '{Account}' ORDER BY watch DESC;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Article Data = new Article();
                    Data.A_Id = Convert.ToInt32(dr["A_Id"]);
                    Data.Account = dr["Account"].ToString();
                    Data.Title = dr["Title"].ToString();
                    Data.Content = dr["Content"].ToString();
                    Data.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                    Data.Watch = Convert.ToInt32(dr["Watch"]);
                    Data.Member.Name = dr["Name"].ToString();
                    popularList.Add(Data);
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
            return popularList;
        }
        public void AddWatch(int A_id)
        {
            string sql = $@" UPDATE Article SET Watch = Watch + 1 WHERE A_Id = '{A_id}';";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
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