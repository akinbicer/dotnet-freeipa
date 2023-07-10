using Newtonsoft.Json;

namespace FreeIPA.DotNet.Dtos.RPC;

public class IpaRpcResponseDto
{
    [JsonProperty("result")] public object? Result { get; set; }
    [JsonProperty("error")] public IpaRpcErrorDto? Error { get; set; }
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("principal")] public string? Principal { get; set; }
    [JsonProperty("version")] public string? Version { get; set; }
}