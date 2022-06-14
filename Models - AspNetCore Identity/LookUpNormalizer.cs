using Microsoft.AspNetCore.Identity;

namespace example.AspnetCoreIdentity.StoragePlugin.IdentityStore.Models;

// @NOTE: Normalize names so that "Alice" and "alice" are treated same
public class LookUpNormalizer : ILookupNormalizer
{
    public string NormalizeEmail(string email)
    {
        return email.ToLowerInvariant();
    }

    public string NormalizeName(string name)
    {
        return name.ToLowerInvariant();
    }
}

