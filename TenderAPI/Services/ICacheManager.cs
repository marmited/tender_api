namespace TenderAPI.Services;

public interface ICacheManager
{
    (bool success, T data) GetData<T>(string key) where T : class;
    void SetData(string key, object data);
}