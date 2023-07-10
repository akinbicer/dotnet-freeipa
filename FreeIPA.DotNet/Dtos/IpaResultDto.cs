using FreeIPA.DotNet.Dtos.RPC;

namespace FreeIPA.DotNet.Dtos;

public class IpaResultDto<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}