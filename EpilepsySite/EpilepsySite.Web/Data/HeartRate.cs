using EpilepsySite.Web.Objects;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data.SqlServerCe;
using System.Linq;
using System.Web;

namespace EpilepsySite.Web.Data
{
    public class HeartRate
    {

        private const string InsertHeartRateItemQuery = "INSERT INTO dt_HeartRate (UserId, SyncId, TimeStamp, HeartRate) VALUES (@UserId,@SyncId,@TimeStamp,@HeartRate)";
        private const string GetAllHeartRateItemsQuery = "SELECT * FROM dt_HeartRate";
        private const string GetAllHeartRateItemsByUserIdQuery = "SELECT * FROM dt_HeartRate WHERE UserId = @userId";

        public static bool InsertHeartRateItem(HeartRateItem heartRateItem)
        {
            bool success = false;

            SqlCeConnection connection = new SqlCeConnection(Configuration.ConfigurationManager.ConnectionString);
            
            SqlCeCommand insertSQL = new SqlCeCommand(InsertHeartRateItemQuery, connection);

            insertSQL.Parameters.AddWithValue("@TimeStamp", heartRateItem.DateTime);
            insertSQL.Parameters.AddWithValue("@UserId", heartRateItem.UserId);
            insertSQL.Parameters.AddWithValue("@SyncId", heartRateItem.SyncId);
            insertSQL.Parameters.AddWithValue("@HeartRate", heartRateItem.HeartRate);

            try
            {
                connection.Open();
                if (insertSQL.ExecuteNonQuery() > 0)
                {
                    SqlCeCommand command = new SqlCeCommand("SELECT @@IDENTITY AS Id", connection);
                    int rowId = 0;
                    object newRowId = command.ExecuteScalar();
                    int.TryParse(newRowId.ToString(), out rowId);
                    heartRateItem.Id = rowId;
                    success = true;
                }
                else
                {
                    success = false;
                }

            }
            catch (Exception ex)
            {
                success = false;
            }
            finally
            {
                connection.Close();
            }

            return success;
        }

        public static List<HeartRateItem> GetAllHeartRateItems()
        {
            List<HeartRateItem> heartRateItems = new List<HeartRateItem>();

            SqlCeConnection connection = new SqlCeConnection(Configuration.ConfigurationManager.ConnectionString);

            SqlCeCommand selectSQL = new SqlCeCommand(GetAllHeartRateItemsQuery, connection);
            
            try
            {
                connection.Open();

                SqlCeDataReader dataReader = selectSQL.ExecuteReader();

                while (dataReader.Read())
                {
                    HeartRateItem heartRateItem = new HeartRateItem();
                    heartRateItem.DateTime = (DateTime)dataReader.GetSqlDateTime(dataReader.GetOrdinal("TimeStamp"));
                    heartRateItem.UserId = (int)dataReader.GetSqlInt32(dataReader.GetOrdinal("UserId"));
                    heartRateItem.HeartRate = (int)dataReader.GetSqlInt32(dataReader.GetOrdinal("HeartRate"));
                    heartRateItem.SyncId = (int)dataReader.GetSqlInt32(dataReader.GetOrdinal("SyncId"));
                    heartRateItems.Add(heartRateItem);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return heartRateItems;

        }

        public static List<HeartRateItem> GetAllHeartRateItemsByUserId(int userid)
        {
            List<HeartRateItem> heartRateItems = new List<HeartRateItem>();

            SqlCeConnection connection = new SqlCeConnection(Configuration.ConfigurationManager.ConnectionString);

            SqlCeCommand selectSQL = new SqlCeCommand(GetAllHeartRateItemsByUserIdQuery, connection);
            selectSQL.Parameters.AddWithValue("@UserId", userid);

            try
            {
                connection.Open();

                SqlCeDataReader dataReader = selectSQL.ExecuteReader();

                while (dataReader.Read())
                {
                    HeartRateItem heartRateItem = new HeartRateItem();
                    heartRateItem.DateTime = (DateTime)dataReader.GetSqlDateTime(dataReader.GetOrdinal("TimeStamp"));
                    heartRateItem.UserId = (int)dataReader.GetSqlInt32(dataReader.GetOrdinal("UserId"));
                    heartRateItem.HeartRate = (int)dataReader.GetSqlInt32(dataReader.GetOrdinal("HeartRate"));
                    heartRateItem.SyncId = (int)dataReader.GetSqlInt32(dataReader.GetOrdinal("SyncId"));
                    heartRateItems.Add(heartRateItem);                    
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return heartRateItems;
        }


    }
}