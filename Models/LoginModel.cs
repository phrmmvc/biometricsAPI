namespace Biometrics.Models
{
    public class LoginModel
    {
        public string UserId
        {
            get; set;
        }
        public string Password
        {
            get; set;
        }
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
    }
}
