using Newtonsoft.Json;

namespace FreeIPA.DotNet.Dtos.User.Create;

public class IpaCreateUserResponseDto
{
    [JsonProperty("result")] public Result? Result { get; set; }
    [JsonProperty("value")] public string? Value { get; set; }
    [JsonProperty("messages")] public List<Message>? Messages { get; set; }
    [JsonProperty("summary")] public string? Summary { get; set; }
}

public class Data
{
    [JsonProperty("server_version")] public string? ServerVersion { get; set; }
}

public class KerberosExtraData
{
    [JsonProperty("__base64__")] public string? Base64 { get; set; }
}

public class Message
{
    [JsonProperty("type")] public string? Type { get; set; }
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("message")] public string? Messages { get; set; }
    [JsonProperty("code")] public int? Code { get; set; }
    [JsonProperty("data")] public Data? Data { get; set; }
}

public class Result
{
    [JsonProperty("objectclass")] public List<string>? ObjectClass { get; set; }
    [JsonProperty("givenname")] public List<string>? GivenName { get; set; }
    [JsonProperty("mail")] public List<string>? MailAddress { get; set; }
    [JsonProperty("sn")] public List<string>? Sn { get; set; }
    [JsonProperty("ipauniqueid")] public List<string>? IpaUniqueId { get; set; }
    [JsonProperty("uid")] public List<string>? UniqueId { get; set; }
    [JsonProperty("krbcanonicalname")] public List<string>? KerberosCanonicalName { get; set; }
    [JsonProperty("displayname")] public List<string>? DisplayName { get; set; }
    [JsonProperty("loginshell")] public List<string>? LoginShell { get; set; }
    [JsonProperty("krbprincipalname")] public List<string>? KerberosPrincipalName { get; set; }
    [JsonProperty("cn")] public List<string>? Cn { get; set; }
    [JsonProperty("homedirectory")] public List<string>? HomeDirectory { get; set; }
    [JsonProperty("ipauserauthtype")] public List<string>? IpaUserAuthType { get; set; }
    [JsonProperty("gecos")] public List<string>? Gecos { get; set; }
    [JsonProperty("uidnumber")] public List<string>? UniqueIdNumber { get; set; }
    [JsonProperty("initials")] public List<string>? Initials { get; set; }
    [JsonProperty("gidnumber")] public List<string>? GidNumber { get; set; }

    [JsonProperty("krbpasswordexpiration")]
    public List<string>? KerberosPasswordExpiration { get; set; }

    [JsonProperty("krblastpwdchange")] public List<string>? KerberosLastPwdChange { get; set; }
    [JsonProperty("krbextradata")] public List<KerberosExtraData>? KerberosExtraData { get; set; }
    [JsonProperty("mepmanagedentry")] public List<string>? MepManagedEntry { get; set; }
    [JsonProperty("has_password")] public bool? HasPassword { get; set; }
    [JsonProperty("has_keytab")] public bool? HasKeytab { get; set; }
    [JsonProperty("memberof_group")] public List<string>? MemberOfGroup { get; set; }
    [JsonProperty("memberof_subid")] public List<string>? MemberOfSubid { get; set; }
    [JsonProperty("dn")] public string? Dn { get; set; }
}