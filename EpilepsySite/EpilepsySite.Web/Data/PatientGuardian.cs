using EpilepsySite.Web.Objects;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Web;

namespace EpilepsySite.Web.Data
{
    public class PatientGuardian
    {
        private const string GetAllGuardiansForPatientQuery = "SELECT * FROM dt_PatientGuardianLink where patientid = @patientId";
        private const string GetAllPatientsForGuardianQuery = "SELECT * FROM dt_PatientGuardianLink where guardianid = @guardianId";
        private const string GetPatientsPatientGuardianLinkQuery = "SELECT * FROM dt_PatientGuardianLink where id = @linkId";
        private const string InsertPatientGuardianLinkQuery = "INSERT INTO dt_PatientGuardianLink ( PatientId, GuardianId, Status, DateRequested) VALUES (@patientid, @guardianid, @status, @daterequested)";
        private const string UpdatePatientGuardianLinkQuery = "UPDATE dt_PatientGuardianLink SET Status= @status, DateConfirmed= @dateconfirmed WHERE Id= @linkid";

        public static bool UpdatePatientGuardianLink(int linkId, GuardianStatus status)
        {
            bool success = false;

            SqlCeConnection connection = new SqlCeConnection(Configuration.ConfigurationManager.ConnectionString);

            SqlCeCommand insertSQL = new SqlCeCommand(UpdatePatientGuardianLinkQuery, connection);

            insertSQL.Parameters.AddWithValue("@linkid", linkId);            
            insertSQL.Parameters.AddWithValue("@status", status);            
            insertSQL.Parameters.AddWithValue("@dateconfirmed", DateTime.Now);

            try
            {
                connection.Open();
                if (insertSQL.ExecuteNonQuery() > 0)
                {                    
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

        public static bool InsertPatientGuardianLink(PatientGuardianLink link)
        {
            bool success = false;

            SqlCeConnection connection = new SqlCeConnection(Configuration.ConfigurationManager.ConnectionString);

            SqlCeCommand insertSQL = new SqlCeCommand(InsertPatientGuardianLinkQuery, connection);

            insertSQL.Parameters.AddWithValue("@patientid", link.PatientId);
            insertSQL.Parameters.AddWithValue("@guardianid", link.GuardianId);
            insertSQL.Parameters.AddWithValue("@status", link.Status);
            insertSQL.Parameters.AddWithValue("@daterequested", link.DateRequested);
            

            try
            {
                connection.Open();
                if (insertSQL.ExecuteNonQuery() > 0)
                {
                    SqlCeCommand command = new SqlCeCommand("SELECT @@IDENTITY AS Id", connection);
                    int rowId = 0;
                    object newRowId = command.ExecuteScalar();
                    int.TryParse(newRowId.ToString(), out rowId);
                    link.Id = rowId;
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

        public static PatientGuardianLink GetPatientsPatientGuardianLink(int linkid)
        {
            

            SqlCeConnection connection = new SqlCeConnection(Configuration.ConfigurationManager.ConnectionString);

            SqlCeCommand selectSQL = new SqlCeCommand(GetPatientsPatientGuardianLinkQuery, connection);
            selectSQL.Parameters.AddWithValue("@linkId", linkid);
            PatientGuardianLink link = new PatientGuardianLink();
            try
            {
                connection.Open();

                SqlCeDataReader dataReader = selectSQL.ExecuteReader();
                
                while (dataReader.Read())
                {
                   
                    link.Id = (int)dataReader.GetSqlInt32(dataReader.GetOrdinal("id"));
                    link.GuardianId = (int)dataReader.GetSqlInt32(dataReader.GetOrdinal("GuardianId"));
                    link.PatientId = (int)dataReader.GetSqlInt32(dataReader.GetOrdinal("PatientId"));
                    link.DateRequested = (DateTime)dataReader.GetSqlDateTime(dataReader.GetOrdinal("DateRequested"));
                    link.DateConfirmed = (DateTime)dataReader.GetSqlDateTime(dataReader.GetOrdinal("DateConfirmed"));
                    link.Status = (GuardianStatus)(int)dataReader.GetSqlInt32(dataReader.GetOrdinal("Status"));
                    
                }
                
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return link;

        }

        public static List<PatientGuardianLink> GetAllGuardiansForPatient(int userid)
        {
            List<PatientGuardianLink> allGuardiansForPatients = new List<PatientGuardianLink>();

            SqlCeConnection connection = new SqlCeConnection(Configuration.ConfigurationManager.ConnectionString);

            SqlCeCommand selectSQL = new SqlCeCommand(GetAllGuardiansForPatientQuery, connection);
            selectSQL.Parameters.AddWithValue("@patientId", userid);

            try
            {
                connection.Open();

                SqlCeDataReader dataReader = selectSQL.ExecuteReader();

                while (dataReader.Read())
                {
                    PatientGuardianLink link = new PatientGuardianLink();
                    link.Id = (int)dataReader.GetSqlInt32(dataReader.GetOrdinal("id"));
                    link.GuardianId = (int)dataReader.GetSqlInt32(dataReader.GetOrdinal("GuardianId"));
                    link.PatientId = (int)dataReader.GetSqlInt32(dataReader.GetOrdinal("PatientId"));
                    link.DateRequested = (DateTime)dataReader.GetSqlDateTime(dataReader.GetOrdinal("DateRequested"));
                    link.DateConfirmed = (DateTime)dataReader.GetSqlDateTime(dataReader.GetOrdinal("DateConfirmed"));
                    link.Status = (GuardianStatus)(int)dataReader.GetSqlInt32(dataReader.GetOrdinal("Status"));
                    allGuardiansForPatients.Add(link);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return allGuardiansForPatients;

        }

        public static List<PatientGuardianLink> GetAllPatientsForGuardian(int userid)
        {
            List<PatientGuardianLink> allPatientsForGuardians = new List<PatientGuardianLink>();

            SqlCeConnection connection = new SqlCeConnection(Configuration.ConfigurationManager.ConnectionString);

            SqlCeCommand selectSQL = new SqlCeCommand(GetAllPatientsForGuardianQuery, connection);
            selectSQL.Parameters.AddWithValue("@guardianId", userid);

            try
            {
                connection.Open();

                SqlCeDataReader dataReader = selectSQL.ExecuteReader();

                while (dataReader.Read())
                {
                    PatientGuardianLink link = new PatientGuardianLink();
                    link.Id = (int)dataReader.GetSqlInt32(dataReader.GetOrdinal("id"));
                    link.GuardianId = (int)dataReader.GetSqlInt32(dataReader.GetOrdinal("GuardianId"));
                    link.PatientId = (int)dataReader.GetSqlInt32(dataReader.GetOrdinal("PatientId"));
                    link.DateRequested = (DateTime)dataReader.GetSqlDateTime(dataReader.GetOrdinal("DateRequested"));
                    link.DateConfirmed = (DateTime)dataReader.GetSqlDateTime(dataReader.GetOrdinal("DateConfirmed"));
                    link.Status = (GuardianStatus)(int)dataReader.GetSqlInt32(dataReader.GetOrdinal("Status"));
                    allPatientsForGuardians.Add(link);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return allPatientsForGuardians;

        }

    }
}