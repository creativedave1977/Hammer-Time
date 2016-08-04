using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HammerTime.Classes.Data
{
    public class HammerData
    {
        #region "assets"
        //fields
        private string conn = "";

        #endregion

        #region "public methods"
        //constructor
        public HammerData()
        {
            conn = ConfigurationManager.ConnectionStrings["HammerTime"].ToString();
        }

        public int AddHammer(string HammerName, string HammerDesc, int Qty)
        {
            int intReturn = 0;
            string strSQL = "proc_AddHammer";
            SqlConnection dbConn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand(strSQL, dbConn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@HammerName", SqlDbType.VarChar).Value = HammerName;
            cmd.Parameters.Add("@HammerDesc", SqlDbType.VarChar).Value = HammerDesc;
            cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = Qty;
            SqlParameter NewID = new SqlParameter("@NewID", SqlDbType.Int);
            NewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(NewID);

            foreach (IDataParameter param in cmd.Parameters)
            {
                if (param.Value == null) param.Value = DBNull.Value;
            }
            try
            {
                dbConn.Open();
                cmd.ExecuteNonQuery();
                intReturn = Convert.ToInt32(NewID.Value);
            }
            catch (Exception ex)
            {
                intReturn = -1;
                //TODO  Exception Log
            }
            finally
            {
                dbConn.Close();
            }
            return intReturn;
        }

        public bool DeleteHammer(int HammerID)
        {
            bool ynSuccess;
            string strSQL = "proc_DeleteHammer";
            SqlConnection dbConn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand(strSQL, dbConn);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@HammerID", SqlDbType.Int).Value = HammerID;

            foreach (IDataParameter param in cmd.Parameters)
            {
                if (param.Value == null) param.Value = DBNull.Value;
            }
            try
            {
                dbConn.Open();
                cmd.ExecuteNonQuery();
                ynSuccess = true;
            }
            catch (Exception ex)
            {
                //TODO  Exception Log
                ynSuccess = false;
            }
            finally
            {
                dbConn.Close();
            }
            return ynSuccess;
        }

        public DataTable GetHammers()
        {
            string strSQL = "proc_GetHammers";
            SqlConnection dbConn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand(strSQL, dbConn);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                dbConn.Open();
                cmd.ExecuteNonQuery();
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                //TODO  Exception Log
            }
            finally
            {
                dbConn.Close();
            }
            return ds.Tables[0];
        }

        public bool UpdateHammer(int HammerID, string HammerName, string HammerDesc, int Qty)
        {
            bool ynSuccess;
            string strSQL = "proc_UpdateHammer";
            SqlConnection dbConn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand(strSQL, dbConn);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@HammerID", SqlDbType.Int).Value = HammerID;
            cmd.Parameters.Add("@HammerName", SqlDbType.VarChar).Value = HammerName;
            cmd.Parameters.Add("@HammerDesc", SqlDbType.VarChar).Value = HammerDesc;
            cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = Qty;

            foreach (IDataParameter param in cmd.Parameters)
            {
                if (param.Value == null) param.Value = DBNull.Value;
            }
            try
            {
                dbConn.Open();
                cmd.ExecuteNonQuery();
                ynSuccess = true;
            }
            catch (Exception ex)
            {
                //TODO  Exception Log
                ynSuccess = false;
            }
            finally
            {
                dbConn.Close();
            }
            return ynSuccess;
        }

        public bool UpdateHammerQty(int HammerID, int Qty)
        {
            bool ynSuccess;
            string strSQL = "proc_UpdateHammerQty";
            SqlConnection dbConn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand(strSQL, dbConn);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@HammerID", SqlDbType.Int).Value = HammerID;
            cmd.Parameters.Add("@UpdateQty", SqlDbType.Int).Value = Qty;

            foreach (IDataParameter param in cmd.Parameters)
            {
                if (param.Value == null) param.Value = DBNull.Value;
            }
            try
            {
                dbConn.Open();
                cmd.ExecuteNonQuery();
                ynSuccess = true;
            }
            catch (Exception ex)
            {
                //TODO  Exception Log
                ynSuccess = false;
            }
            finally
            {
                dbConn.Close();
            }
            return ynSuccess;
        }
        #endregion
    }
}