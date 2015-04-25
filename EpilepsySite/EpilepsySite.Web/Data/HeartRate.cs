using EpilepsySite.Web.Objects;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace EpilepsySite.Web.Data
{
    public class HeartRate
    {

        private const string InsertHeartRateItemQuery = "INSERT INTO dt_HeartRate (UserId, SyncId, TimeStamp, HeartRate) VALUES (?,?,?,?)";

        public static bool InsertHeartRateItem(HeartRateItem heartRateItem)
        {
            bool success = false;
            SQLiteConnection connection = new SQLiteConnection(Configuration.ConfigurationManager.ConnectionString);

            SQLiteCommand insertSQL = new SQLiteCommand(InsertHeartRateItemQuery, connection);

            insertSQL.Parameters.Add(heartRateItem.UserId);
            insertSQL.Parameters.Add(heartRateItem.SyncId);
            insertSQL.Parameters.Add(heartRateItem.DateTime);
            insertSQL.Parameters.Add(heartRateItem.HeartRate);
            
            try
            {
                if (insertSQL.ExecuteNonQuery() > 0)
                {
                    SQLiteCommand command = new SQLiteCommand(@"select last_insert_rowid()", connection);
                    heartRateItem.Id = (int)command.ExecuteScalar();
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