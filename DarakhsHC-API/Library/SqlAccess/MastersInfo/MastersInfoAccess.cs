using DarakhsHC_API.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DarakhsHC_API.Library.SqlAccess.MastersInfo
{
    public class MastersInfoAccess
    {
        public static string connString = ConfigurationManager.ConnectionStrings["ConnStringDb"].ConnectionString;

        public static int UpsertReference(ReferencesInfo referencesInfo)
        {
            try
            {
                string sql = MastersInfo.UpsertReference;
                string strConString = connString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", referencesInfo.Id);
                    cmd.Parameters.AddWithValue("@MS_Comp_Id", referencesInfo.MS_Comp_Id);
                    cmd.Parameters.AddWithValue("@Reference", !string.IsNullOrEmpty(referencesInfo.Reference) ? referencesInfo.Reference : "");
                    
                    // cmd.ExecuteNonQuery();
                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        Int32.TryParse(returnObj.ToString(), out int returnValue);
                        return returnValue;
                    }

                    return 0;

                    // var ddd = employeeFamilyInformation.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int UpsertTreatement(TreatmentsInfo treatment)
        {
            try
            {
                string sql = MastersInfo.UpsertTreatment;
                string strConString = connString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", treatment.Id);
                    cmd.Parameters.AddWithValue("@MS_Comp_Id", treatment.MS_Comp_Id);
                    cmd.Parameters.AddWithValue("@TreamentName", !string.IsNullOrEmpty(treatment.TreamentName) ? treatment.TreamentName : "");

                    // cmd.ExecuteNonQuery();
                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        Int32.TryParse(returnObj.ToString(), out int returnValue);
                        return returnValue;
                    }

                    return 0;

                    // var ddd = employeeFamilyInformation.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}