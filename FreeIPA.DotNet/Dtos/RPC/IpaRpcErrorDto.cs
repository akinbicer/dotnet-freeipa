using Newtonsoft.Json;

namespace FreeIPA.DotNet.Dtos.RPC;

public class IpaRpcErrorDto
{
    [JsonProperty("code")] public int? Code { get; set; }
    [JsonProperty("message")] public string? Message { get; set; }
    [JsonProperty("data")] public Data? Data { get; set; }
    [JsonProperty("name")] public string? Name { get; set; }
}

public class Data
{
    [JsonProperty("name")] public string? Name { get; set; }
}