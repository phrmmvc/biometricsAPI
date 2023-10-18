using Biometrics.Helpers.Interface;
using Biometrics.Models;
using Biometrics.Request;
using Biometrics.Response;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Biometrics.Repository
{
    public interface ILoginRepository
    {
        public void setConnectionString(LoginModel model);
        public List<LoginResponse> userExist(LoginRequest request);
        public DataTable getCurrentRoles(string userId);
        string getCredentials(string token);
        DataTable getDesignation(string employeeId);
        DataTable getUserBranch(string brnCod);
        DataTable getRmBr(string brCode);
        DataTable getSubBranch(string employeeId);
        DataTable getSubBranchInfo(string brCode, string agpCod);
        string addLoginInfo();
    }
    public class LoginRepository : ILoginRepository
    {
        private readonly IDbHelper _dbHelper;
        public LoginRepository(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public void setConnectionString(LoginModel model)
        {
            _dbHelper.setConnectionString(model);
        }

        public List<LoginResponse> userExist(LoginRequest request)
        {
            LoginModel model = new LoginModel();
            model.UserId = request.userId;
            model.Password = request.password;
            setConnectionString(model);
            bool isExists = false;
            string sql = "select * from tguser where upper(userid)=upper('" + request.userId + "') and  user_pass= pkg_app_context.encrypt('" + request.password.ToUpper() + "') and usrsts='A' ";

            string credentials = null;
            string userLevel = null;

            //--------------------------------------------------------------------------------------------------------------
            _dbHelper.ExecuteQuery(sql, reader =>
            {
                credentials = reader["USRNAM"].Equals(DBNull.Value) ? "" : reader["USRNAM"].ToString();
                userLevel = reader["LVLCOD"].Equals(DBNull.Value) ? "" : reader["LVLCOD"].ToString();
            }, (command) =>
            {
                command.Parameters.Add(new OracleParameter("USERID", model.UserId));
            });
            //credentials = "MANZURUL";   //-----need to delete-------
            //userLevel = "5";            //-----need to delete-------
            //--------------------------------------------------------------------------------------------------------------

            List<LoginResponse> userLevelList = null;
            if (credentials != null || credentials == "")
            {
                userLevelList = userLevelExist(userLevel);
                if (userLevelList != null)
                {
                    isExists = true;
                }
            }
            if (isExists)
            {
                return userLevelList;
            }
            return null;
        }

        public List<LoginResponse> userLevelExist(string userLevel)
        {

            string sql = "select db_user,pkg_app_context.decrypt(db_user_pass  ) db_user_pass from tgulvl where lvlcod='" + userLevel + "'";
            var loginViewModelList = new List<LoginResponse>();
            _dbHelper.ExecuteQuery(sql, reader =>
            {
                var loginViewModel = new LoginResponse
                {
                    UserLevelId = reader["DB_USER"].Equals(DBNull.Value) ? "" : reader["DB_USER"].ToString(),
                    LevelPassword = reader["DB_USER_PASS"].Equals(DBNull.Value) ? "" : reader["DB_USER_PASS"].ToString(),
                };
                loginViewModelList.Add(loginViewModel);
            });
            if (loginViewModelList != null)
            {
                return loginViewModelList;
            }

            return null;
        }

        public DataTable getCurrentRoles(string userId)
        {
            DataTable result = null;
            string sql = @" select * from V_USER_CURR_ROLE where upper(USERID)='" + userId.ToUpper() + "'  and (   power_type is null or  power_type not in (1) )";
            result = _dbHelper.GetData(sql);
            return result;
        }

        public string addLoginInfo()
        {
            //string str = "insert into hrm.HR_USER_LOG (ID, EMPLOYEE_ID, LOGIN_IP,  LOGIN_DATE_TIME  ) values (" + id + ",'" + UserID + "','" + Request.UserHostAddress + "',sysdate )";
            return "";
        }

        public string getCredentials(string token)
        {

            string sql = "SELECT USER_APT FROM  hrm.HR_APP_LINK_INFO where USER_SKT = :TOKEN";
            string credentials = null;
            //_dbHelper.ExecuteQuery(sql, reader =>
            //{
            //    credentials = reader["USER_APT"].Equals(DBNull.Value) ? "" : reader["USER_APT"].ToString();                   
            //}, (command) =>
            //{
            //    command.Parameters.Add(new OracleParameter("TOKEN", token));
            //});

            return credentials;
        }


        public DataTable getDesignation(string employeeId)
        {
            DataTable result = null;
            string dstr = @"Select '('||D.Dsg_Name||')' as  DESIG From Hr_Professional_History P
                Left Join Hr_Tdesignation D On(P.Tdesignation_Id=D.Id)
                where (to_char(P.Date_To)='01-JAN-00' or P.Date_To is null)  and p.employee_id='" + employeeId + @"'";
            result = _dbHelper.GetData(dstr);
            return result;
        }

        public DataTable getRmBr(string brCode)
        {
            DataTable result = null;
            var StrSql = @"Select BRCOD 
                               From V_Br_Ro_List
                               Where RO_PLACE_COD in (" + brCode.ToString() + ")";
            result = _dbHelper.GetData(StrSql);
            return result;
        }

        public DataTable getSubBranch(string employeeId)
        {
            DataTable result = null;
            string sub_br_query = @"select AGPCOD from hr_tposting where employee_id = '" + employeeId + @"' and active_status=1";
            result = _dbHelper.GetData(sub_br_query);
            return result;
        }

        public DataTable getSubBranchInfo(string brCode, string agpCod)
        {
            DataTable result = null;
            string q = @"select upper(agpname) as brnam,agpcod from tab_agent where associated_brcod ='" + brCode + @"' and 
                       agpcod='" + agpCod + "'";
            result = _dbHelper.GetData(q);
            return result;
        }

        public DataTable getUserBranch(string brnCod)
        {
            DataTable result = null;
            string StrSql1 = @" select * from  v_Tgbran where brcod in (" + brnCod + ")";
            result = _dbHelper.GetData(StrSql1);
            return result;
        }

    }
}
