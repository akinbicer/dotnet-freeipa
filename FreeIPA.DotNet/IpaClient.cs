using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using FreeIPA.DotNet.Constants;
using FreeIPA.DotNet.Models;
using FreeIPA.DotNet.Models.Login;
using FreeIPA.DotNet.Models.RPC;
using FreeIPA.DotNet.Models.User;
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
            id = model.Id,
            method = model.Method,
            @params = model.Parameters,
            version = "2.251"
        };

        var jsonRequest = JsonConvert.SerializeObject(requestData, Formatting.Indented);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/ipa/session/json", content);
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Something went wrong. Content: {response.Content.ReadAsStringAsync()}");

        var result = JsonConvert.DeserializeObject<IpaRpcResponseModel>(await response.Content.ReadAsStringAsync());

        return new IpaResultModel<IpaRpcResponseModel>
        {
            Success = result?.Error == null,
            Message = result?.Error == null ? CustomResponseMessage.TransactionSuccess : CustomResponseMessage.TransactionError,
            Data = result
        };
    }

    public async Task<IpaResultModel<IpaCreateUserResponseModel>> CreateUser(IpaCreateUserRequestModel model)
    {
        var requestModel = new IpaRpcRequestModel
        {
            Id = 0,
            Method = "user_add",
            Parameters = new object[]
            {
                Array.Empty<string>(), new
                {
                    ipauserauthtype = "password",
                    givenname = model.FirstName,
                    sn = model.LastName,
                    cn = $"{model.FirstName} {model.LastName}",
                    uid = model.Username,
                    userpassword = model.Password
                }
            },
            Version = "2.251"
        };

        var data = await SendRpcRequest(requestModel);
        if (data.Success)
        {
            return new IpaResultModel<IpaCreateUserResponseModel>()
            {
                Success = true,
                Message = CustomResponseMessage.RecordAdded,
                Data = JsonConvert.DeserializeObject<IpaCreateUserResponseModel>(data!.Data!.Result!.ToString()!)
            };
        }

        return new IpaResultModel<IpaCreateUserResponseModel>
        {
            Success = false, 
            Message = CustomResponseMessage.TransactionError,
            Data = null
        };
    }

    public async Task<IpaResultModel<IpaRpcResponseModel>> DeleteUser(string username)
    {
        var requestModel = new IpaRpcRequestModel
        {
            Id = 0,
            Method = "user_del",
            Parameters = new object[]
            {
                Array.Empty<string>(), new
                {
                    uid = username,
                }
            },
            Version = "2.251"
        };

        var result = await SendRpcRequest(requestModel);
        return result;
    }
}