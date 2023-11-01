using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biometrics.Repository
{
    public interface IUserRegistrationService
    {
        List<SelectListItem> getAllBranchByZoneCode(string znCod, string brCod, string userLevelCode, string userLevelId, string userLevelPassword);
        List<SelectListItem> getAllEmployeeByBranchReportWithTerminal(string znCod, string brCod, string subBrCod, string userLevelCode, string userLevelId, string userLevelPassword, string empId);
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

        public List<SelectListItem> getAllEmployeeByBranchReportWithTerminal(string znCod, string brCod, string subBrCod, string userLevelCode, string userLevelId, string userLevelPassword, string empId)
        {
            return _repository.getAllEmployeeByBranchReportWithTerminal(znCod, brCod, subBrCod, userLevelCode, userLevelId, userLevelPassword,empId);
        }
    }
}
