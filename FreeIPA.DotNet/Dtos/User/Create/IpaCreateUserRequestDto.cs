namespace FreeIPA.DotNet.Dtos.User.Create;

public class IpaCreateUserRequestDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}