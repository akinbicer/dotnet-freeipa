using FreeIPA.DotNet;
using FreeIPA.DotNet.Models.Login;
using FreeIPA.DotNet.Models.RPC;

using (var ipaClient = new IpaClient("https://ipa.tazmanyak.local"))
{
    var loginResult = await ipaClient.LoginWithPassword(new IpaLoginRequestModel
    {
        Username = "admin",
        Password = "Aq123456"
    });

    if (loginResult.Success)
    {
        var result = await ipaClient.SendRpcRequest(new IpaRpcRequestModel
        {
            Id = 0,
            Method = "user_show/1",
            Parameters = new object[]
            {
                new object[]
                {
                    "akin.bicer"
                },
                new
                {
                    version = "2.251"
                }
            }
        });
    }
}

Console.ReadLine();