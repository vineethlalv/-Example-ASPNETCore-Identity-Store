using System.Text.Json.Serialization;

namespace example.AspnetCoreIdentity.StoragePlugin.Models;

public class ClaimModel
{
    [JsonPropertyName("userId")]
    public string UserId { get; set; }

    [JsonPropertyName("type")]
    public string ClaimType { get; set; }

    [JsonPropertyName("value")]
    public string ClaimValue { get; set; }
}