using Biometrics.Models;
using System.Data;

namespace Biometrics.Helpers.Interface
{
    public interface IDbHelper
    {
        void ExecuteNonQuery(string sql, Action<IDbCommand> addParameters);
        T ExecuteScalar<T>(string sql, Action<IDbCommand> addParameters);
        void ExecuteQuery(string sql, Action<IDataReader> readData, Action<IDbCommand> addParameters = null);
        void ExecuteQueryWithProcedure(string sql, Action<IDataReader> readData, Action<IDataParameterCollection> addParameters1, Action<IDataParameterCollection> addParameters2);
        void ExecutePackageProcedure(string procedureName, Action<IDataParameterCollection> addParameters, Action<IDataReader, int> readData);
        void ExecuteNonQueryPackageProcedure(string procedureName, Action<IDataParameterCollection> addParameters, out int success, out int r_id);
        void ExecuteNonQueryProcedure(string procedureName, Action<IDataParameterCollection> addParameters, out int success);
        string InsUpdDelByStr(string strCommand);
        string InsUpdDelByStr_blob(string strCommand, byte[] thumbLeft, byte[] thumbRight);
        DataTable GetData(string sql);
        void setConnectionString(LoginModel loginViewModel);

        string ExecutePackageProcedure(string procedureName, Action<IDataParameterCollection> addParameters);
        string ExecutePackageProcedure2(string procedureName, Action<IDataParameterCollection> addParameters);

        public string ExecuteProcedureInPackage(string procedureName, Action<IDataParameterCollection> addParameters);

    }
}
