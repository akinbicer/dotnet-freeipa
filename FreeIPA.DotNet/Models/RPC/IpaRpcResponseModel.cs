using Newtonsoft.Json;

namespace FreeIPA.DotNet.Models.RPC;

public class IpaRpcResponseModel
{
    [JsonProperty("result")] public object? Result { get; set; }

    [JsonProperty("error")] public object? Error { get; set; }

    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("principal")] public string? Principal { get; set; }

    [JsonProperty("version")] public string? Version { get; set; }
}