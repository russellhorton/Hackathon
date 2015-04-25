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

    }
}