using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace example.AspnetCoreIdentity.StoragePlugin.DataAccessLayer;

public class IdentityDAL : IDisposable
{   
    private List<IdentityUser> _users;
    private Dictionary<string, List<Claim>> _claimsDict;

    public IdentityDAL()
    {
        _users = new List<IdentityUser>();
        _claimsDict = new Dictionary<string, List<Claim>>();
    }

    public bool Create(IdentityUser user)
    {
        if(user.UserName == null)
            return false;

        _users.Add(user);
        return true;
    }

    public bool Update(IdentityUser user)
    {
        IdentityUser storeUser = _users.Find(x => x.Id == user.Id);
        if (storeUser == null)
            return false;

        storeUser.UserName = user.UserName;
        storeUser.NormalizedUserName = user.NormalizedUserName;
        storeUser.Email = user.Email;

        return true;
    }

    public bool Delete(IdentityUser user)
    {
        return _users.RemoveAll(x => x.Id == user.Id) > 0;        
    }

    public IdentityUser FindById(string userId)
    {
        return _users.Find(x => x.Id == userId);
    }

    public IdentityUser FindByName(string normalizedUserName)
    {
        return _users.Find(x => x.NormalizedUserName == normalizedUserName);
    }

    public Task AddClaimsAsync(IdentityUser user, IEnumerable<Claim> claims)
    {
        if(_claimsDict.ContainsKey(user.Id))
                _claimsDict.Add(user.Id, new List<Claim>());
            
        _claimsDict[user.Id].AddRange(claims);

        return Task.CompletedTask;
    }

    public Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
    {
        List<Claim> userClaims;
        if(_claimsDict.ContainsKey(user.Id))
            userClaims = _claimsDict[user.Id];
        else 
            userClaims = new List<Claim>();

        return Task.FromResult<IList<Claim>>(userClaims);
    }

    public Task<IList<IdentityUser>> GetUsersForClaimAsync(Claim claim)
    {
        #nullable disable
        List<IdentityUser> claimUsers = new List<IdentityUser>();
        foreach(var claims in _claimsDict)
        {
            if(claims.Value.Find(x => x.Type == claim.Type && x.Value == claim.Value) != null)
                claimUsers.Add(_users.Find(x => x.Id == claims.Key));
        }
        #nullable restore

        return Task.FromResult<IList<IdentityUser>>(claimUsers);
    }

    public Task RemoveClaimsAsync(IdentityUser user, IEnumerable<Claim> claims)
    {
        if(_claimsDict.ContainsKey(user.Id))
        {
            foreach (var claim in claims)
            _claimsDict[user.Id].RemoveAll(x => x.Type == claim.Type && x.Value == claim.Value);
        }

        return Task.CompletedTask;
    }

    public Task ReplaceClaimAsync(IdentityUser user, Claim claim, Claim newClaim)
    {
        if(_claimsDict.ContainsKey(user.Id))
        {
            _claimsDict[user.Id].RemoveAll(x => x.Type == claim.Type && x.Value == claim.Value);
            _claimsDict[user.Id].Add(newClaim);
        }

        return Task.CompletedTask;
    }

    public void Dispose() {  }
}