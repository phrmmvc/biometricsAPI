using Biometrics.Models;

namespace Biometrics.Response
{
    public class ZoneResponse
    {
        
        public string znCod
        {
            get; set;
        }
        public string znName
        {
            get; set;
        }
       public ZoneResponse(string znCod, string znName)
        {
            this.znCod = znCod;
            this.znName = znName;
        }
    }
}
