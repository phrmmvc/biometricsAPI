using Biometrics.Request;
using Biometrics.Response;

namespace Biometrics.Repository
{
    public interface ILoginService
    {
        public List<LoginResponse> isUserExist(LoginRequest request);
    }
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _repository;

        public LoginService(ILoginRepository repository)
        {
            _repository = repository;
        }

        public List<LoginResponse> isUserExist(LoginRequest request)
        {
            return _repository.userExist(request);
        }
    }
}
