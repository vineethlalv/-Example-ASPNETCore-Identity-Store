using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using example.AspnetCoreIdentity.StoragePlugin.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace example.AspnetCoreIdentity.StoragePlugin;

[ApiController]
[Route("[controller]")]
public class UserManagementController : ControllerBase
{    
    // @NOTE : UserManager<TUser> for managing the Identity Store
    //         SignInManager<TUser> for managing the user Sign-in 
    private UserManager<IdentityUser> _userMan;
    private SignInManager<IdentityUser> _signInMan;
    private IPasswordHasher<IdentityUser> _pwHasher;
    public UserManagementController(UserManager<IdentityUser> userManager
                                    , SignInManager<IdentityUser> signInManager
                                    , IPasswordHasher<IdentityUser> pwHasher)
    {
        _userMan = userManager;
        _signInMan = signInManager;
        _pwHasher = pwHasher;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserModel user)
    {
        IdentityUser identityUser = new IdentityUser(){
            UserName = user.Name,
            Email = user.Email            
        };
        identityUser.PasswordHash = _pwHasher.HashPassword(identityUser, user.Password);

        IdentityResult reult = await _userMan.CreateAsync(identityUser);
        if (reult.Succeeded)
            return Ok();
        return BadRequest(reult.Errors.Select(i => i.Description).Aggregate((a, b) => a + "," + b));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserModel user)
    {        
        SignInResult result =  await _signInMan.PasswordSignInAsync(user.Name, user.Password, false, false);
        if(result.Succeeded)
            return Ok();
        else
            return Forbid();
    }

    [HttpDelete("user")]
    public IActionResult Delete([FromBody] UserModel user)
    {
        return Ok();
    }

    [HttpPost("claim")]
    public IActionResult AddClaim([FromBody] ClaimModel claim)
    {
        return Ok();
    }

    [HttpDelete("claim")]
    public IActionResult DeleteClaim([FromBody] ClaimModel claim)
    {
        return Ok();
    }
}
