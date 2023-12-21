using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace WS_Admin.Repo
{
    public class ViewUsers
    {

        public List<Login12> GetUsers()
        {
            List<Login12> resultList = new List<Login12>();
            using (SqlConnection con = clsConnectionDB.OpenConnection())
            {
                SqlCommand com = new SqlCommand("Proc_GetUsers", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader SDR = com.ExecuteReader();
                while (SDR.Read())
                {
                    resultList.Add(new Login12
                    {
                        Name = SDR["Name"].ToString(),
                        UserID = SDR["UserName"].ToString(),
                        MobNo = SDR["MobNo"].ToString(),
                        Role = SDR["Role"].ToString()
                    });
                }
                con.Close();
            }
            return resultList;
        }

    }
}