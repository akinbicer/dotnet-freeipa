namespace FreeIPA.DotNet.Models.Login;

public class IpaLoginRequestModel
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}