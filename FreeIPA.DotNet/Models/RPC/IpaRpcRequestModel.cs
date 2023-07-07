using Newtonsoft.Json;

namespace FreeIPA.DotNet.Models.RPC;

public class IpaRpcRequestModel
{
    [JsonProperty("id")] public required int Id { get; set; }
    [JsonProperty("method")] public required string Method { get; set; }
    [JsonProperty("params")] public required object[] Parameters { get; set; }
    [JsonProperty("version")] public required string Version { get; set; }
}