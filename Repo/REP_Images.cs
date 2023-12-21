using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System;
using System.Text;
using System.Collections.Generic;

public class REP_Images
{
    public void insertImages(string ImgName, string ImgLoc, byte[] ImgData , string ImgText)
    {
        using (SqlConnection con = clsConnectionDB.OpenConnection())
        {
            SqlCommand com = new SqlCommand("Proc_InsImages", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Name" , ImgName);
            com.Parameters.AddWithValue("@Loc" , ImgLoc);
            com.Parameters.AddWithValue("@Data" , ImgData);
            com.Parameters.AddWithValue("@Text", ImgText);
            com.ExecuteNonQuery();
            con.Close();

        }
    }

    public void deleteImages(int ID)
    {
        using(SqlConnection con = clsConnectionDB.OpenConnection())
        {
            SqlCommand com = new SqlCommand("Proc_DeleteImages" , con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ID", ID);
            com.ExecuteNonQuery ();
            con.Close();
        }
    }


    public List<Images12> getAllImages()
    {
        List<Images12> images = new List<Images12>();

        using (SqlConnection conn = clsConnectionDB.OpenConnection())
        {
            SqlCommand com = new SqlCommand("Proc_GetAllImages", conn);
            com.CommandType = CommandType.StoredProcedure;
            try
            {
                using (SqlDataReader SDR = com.ExecuteReader())
                {
                    while (SDR.Read())
                    {
                        images.Add(new Images12
                        {
                            ImgID = SDR["ImgID"].ToString(),
                            ImgName = SDR["ImgName"].ToString(),
                            ImgLoc = SDR["ImgLoc"].ToString(),
                            ImgData = Convert.ToBase64String((byte[])SDR["ImgData"]),
                            ImgText = SDR["ImgText"].ToString()
                        });
                    }
                }
                return images;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}