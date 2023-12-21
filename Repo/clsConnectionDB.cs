using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

public static class clsConnectionDB
{
    public static SqlConnection OpenConnection()
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCON"].ConnectionString;

        SqlConnection con = new SqlConnection(cs);

        con.Open();

        return con;

    }
}