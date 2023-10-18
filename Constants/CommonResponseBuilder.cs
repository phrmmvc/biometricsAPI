namespace Biometrics.Constants
{
    public class CommonResponseBuilder<T>
    {
        private T _data;
        private bool _isSuccess;
        private string _errorMessage;
        private readonly Dictionary<string, object> _meta = new Dictionary<string, object>();
        public CommonResponseBuilder(T data)
        {
            _data = data;
            _isSuccess = true;
        }

        public CommonResponseBuilder<T> SetSuccess(bool isSuccess)
        {
            _isSuccess = isSuccess;
            return this;
        }

        public CommonResponseBuilder<T> SetErrorMessage(string errorMessage)
        {
            _errorMessage = errorMessage;
            return this;
        }

        public CommonResponseBuilder<T> AddMeta(string key, object value)
        {
            _meta[key] = value;
            return this;
        }

        public CommonResponse<T> Build()
        {
            var response = new CommonResponse<T>
            {
                Data = _data,
                IsSuccess = _isSuccess,
                ErrorMessage = _errorMessage
            };

            foreach (var kvp in _meta)
            {
                response.Meta[kvp.Key] = kvp.Value;
            }

            return response;
        }
    }
}
