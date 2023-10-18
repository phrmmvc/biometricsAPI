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
        public string token
        {
            get; set;
        }
    }
}
