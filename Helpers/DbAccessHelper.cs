using Biometrics.Helpers.Interface;
using Biometrics.Models;

namespace Biometrics.Helpers
{
    public class DbAccessHelper : IDbAccessHelper
    {
        private readonly IConfiguration _configuration;

        public DbAccessHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string getConnectionString(LoginModel loginViewModel)
        {
            string a = _configuration.GetValue<string>("dbString");
            string con = "";
            if (a.ToLower() == "rnd")
            {
                //con = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = 172.16.1.116)(PORT = 1528)))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = dbpblrnd)));" +
                //       " User Id = " + "HRMUSR" + "; Password = " + "HRM20230820" + "; ";

                if (loginViewModel.UserLevelId != null)
                {
                    con = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = 172.16.1.116)(PORT = 1528)))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = dbpblrnd)));" +
                       " User Id = " + loginViewModel.UserLevelId + "; Password = " + loginViewModel.LevelPassword + "; ";
                }
                else
                {
                    con = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = 172.16.1.116)(PORT = 1528)))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = dbpblrnd)));" +
                       " User Id = " + "HRMUSR" + "; Password = " + "HRM20230820" + "; ";
                    //" User Id = " + loginViewModel.UserId + "; Password = " + loginViewModel.Password + "; ";
                }
            }
            else if (a.ToLower() == "dc")
            {
                // con = @"User Id=" + loginViewModel.UserId + @";Password=" + loginViewModel.Password + @";
                //Data Source=(DESCRIPTION =  (ADDRESS = (PROTOCOL = TCP)(HOST = pbldc01-scan.pubalibank.com)(PORT = 1527))
                //(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = dbpbldc)));";


                con = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = pbldc01-scan.pubalibank.com)(PORT = 1527)))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = dbpbldc)));" +
                    " User Id = " + loginViewModel.UserId + "; Password = " + loginViewModel.Password + "; ";


            }
            else if (a.ToLower() == "dr")
            {
                // con = @"User Id=" + loginViewModel.UserId + @";Password=" + loginViewModel.Password + @";
                // Data Source=(DESCRIPTION =  (ADDRESS = (PROTOCOL = TCP)(HOST = 172.16.2.109)(PORT = 1527))
                //(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = dbpbldr)));";

                con = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = 172.16.2.109)(PORT = 1527)))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = dbpbldr)));" +
                " User Id = " + loginViewModel.UserId + "; Password = " + loginViewModel.Password + "; ";
            }
            else if (a.ToLower() == "lab")
            {
                con = @"User Id=" + loginViewModel.UserId + @";Password=" + loginViewModel.Password + @";
                Data Source=(DESCRIPTION =  (ADDRESS = (PROTOCOL = TCP)(HOST = pblrnd-scan.pubalibank.com)(PORT = 1528))
               (CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = dbpbllab)));";
            }
            return con;
        }
    }
}
