namespace FreeIPA.DotNet.Dtos.Login;

public class IpaLoginRequestDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}