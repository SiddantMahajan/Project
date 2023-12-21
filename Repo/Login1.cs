using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System;
using System.Text;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Runtime.Remoting.Messaging;

public class Login1
{
    static string EncryptStringToBytes(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    static string DecryptStringFromBytes(string cipherText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            encryptor.Mode = CipherMode.CBC;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }



    public void InsertLogin(string Name, string UserName, string Password, string MobNo , string Role)
    {

            string encrypted = EncryptStringToBytes(Password);
        


        using (SqlConnection con = clsConnectionDB.OpenConnection())
        {
            SqlCommand com = new SqlCommand("Proc_InsLogin", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Name", Name);
            com.Parameters.AddWithValue("@UserName", UserName);
            com.Parameters.AddWithValue("@Password", encrypted);
            com.Parameters.AddWithValue("@MobNo", MobNo);
            com.Parameters.AddWithValue("@Role", Role);
            com.ExecuteNonQuery();
            con.Close();
        }

    }


    public bool GetLogin(String UserName , String Password)
    {
        bool Prog = false;
        string decrypted;
        using (SqlConnection con = clsConnectionDB.OpenConnection())
        {
            SqlCommand com = new SqlCommand("Proc_GetLogin", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserName", UserName);
            SqlDataReader SDR = com.ExecuteReader();
            while (SDR.Read())
            {
                String UName = (string)SDR["UserName"];
                String Pass = (string)SDR["Password"];
                decrypted = DecryptStringFromBytes(Pass);

                if (decrypted.Equals(Password))
                {
                    Prog = true;
                }

            }
            con.Close();
        }

        if(Prog == true)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
    public List<Login12> GetData(string UserName)
    {
        List<Login12> resultList = new List<Login12>();

        using (SqlConnection con = clsConnectionDB.OpenConnection())
        {
            using (SqlCommand com = new SqlCommand("Proc_GetLogin", con))
            {
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 255) { Value = UserName });

                try
                {
                    using (SqlDataReader SDR = com.ExecuteReader())
                    {
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
                    }

                    return resultList;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return null;
                }
            }
        }
    }

}