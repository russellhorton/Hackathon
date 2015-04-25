using EpilepsySite.Web.Objects;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Web;

namespace EpilepsySite.Web.Data
{
    public class MotionSensor
    {

        private const string InsertMotionSensorItemQuery = "INSERT INTO dt_MotionSensor (UserId, SyncId, TimeStamp, XValue, YValue, ZValue, Gravity) VALUES (@UserId,@SyncId,@TimeStamp,@XValue,@YValue,@ZValue,@Gravity)";

        public static bool InsertMotionSensorItem(MotionSensorItem motionSensorItem)
        {
            bool success = false;

            SqlCeConnection connection = new SqlCeConnection(Configuration.ConfigurationManager.ConnectionString);

            SqlCeCommand insertSQL = new SqlCeCommand(InsertMotionSensorItemQuery, connection);

            insertSQL.Parameters.AddWithValue("@TimeStamp", motionSensorItem.DateTime);
            insertSQL.Parameters.AddWithValue("@UserId", motionSensorItem.UserId);
            insertSQL.Parameters.AddWithValue("@SyncId", motionSensorItem.SyncId);
            insertSQL.Parameters.AddWithValue("@XValue", motionSensorItem.XValue);
            insertSQL.Parameters.AddWithValue("@YValue", motionSensorItem.YValue);
            insertSQL.Parameters.AddWithValue("@ZValue", motionSensorItem.ZValue);
            insertSQL.Parameters.AddWithValue("@Gravity", motionSensorItem.Gravity);

            try
            {
                connection.Open();
                if (insertSQL.ExecuteNonQuery() > 0)
                {
                    SqlCeCommand command = new SqlCeCommand("SELECT @@IDENTITY AS Id", connection);
                    int rowId = 0;
                    object newRowId = command.ExecuteScalar();
                    int.TryParse(newRowId.ToString(), out rowId);
                    motionSensorItem.Id = rowId;
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