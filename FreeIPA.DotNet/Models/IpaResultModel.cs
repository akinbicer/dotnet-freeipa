namespace FreeIPA.DotNet.Models;

public class IpaResultModel<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
}