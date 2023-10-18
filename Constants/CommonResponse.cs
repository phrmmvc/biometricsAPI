namespace Biometrics.Constants
{
    public class CommonResponse<T>
    {
        public T Data
        {
            get; set;
        }
        public bool IsSuccess
        {
            get; set;
        }
        public string ErrorMessage
        {
            get; set;
        }
        public Dictionary<string, object> Meta { get; } = new Dictionary<string, object>();
    }

}
