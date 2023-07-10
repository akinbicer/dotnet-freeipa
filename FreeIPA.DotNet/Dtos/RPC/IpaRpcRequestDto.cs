using Newtonsoft.Json;

namespace FreeIPA.DotNet.Dtos.RPC;

public class IpaRpcRequestDto
{
    [JsonProperty("id")] public required int Id { get; set; }
    [JsonProperty("method")] public required string Method { get; set; }
    [JsonProperty("params")] public required object[] Parameters { get; set; }
    [JsonProperty("version")] public required string Version { get; set; }
}