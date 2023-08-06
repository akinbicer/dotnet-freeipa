using System.Net;
using System.Net.Http.Headers;
using System.Text;
using FreeIPA.DotNet.Constants;
using FreeIPA.DotNet.Dtos;
using FreeIPA.DotNet.Dtos.Login;
using FreeIPA.DotNet.Dtos.RPC;
using FreeIPA.DotNet.Dtos.User.Create;
using Newtonsoft.Json;

namespace FreeIPA.DotNet;

public class IpaClient : IDisposable
{
    private readonly HttpClient _client;
    public string? BaseUrl { get; set; }

    public IpaClient()
    {
        var handler = new HttpClientHandler();
        handler.CookieContainer = new CookieContainer();
        handler.UseCookies = true;
        handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

        _client = new HttpClient(handler);
        if (BaseUrl != null)
        {
            _client.BaseAddress = new Uri(BaseUrl);
            _client.DefaultRequestHeaders.Referrer = new Uri($"{BaseUrl}/ipa");
        }

        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
    }

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

    public async Task<IpaResultDto<IpaLoginResponseDto>> LoginWithPassword(IpaLoginRequestDto dto)
    {
        var formData = new List<KeyValuePair<string, string>>
        {
            new("user", dto.Username),
            new("password", dto.Password)
        };

        var content = new FormUrlEncodedContent(formData);
        var response = await _client.PostAsync("/ipa/session/login_password", content);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Failed to login. Content: {await response.Content.ReadAsStringAsync()}");

        return new IpaResultDto<IpaLoginResponseDto>
        {
            Success = response.IsSuccessStatusCode,
            Data = new IpaLoginResponseDto
            {
                Code = (int)response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            }
        };
    }

    public async Task<IpaResultDto<IpaRpcResponseDto>> SendRpcRequest(IpaRpcRequestDto dto)
    {
        var requestData = new
        {
            id = dto.Id,
            method = dto.Method,
            @params = dto.Parameters,
            version = "2.251"
        };

        var jsonRequest = JsonConvert.SerializeObject(requestData, Formatting.Indented);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/ipa/session/json", content);
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Something went wrong. Content: {response.Content.ReadAsStringAsync()}");

        var result = JsonConvert.DeserializeObject<IpaRpcResponseDto>(await response.Content.ReadAsStringAsync());

        return new IpaResultDto<IpaRpcResponseDto>
        {
            Success = result?.Error == null,
            Message = result?.Error == null ? CustomResponseMessage.TransactionSuccess : CustomResponseMessage.TransactionError,
            Data = result
        };
    }
}