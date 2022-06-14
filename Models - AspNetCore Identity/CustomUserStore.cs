
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using example.AspnetCoreIdentity.StoragePlugin.DataAccessLayer;

namespace example.AspnetCoreIdentity.StoragePlugin.IdentityStore.Models;

// @NOTE : Implement Custom UserStore (and RoleStore)
//         (by implementing IUserStore<TUser>, and optional interfaces for additional stuffs)
//         (gets/sets data from datastore)
public class CustomUserStore
                        :   IUserStore<IdentityUser>          // mandatory interface
                          , IUserPasswordStore<IdentityUser>  // and implement optional interfaces as per requirement
                          , IUserClaimStore<IdentityUser> 
{
    private IdentityDAL _dal;
    private static int _idCounter = 1;

    public CustomUserStore()
    {
        _dal = new IdentityDAL();
    }

    public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if(user == null) throw new ArgumentNullException(nameof(user));
        user.Id = _idCounter++.ToString();

        if(_dal.Create(user))
            return Task.FromResult(IdentityResult.Success);
        else
            return Task.FromResult<IdentityResult>(IdentityResult.Failed());
    }

    public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if(user == null) throw new ArgumentNullException(nameof(user));

        if(_dal.Delete(user))
            return Task.FromResult<IdentityResult>(IdentityResult.Success);
        else
            return Task.FromResult<IdentityResult>(IdentityResult.Failed());
    }

    public Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if(userId == null) throw new ArgumentNullException(nameof(userId));

        return Task.FromResult<IdentityUser>(_dal.FindById(userId));
    }

    public Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if(normalizedUserName == null) throw new ArgumentNullException(nameof(normalizedUserName));

        return Task.FromResult<IdentityUser>(_dal.FindByName(normalizedUserName));
    }

    public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if(user == null) throw new ArgumentNullException(nameof(user));

        return Task.FromResult<string>(user.NormalizedUserName);
    }

    public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if(user == null) throw new ArgumentNullException(nameof(user));

        return Task.FromResult<string>(user.Id);
    }

    public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if(user == null) throw new ArgumentNullException(nameof(user));

        return Task.FromResult<string>(user.UserName);
    }

    public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if(user == null) throw new ArgumentNullException(nameof(user));

        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if(user == null) throw new ArgumentNullException(nameof(user));

        user.UserName = userName;
        return Task.CompletedTask;
    }

    public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if(user == null) throw new ArgumentNullException(nameof(user));

        if(_dal.Update(user))
            return Task.FromResult<IdentityResult>(IdentityResult.Success);
        else
            return Task.FromResult<IdentityResult>(IdentityResult.Failed());
    }
    
    public Task AddClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if(user == null) throw new ArgumentNullException(nameof(user));
        if(claims == null) throw new ArgumentNullException(nameof(claims));

        return _dal.AddClaimsAsync(user, claims);
    }    

    public Task<IList<Claim>> GetClaimsAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if(user == null) throw new ArgumentNullException(nameof(user));

        return _dal.GetClaimsAsync(user);
    }    

    public Task<IList<IdentityUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if(claim == null) throw new ArgumentNullException(nameof(claim));

        return _dal.GetUsersForClaimAsync(claim);
    }

    public Task RemoveClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if(user == null) throw new ArgumentNullException(nameof(user));
        if(claims == null) throw new ArgumentNullException(nameof(claims));

        return _dal.RemoveClaimsAsync(user, claims);
    }

    public Task ReplaceClaimAsync(IdentityUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if(user == null) throw new ArgumentNullException(nameof(user));
        if(claim == null) throw new ArgumentNullException(nameof(claim));
        if(newClaim == null) throw new ArgumentNullException(nameof(newClaim));

        return _dal.ReplaceClaimAsync(user, claim, newClaim);
    }   

    public Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (passwordHash == null) throw new ArgumentNullException(nameof(passwordHash));

        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null) throw new ArgumentNullException(nameof(user));

        return Task.FromResult<string>(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null) throw new ArgumentNullException(nameof(user));

        return Task.FromResult<bool>(!string.IsNullOrEmpty(user.PasswordHash));
    }

    public void Dispose()
    {
        _dal.Dispose();
    }
}