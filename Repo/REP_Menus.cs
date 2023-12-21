using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using WS_Admin.DataModels;

public class REP_Menus
{
    public void insertMenu(string MenuName , string MenuLink)
    {
        using(SqlConnection conn = clsConnectionDB.OpenConnection())
        {
            SqlCommand cmd = new SqlCommand("Proc_InsMenu", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", MenuName);
            cmd.Parameters.AddWithValue("@Link", MenuLink);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }


    public List<Menus12> getMenus()
    {
        List<Menus12> ARL = new List<Menus12>();
        using(SqlConnection conn = clsConnectionDB.OpenConnection())
        {
            SqlCommand cmd = new SqlCommand("Proc_GetMenus", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader SDR = cmd.ExecuteReader())
            {
                while(SDR.Read())
                {
                    ARL.Add(new Menus12
                    {
                        MenuID = SDR["MenuID"].ToString(),
                        MenuName = SDR["MenuName"].ToString(),
                        MenuLink = SDR["MenuLink"].ToString()
                    });
                }
            }
        }
        return ARL;
    }


    public void deleteMenu(int MenuID)
    {
        using(SqlConnection conn = clsConnectionDB.OpenConnection())
        {
            SqlCommand cmd = new SqlCommand("Proc_DeleteMenu", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", MenuID);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
