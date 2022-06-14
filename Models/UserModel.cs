using System.Text.Json.Serialization;

namespace example.AspnetCoreIdentity.StoragePlugin.Models;

public class UserModel
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("uname")]
    public string? UserName { get; set; }

    [JsonPropertyName("pw")]
    public string? Password { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }
}
