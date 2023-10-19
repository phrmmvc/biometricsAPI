using Biometrics.Request;
using Biometrics.Response;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Biometrics.Repository
{
    public interface IUserRegistrationService
    {
        List<SelectListItem> getAllBranchByZoneCode(string znCod, string brCod, string userLevelCode, string userLevelId, string userLevelPassword);
        List<SelectListItem> getAllSubBranchByBranchCode(string znCod, string brCod, string subBrCod, string userLevelCode, string userLevelId, string userLevelPassword);
        List<SelectListItem> getAllZone(string znCod, string userLevelCode, string userLevelId, string userLevelPassword);

    }
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IUserRegistrationRepository _repository;

        public UserRegistrationService(IUserRegistrationRepository repository)
        {
            _repository = repository;
        }

        public List<SelectListItem> getAllZone(string znCod, string userLevelCode, string userLevelId, string userLevelPassword)
        {
            return _repository.getAllZone(znCod, userLevelCode, userLevelId, userLevelPassword);
        }
        public List<SelectListItem> getAllBranchByZoneCode(string znCod, string brCod, string userLevelCode, string userLevelId, string userLevelPassword)
        {
            return _repository.getAllBranchByZoneCode(znCod, brCod, userLevelCode, userLevelId, userLevelPassword);
        }

        public List<SelectListItem> getAllSubBranchByBranchCode(string znCod, string brCod, string subBrCod, string userLevelCode, string userLevelId, string userLevelPassword)
        {
            return _repository.getAllSubBranchByBranchCode(znCod, brCod,subBrCod, userLevelCode, userLevelId, userLevelPassword);
        }
    }
}
