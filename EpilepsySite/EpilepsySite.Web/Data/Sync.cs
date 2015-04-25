using EpilepsySite.Web.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SQLite;

namespace EpilepsySite.Web.Data
{
    public class Sync
    {

        private const string InsertSyncItemQuery = "INSERT INTO dt_Sync (DateTime, UserId, PacketLength, Status, Long, Lat, Alt, Accuracy) VALUES (@DateTime,@UserId,@PacketLength,@Status,@Long,@Lat,@Alt,@Accuracy)";

        public static bool InsertSyncItem(SyncItem syncItem)
        {
            bool success = false;
            SQLiteConnection connection = new SQLiteConnection(Configuration.ConfigurationManager.ConnectionString);

            SQLiteCommand insertSQL = new SQLiteCommand(InsertSyncItemQuery, connection);
          
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
                    SQLiteCommand command = new SQLiteCommand(@"select last_insert_rowid()", connection);                     
                    syncItem.Id = (int)command.ExecuteScalar();
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