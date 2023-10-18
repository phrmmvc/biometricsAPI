using Biometrics.Helpers.Interface;
using Biometrics.Models;
using Biometrics.Request;
using Biometrics.Response;
using Oracle.ManagedDataAccess.Client;

namespace Biometrics.Repository
{
    public interface ILoginRepository
    {
        public void setConnectionString(LoginModel model);
        public List<LoginResponse> userExist(LoginRequest request);
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
    }
}
