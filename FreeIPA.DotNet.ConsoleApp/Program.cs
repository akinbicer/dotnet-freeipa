using FreeIPA.DotNet;
using FreeIPA.DotNet.Dtos.Login;
using FreeIPA.DotNet.Dtos.RPC;

using var client = new IpaClient("https://ipa.calismaortamim.local");
var loginResult = await client.LoginWithPassword(new IpaLoginRequestDto
{
    Username = "admin",
    Password = "CalismaOrtamim2023"
});

var result = await client.SendRpcRequest(new IpaRpcRequestDto
{
    Id = 0,
    Method = "user_find",
    Parameters = new object[]
    {
        Array.Empty<string>(), new
        {
        }
    },
    Version = "2.251"
});

Console.ReadLine();