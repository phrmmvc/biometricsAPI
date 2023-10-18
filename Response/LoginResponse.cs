using Biometrics.Models;

namespace Biometrics.Response
{
    public class LoginResponse
    {
        public bool IsPersistent
        {
            get; set;
        }
        public Role Role
        {
            get; set;
        }

        public string UserLevelId
        {
            get; set;
        }
        public string LevelPassword
        {
            get; set;
        }
        public string userRoleCode
        {
            get; set;
        }
        public string userName
        {
            get; set;
        }
        public string empUserId
        {
            get; set;
        }
        public string empId
        {
            get; set;
        }
        public string hBrCod
        {
            get; set;
        }
        public string branName
        {
            get; set;
        }
        public string znCod
        {
            get; set;
        }
        public string userLevelCode
        {
            get; set;
        }
        public string rmBrCod
        {
            get; set;
        }
        public string subBrCod
        {
            get; set;
        }
        public string token
        {
            get; set;
        }
    }
}
