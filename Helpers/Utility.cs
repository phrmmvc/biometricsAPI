using Biometrics.Helpers.Interface;

namespace Biometrics.Helpers
{
    public class Utility : IUtility
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDbHelper _dbHelper;
        public Utility(IHttpContextAccessor contextAccessor, IDbHelper dbHelper)
        {
            _contextAccessor = contextAccessor;
            _dbHelper = dbHelper;
        }
        public string insert_string(string tablename, Dictionary<string, string> dic)
        {

            string p = "";
            string q = "";
            foreach (var item in dic)
            {
                p += item.Key + ",";

                if (item.Value.ToString().Contains(":BlobParameter1") || item.Value.ToString().Contains(":BlobParameter2") || item.Value.ToString().Contains("to_date") || item.Value.ToString().Contains("sysdate") || item.Value.ToString().Contains("session") || item.Value.ToString().Contains("PKG_APP_CONTEXT.encrypt"))
                {
                    q += "" + item.Value.ToString().Trim() + ",";
                }
                else
                {
                    q += "'" + item.Value.ToString().Trim() + "',";
                }
            }
            p = p.Trim(',');
            q = q.Trim(',');
            string strUpdSql = "insert into " + tablename + " (" + p + ") values (" + q + ")";
            return strUpdSql;

        }

        public string update_string(string tablename, Dictionary<string, string> dic, string where)
        {

            string q = "";
            foreach (var item in dic)
            {


                if (item.Value.ToString().Contains("to_date") || item.Value.ToString().Contains("sysdate") || item.Value.ToString().Contains("session") || item.Value.ToString().Contains("PKG_APP_CONTEXT.encrypt"))
                {
                    q += item.Key + "=" + item.Value.ToString().Trim() + ",";
                }
                else
                {
                    q += item.Key + "='" + item.Value.ToString().Trim() + "',";
                }
            }

            q = q.Trim(',');
            string strUpdSql = "update  " + tablename + " set  " + q + " where " + where;
            return strUpdSql;

        }

        public string Session(string key)
        {
            return _contextAccessor.HttpContext.Session.GetString(key);
        }

        public string GetMACAddress()
        {
            string sql = " select CF_IP from DUAL ";
            var ipAddress = "";
            _dbHelper.ExecuteQuery(sql, reader =>
            {
                ipAddress = reader["CF_IP"] == DBNull.Value ? "" : reader["CF_IP"].ToString();
            });

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }

            return ipAddress;

        }
    }
}
