using EpilepsySite.Web.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlServerCe;

namespace EpilepsySite.Web.Data
{
    public class Sync
    {

        private const string InsertSyncItemQuery = "INSERT INTO dt_Sync (DateTime, UserId, PacketLength, Status, Long, Lat, Alt, Accuracy) VALUES (@DateTime,@UserId,@PacketLength,@Status,@Long,@Lat,@Alt,@Accuracy)";

        public static bool InsertSyncItem(SyncItem syncItem)
        {
            bool success = false;
            SqlCeConnection connection = new SqlCeConnection(Configuration.ConfigurationManager.ConnectionString);
            //connection.SetPassword("password");
            SqlCeCommand insertSQL = new SqlCeCommand(InsertSyncItemQuery, connection);
          
            insertSQL.Parameters.AddWithValue("@DateTime", syncItem.DateTime);
            insertSQL.Parameters.AddWithValue("@UserId", syncItem.UserId);
            insertSQL.Parameters.AddWithValue("@PacketLength", syncItem.HeartRatePackets.Count);
            insertSQL.Parameters.AddWithValue("@Status", syncItem.Status);
            insertSQL.Parameters.AddWithValue("@Long", syncItem.Long);
            insertSQL.Parameters.AddWithValue("@Lat", syncItem.Lat);
            insertSQL.Parameters.AddWithValue("@Alt", syncItem.Alt);
            insertSQL.Parameters.AddWithValue("@Accuracy", syncItem.Accuracy);
            
            try
            {
                connection.Open();
                if (insertSQL.ExecuteNonQuery() > 0)
                {
                    SqlCeCommand command = new SqlCeCommand("SELECT @@IDENTITY AS Id", connection);
                    int rowId = 0;
                    object newRowId = command.ExecuteScalar();
                    int.TryParse(newRowId.ToString(), out rowId);
                    syncItem.Id = rowId;
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