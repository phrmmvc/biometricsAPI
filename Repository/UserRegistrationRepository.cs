using Biometrics.Helpers.Interface;
using Biometrics.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biometrics.Repository
{
    public interface IUserRegistrationRepository
    {
        public void setConnectionString(string userLevelId, string userLevelPassword);

        public List<SelectListItem> getAllZone(string znCod, string userLevelCode, string userLevelId, string userLevelPassword);
        List<SelectListItem> getAllBranchByZoneCode(string znCod, string brCod, string userLevelCode, string userLevelId, string userLevelPassword);
        List<SelectListItem> getAllSubBranchByBranchCode(string znCod, string brCod, string subBrCod, string userLevelCode, string userLevelId, string userLevelPassword);
    }
    public class UserRegistrationRepository : IUserRegistrationRepository
    {
        private readonly IDbHelper _dbHelper;
        public UserRegistrationRepository(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<SelectListItem> getAllBranchByZoneCode(string znCod, string brCod, string userLevelCode, string userLevelId, string userLevelPassword)
        {
            setConnectionString(userLevelId, userLevelPassword);
            var branchList = new List<SelectListItem>();
            string sql = "SELECT BRCOD,BRNAM FROM V_TGBRAN where zncod='" + znCod + "'";
            if (userLevelCode == "5" || userLevelCode == "67" || userLevelCode == "99" || userLevelCode == "6")
            {
                sql += " and BRCOD = '" + brCod + "' ";
            }
            else
            {
                var titleModel = new SelectListItem
                {
                    Text = "0",
                    Value = "--All Branch--",
                };

                branchList.Add(titleModel);
            }
            sql += " order by BRNAM";
            try
            {
                _dbHelper.ExecuteQuery(sql, reader =>
                {
                    var titleModel = new SelectListItem
                    {
                        Text = reader.GetString(0),
                        Value = reader.GetString(1),
                    };

                    branchList.Add(titleModel);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return branchList;
        }

        public List<SelectListItem> getAllSubBranchByBranchCode(string znCod, string brCod, string subBrCod, string userLevelCode, string userLevelId, string userLevelPassword)
        {
            setConnectionString(userLevelId, userLevelPassword);
            string sql = "select agpcod brcod,agpname||'('||agpcod||')' as brnam  from  tab_agent where  1 = 1";
            if (brCod != "")
            {
                sql += " and associated_brcod = '" + brCod + "'";
            }
            if (subBrCod != ""&&subBrCod!="0")
            {
                sql += " and agpcod = '" + subBrCod + "' ";
            }
            var branchList = new List<SelectListItem>();
            _dbHelper.ExecuteQuery(sql, reader =>
            {
                var titleModel = new SelectListItem
                {
                    Text = reader.GetString(0),
                    Value = reader.GetString(1),
                };

                branchList.Add(titleModel);
            });
            if (branchList.Count == 0 || userLevelCode == "5")
            {
                var emptyList = new List<SelectListItem>();
                return emptyList;
            }
            else if (userLevelCode != "6" && userLevelCode != "67")
            {
                var titleModel = new SelectListItem
                {
                    Text = "0",
                    Value = "--Please Select a Sub-Branch--",
                };
                branchList.Insert(0, titleModel);
            }
            return branchList;
        }

        public List<SelectListItem> getAllZone(string znCod, string userLevelCode, string userLevelId, string userLevelPassword)
        {
            setConnectionString(userLevelId, userLevelPassword);

            string sql = "SELECT ZNCOD,ZNNAM FROM CBSMS.TGZONE where 1 = 1 ";
            if (userLevelCode == "5" || userLevelCode == "67" || userLevelCode == "99" || userLevelCode == "9" || userLevelCode == "6")
            {
                sql += " and ZNCOD = " + znCod;
            }

            sql += " ORDER BY ZNNAM";

            var zoneList = new List<SelectListItem>();
            _dbHelper.ExecuteQuery(sql, reader =>
            {
                var titleModel = new SelectListItem
                {
                    Text = reader.GetString(0),
                    Value = reader.GetString(1),
                };

                zoneList.Add(titleModel);
            });

            return zoneList;
        }

        public void setConnectionString(string userLevelId, string userLevelPassword)
        {

            LoginModel loginModel = new LoginModel();
            loginModel.UserLevelId = userLevelId;
            loginModel.LevelPassword = userLevelPassword;
            _dbHelper.setConnectionString(loginModel);
        }

    }
}
