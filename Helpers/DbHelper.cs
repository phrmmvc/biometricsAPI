
using Biometrics.Helpers.Interface;
using Biometrics.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace Biometrics.Helpers
{
    public class DbHelper : IDbHelper
    {
        private readonly IDbAccessHelper _dbAccessHelper;
        private string _connectionString;
        public DbHelper(IDbAccessHelper dbAccessHelper)
        {
            _dbAccessHelper = dbAccessHelper;
            _connectionString = "";
        }

        public void ExecuteNonQuery(string sql, Action<IDbCommand> addParameters)
        {
            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    var command = new OracleCommand(sql, connection);

                    addParameters(command);

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                throw;
            }
        }
        public DataTable GetData(string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                using (var connection = new OracleConnection(_connectionString))
                {
                    var command = new OracleCommand(sql, connection);
                    connection.Open();
                    using (OracleDataReader rdr = command.ExecuteReader())
                    {
                        dt.Load(rdr);
                        return dt;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                throw;
            }
        }
        public T ExecuteScalar<T>(string sql, Action<IDbCommand> addParameters)
        {
            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    var command = new OracleCommand(sql, connection);

                    addParameters(command);

                    connection.Open();

                    //return (T)command.ExecuteScalar();
                    object result = command.ExecuteScalar();

                    if (result is DBNull)
                    {
                        return default;
                    }

                    return (T)Convert.ChangeType(result, typeof(T));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                throw;
            }
        }
        public void ExecuteQuery(string sql, Action<IDataReader> readData, Action<IDbCommand> addParameters = null)
        {
            try
            {
                if (_connectionString != "")
                {
                    using (var connection = new OracleConnection(_connectionString))
                    {
                        var command = new OracleCommand(sql, connection);

                        addParameters?.Invoke(command);

                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                readData(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                throw;
            }







        }


        public void ExecuteQueryWithProcedure(string sql, Action<IDataReader> readData, Action<IDataParameterCollection> addParameters1, Action<IDataParameterCollection> addParameters2)
        {
            try
            {
                if (_connectionString != "")
                {
                    using (var connection = new OracleConnection(_connectionString))
                    {
                        var command = new OracleCommand(sql, connection);



                        connection.Open();
                        var command1 = new OracleCommand("PKG_TPMS_MONTHYR.SET_RTYP", connection);
                        command1.CommandType = CommandType.StoredProcedure;
                        addParameters1(command1.Parameters);

                        command1.ExecuteNonQuery();

                        var command2 = new OracleCommand("PKG_TPMS_MONTHYR.SET_YEARNO", connection);
                        command2.CommandType = CommandType.StoredProcedure;
                        addParameters2(command2.Parameters);
                        command2.ExecuteNonQuery();


                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                readData(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                throw;
            }
        }











        public void ExecutePackageProcedure(string procedureName, Action<IDataParameterCollection> addParameters, Action<IDataReader, int> readData)
        {
            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    var command = new OracleCommand(procedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    addParameters(command.Parameters);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        int resultIndex = 0;
                        do
                        {
                            while (reader.Read())
                            {
                                readData(reader, resultIndex);
                            }
                            resultIndex++;
                        } while (reader.NextResult());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                throw;
            }
        }
        //for adding memorandum
        public void ExecuteNonQueryPackageProcedure(string procedureName, Action<IDataParameterCollection> addParameters, out int success, out int r_id)
        {
            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    var command = new OracleCommand(procedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    addParameters(command.Parameters);

                    connection.Open();
                    command.ExecuteNonQuery();

                    // Retrieve output parameter values
                    success = command.Parameters["PSUCCESS"].Value == DBNull.Value ? 0 : ((OracleDecimal)command.Parameters["PSUCCESS"].Value).ToInt32();
                    r_id = command.Parameters["P_ID"].Value == DBNull.Value ? 0 : ((OracleDecimal)command.Parameters["P_ID"].Value).ToInt32();
                    ;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                throw;
            }
        }
        public void ExecuteNonQueryProcedure(string procedureName, Action<IDataParameterCollection> addParameters, out int success)
        {
            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    var command = new OracleCommand(procedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    addParameters(command.Parameters);

                    connection.Open();
                    command.ExecuteNonQuery();

                    // Retrieve output parameter values
                    success = command.Parameters["PSUCCESS"].Value == DBNull.Value ? 0 : ((OracleDecimal)command.Parameters["PSUCCESS"].Value).ToInt32();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                throw;
            }
        }

        public string InsUpdDelByStr(string strCommand)
        {
            OracleCommand olInsUpdDelByStr = new OracleCommand();

            string ConStr = _connectionString;

            using (OracleConnection conn = new OracleConnection(ConStr))
            {

                OracleCommand oCommand = new OracleCommand();
                oCommand.Connection = conn;

                conn.Open();

                try
                {

                    oCommand.CommandText = strCommand;
                    oCommand.ExecuteNonQuery();
                    conn.Close();// new added
                    return "";
                }
                catch (OracleException ex)
                {
                    return ex.Message;
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();

                }
            }
        }



        public string InsUpdDelByStr_blob(string strCommand, byte[] blob1, byte[] blob2)
        {
            OracleCommand olInsUpdDelByStr = new OracleCommand();

            string ConStr = _connectionString;

            using (OracleConnection conn = new OracleConnection(ConStr))
            {

                OracleCommand oCommand = new OracleCommand();
                oCommand.Connection = conn;

                conn.Open();

                OracleParameter blobParameter1 = new OracleParameter();
                blobParameter1.OracleDbType = OracleDbType.Blob;


                blobParameter1.ParameterName = "BlobParameter1";
                blobParameter1.Value = blob1;

                OracleParameter blobParameter2 = new OracleParameter();
                blobParameter2.OracleDbType = OracleDbType.Blob;


                blobParameter2.ParameterName = "BlobParameter2";
                blobParameter2.Value = blob2;



                OracleTransaction sqlTran1 = conn.BeginTransaction();
                oCommand.Parameters.Add(blobParameter1);
                oCommand.Parameters.Add(blobParameter2);
                oCommand.Transaction = sqlTran1;

                //---------------------------------------------------------------------

                try
                {

                    oCommand.CommandText = strCommand;
                    oCommand.ExecuteNonQuery();
                    sqlTran1.Commit();
                    conn.Close();
                    return "";
                }
                catch (OracleException ex)
                {
                    return ex.Message;
                    //Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();

                }
            }
        }

        public void setConnectionString(LoginModel loginViewModel)
        {
            _connectionString = _dbAccessHelper.getConnectionString(loginViewModel);
        }

        public string ExecutePackageProcedure(string procedureName, Action<IDataParameterCollection> addParameters)
        {
            string VMESSAGE;
            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    var command = new OracleCommand(procedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    addParameters(command.Parameters);
                    connection.Open();
                    command.ExecuteNonQuery();
                    VMESSAGE = command.Parameters["V_MESSAGE"].Value.ToString();
                }
                return VMESSAGE;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                throw;
            }
        }


        public string ExecuteProcedureInPackage(string procedureName, Action<IDataParameterCollection> addParameters)
        {
            string VMESSAGE = "Fail";
            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {

                    var command = connection.CreateCommand();
                    command.CommandText = procedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    addParameters(command.Parameters);
                    connection.Open();
                    command.ExecuteNonQuery();
                    VMESSAGE = "sucesss";
                }
                return VMESSAGE;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                throw;
            }
        }

        public string ExecutePackageProcedure2(string procedureName, Action<IDataParameterCollection> addParameters)
        {
            string VMESSAGE = "success";
            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    var command = new OracleCommand(procedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    addParameters(command.Parameters);
                    connection.Open();
                    command.ExecuteNonQuery();
                    //VMESSAGE = command.Parameters["V_MESSAGE"].Value.ToString();
                    //VMESSAGE = command.Parameters["V_MESSAGE"].Value.ToString();
                }
                return VMESSAGE;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                throw;
            }
        }

    }
}
