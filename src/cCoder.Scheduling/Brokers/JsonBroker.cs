using cCoder.Data.Extensions;
using Newtonsoft.Json;


namespace cCoder.Scheduling.Brokers;

public interface IJsonBroker
{
    object ParseJson(string json);
    T ParseJson<T>(string json);
    string Serialize(object value);
}

public class JsonBroker : IJsonBroker
{
    public object ParseJson(string json) => JsonConvert.DeserializeObject(json, ObjectExtensions.GetJSONSettings());

    public T ParseJson<T>(string json) => JsonConvert.DeserializeObject<T>(json, ObjectExtensions.GetJSONSettings());

    public string Serialize(object value) => JsonConvert.SerializeObject(value, ObjectExtensions.GetJSONSettings());
}


