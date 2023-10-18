using Biometrics.Models;

namespace Biometrics.Helpers.Interface
{
    public interface IDbAccessHelper
    {
        string getConnectionString(LoginModel loginViewModel);
    }
}
