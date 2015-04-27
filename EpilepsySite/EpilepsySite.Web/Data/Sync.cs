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
        private const string GetSyncHistoryQuery = "SELECT * FROM dt_Sync where userid = @userid ORDER BY DateTime DESC";

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
            insertSQL.Parameters.AddWithValue("@Long", syncItem.Lng);
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

        public static List<SyncItem> GetAllSyncHistory(int userId)
        {
            
            SqlCeConnection connection = new SqlCeConnection(Configuration.ConfigurationManager.ConnectionString);

            SqlCeCommand insertSQL = new SqlCeCommand(GetSyncHistoryQuery, connection);

            insertSQL.Parameters.AddWithValue("@UserId", userId);
            List<SyncItem> syncitems = new List<SyncItem>();
            try
            {
                connection.Open();

                SqlCeDataReader dataReader = insertSQL.ExecuteReader();

                while (dataReader.Read())
                {
                    SyncItem syncItem = new SyncItem();
                    syncItem.DateTime = (DateTime)dataReader.GetSqlDateTime(dataReader.GetOrdinal("DateTime"));
                    syncItem.UserId = (int)dataReader.GetSqlInt32(dataReader.GetOrdinal("UserId"));
                    syncItem.Accuracy = (int)dataReader.GetSqlInt32(dataReader.GetOrdinal("Accuracy"));
                    syncItem.Lng = (float)dataReader.GetSqlDouble(dataReader.GetOrdinal("Long"));
                    syncItem.Lat = (float)dataReader.GetSqlDouble(dataReader.GetOrdinal("Lat"));
                    syncItem.Status = (string)dataReader.GetSqlString(dataReader.GetOrdinal("Status"));

                    syncitems.Add(syncItem);
                }

            }
            catch (Exception ex)
            {
               
            }
            finally
            {
                connection.Close();

            }

            return syncitems;
        }

        

    }
}