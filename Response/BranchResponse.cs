using Biometrics.Models;

namespace Biometrics.Response
{
    public class BranchResponse
    {
        
        public string brCod
        {
            get; set;
        }
        public string brName
        {
            get; set;
        }
       public BranchResponse(string brCod, string brNam)
        {
            this.brCod = brCod;
            this.brName = brNam;
        }
    }
}
