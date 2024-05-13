using System.Security.Claims;
using DBComm.Shared;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Service;
[ApiController]
[Route("[controller]")]

public class AuthController : ControllerBase
{
    private readonly IConfiguration config;
    private readonly IAccountLogic _accountLogic;
 
    public AuthController(IConfiguration config, IAccountLogic accountService)
    {
        this.config = config;
        this._accountLogic = accountService;
    }
    
    
    [HttpPost, Route("members/register")]
    public async Task<ActionResult> Register([FromQuery] string username, [FromQuery] string password)
    {
        try
        {
            await _accountLogic.RegisterMember(username, password);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
        
    }
    
    [HttpPost, Route("admins/register")]
    public async Task<ActionResult> RegisterAdmin([FromQuery] string username, [FromQuery] string password)
    {
        await _accountLogic.RegisterAdmin(username, password);
        return Ok();
    }

    [HttpPatch, Route("members/remove")]
    public async Task<ActionResult> RemoveMemberFromHouse([FromQuery] string username, [FromQuery] string houseId)
    {
        try
        {
            await _accountLogic.RemoveMemberFromHouse(username, houseId);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    private string GenerateJwt(Member member)
    {
        List<Claim> claims = GenerateClaims(member);
        
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        SigningCredentials signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        
        JwtHeader header = new JwtHeader(signIn);
        
        JwtPayload payload = new JwtPayload(
            config["Jwt:Issuer"],
            config["Jwt:Audience"],
            claims, 
            null,
            DateTime.UtcNow.AddMinutes(60));
        
        JwtSecurityToken token = new JwtSecurityToken(header, payload);
        
        string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
        return serializedToken;
    }

    private List<Claim> GenerateClaims(Member member)
    {
        ///change claims as you like
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, config["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            /*
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("DisplayName", user.Name),
            new Claim("Email", user.Email),
            new Claim("Age", user.Age.ToString()),
            new Claim("Domain", user.Domain),
            new Claim("SecurityLevel", user.SecurityLevel.ToString())*/
        };
        return claims.ToList();
    }
}