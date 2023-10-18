using Biometrics.Request;
using Biometrics.Response;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Biometrics.Repository
{
    public interface ILoginService
    {
        public List<LoginResponse> isUserExist(LoginRequest request);
        public DataTable getUserCurrRole(string userId);

        string getUserCredentials(string token);
        string getEncryptString(string strPlainText);
        string getDecryptString(string EncryptText);
        string Encrypt_QueryString(string id);
        string Decrypt_QueryString(string str);
        DataTable getUserDesignation(string employeeId);
        DataTable getUserBranch(string brnCod);
        DataTable getRmBrCode(string brCode);
        DataTable getSubBranch(string employeeId);
        DataTable getSubBranchInfo(string brCode, string agpCod);
        string addLoginInfo();
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


        public DataTable getUserCurrRole(string userId)
        {
            return _repository.getCurrentRoles(userId);
        }

        public string getEncryptString(string strPlainText)
        {
            string strEncrypt = "";

            string passPhrase = "abu67pulockxX(^@!yh98766";
            string saltValue = "asdfzxcv98761234";
            string hashAlgorithm = "SHA1";
            int passwordIterations = 2;
            string initVector = "ASDFGHJKL#25DFGZ"; // must be 16 bytes
            int keySize = 192;
            strEncrypt = EncryptPass(strPlainText, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize).Trim();
            return strEncrypt;
        }
        public string Encrypt_QueryString(string str)
        {
            string EncrptKey = "2018;[pnuLIT)WebCodeExpertpulock";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byKey = Encoding.UTF8.GetBytes(EncrptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(str.Trim());
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
        public string Decrypt_QueryString(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace(" ", "+");
                string DecryptKey = "2018;[pnuLIT)WebCodeExpertpulock";
                byte[] byKey = { };
                byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
                byte[] inputByteArray = new byte[str.Length];

                byKey = Encoding.UTF8.GetBytes(DecryptKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(str.Trim());
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            else
            {
                return "";
            }
        }
        public string getDecryptString(string EncryptText)
        {
            string strEncrypt = "";
            string passPhrase = "abu67pulockxX(^@!yh98766";
            string saltValue = "asdfzxcv98761234";
            string hashAlgorithm = "SHA1";
            int passwordIterations = 2;
            string initVector = "ASDFGHJKL#25DFGZ"; // must be 16 bytes
            int keySize = 192;
            strEncrypt = DecryptPass(EncryptText, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize).Trim();
            return strEncrypt;
        }

        public string EncryptPass(string plainText,
                                      string passPhrase,
                                      string saltValue,
                                      string hashAlgorithm,
                                      int passwordIterations,
                                      string initVector,
                                      int keySize)
        {

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = password.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string cipherText = Convert.ToBase64String(cipherTextBytes);
            return cipherText;
        }

        public string DecryptPass(string cipherText,
                                  string passPhrase,
                                  string saltValue,
                                  string hashAlgorithm,
                                  int passwordIterations,
                                  string initVector,
                                  int keySize)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase,
saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = password.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            return plainText;
        }
        public string getUserCredentials(string token)
        {
            return _repository.getCredentials(token);
        }


        public DataTable getUserDesignation(string employeeId)
        {
            return _repository.getDesignation(employeeId);
        }

        public DataTable getUserBranch(string brnCod)
        {
            return _repository.getUserBranch(brnCod);
        }

        public DataTable getRmBrCode(string brCode)
        {
            return _repository.getRmBr(brCode);
        }

        public DataTable getSubBranch(string employeeId)
        {
            return _repository.getSubBranch(employeeId);
        }

        public DataTable getSubBranchInfo(string brCode, string agpCod)
        {
            return _repository.getSubBranchInfo(brCode, agpCod);
        }

        public string addLoginInfo()
        {
            return _repository.addLoginInfo();
        }

    }
}
