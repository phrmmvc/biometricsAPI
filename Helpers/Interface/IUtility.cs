namespace Biometrics.Helpers.Interface
{
    public interface IUtility
    {
        string update_string(string tablename, Dictionary<string, string> dic, string where);
        string insert_string(string tablename, Dictionary<string, string> dic);
        string Session(string keyName);
        string GetMACAddress();

    }
}
