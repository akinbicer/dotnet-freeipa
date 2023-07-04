using System.Net;
using System.Net.Http.Headers;
using System.Text;
using FreeIPA.DotNet.Models;
using FreeIPA.DotNet.Models.Login;
using FreeIPA.DotNet.Models.RPC;
using Newtonsoft.Json;

namespace FreeIPA.DotNet;

public class IpaClient : IDisposable
{
    private readonly HttpClient _client;

    public IpaClient(string baseUrl)
    {
        var handler = new HttpClientHandler();
        handler.CookieContainer = new CookieContainer();
        handler.UseCookies = true;
        handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

        _client = new HttpClient(handler);
        _client.BaseAddress = new Uri(baseUrl);
        _client.DefaultRequestHeaders.Referrer = new Uri($"{baseUrl}/ipa");
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public async Task<IpaResultModel<IpaLoginResponseModel>> LoginWithPassword(IpaLoginRequestModel model)
    {
        var formData = new List<KeyValuePair<string, string>>
        {
            new("user", model.Username),
            new("password", model.Password)
        };

        var content = new FormUrlEncodedContent(formData);
        var response = await _client.PostAsync("/ipa/session/login_password", content);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Failed to login. Content: {await response.Content.ReadAsStringAsync()}");

        return new IpaResultModel<IpaLoginResponseModel>
        {
            Success = response.IsSuccessStatusCode,
            Data = new IpaLoginResponseModel
            {
                Code = (int)response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            }
        };
    }

    public async Task<IpaResultModel<IpaRpcResponseModel>> SendRpcRequest(IpaRpcRequestModel model)
    {
        var requestData = new
        {
            method = model.Method,
            @params = model.Parameters,
            id = model.Id
        };

        var jsonRequest = JsonConvert.SerializeObject(requestData, Formatting.Indented);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/ipa/session/json", content);
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Something went wrong. Content: {response.Content.ReadAsStringAsync()}");

        return new IpaResultModel<IpaRpcResponseModel>
        {
            Success = response.IsSuccessStatusCode,
            Data = JsonConvert.DeserializeObject<IpaRpcResponseModel>(await response.Content.ReadAsStringAsync())
        };
    }
}