using System.Security.Claims;
using DBComm.Shared;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using WebAPI.DTOs;
using Microsoft.AspNetCore.Identity;

namespace WebAPI.Service;

[ApiController]
[Route("auth")]
[Authorize]

public class AuthController : ControllerBase
{
    private readonly IConfiguration config;
    private readonly IAccountLogic _accountLogic;

    public AuthController(IConfiguration config, IAccountLogic accountService)
    {
        this.config = config;
        this._accountLogic = accountService;
    }


    [HttpPost, Route("register"), AllowAnonymous]
    public async Task<ActionResult> Register([FromBody] UserLoginDto dto)
    {
        try
        {
            await _accountLogic.RegisterMember(dto.Username, dto.Password);
            return Ok("User registered.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
        
        
    }

    [HttpPost, Route("login"), AllowAnonymous]
    public async Task<ActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        try
        {
            Member? member = await _accountLogic.Login(userLoginDto.Username, userLoginDto.Password);
            string token = GenerateJwt(member);
            return Ok(token);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete, Route("delete/users/{username}")]
    public async Task<ActionResult> Delete([FromRoute]string username, [FromBody] UserGetterDTO dto)
    {
        try
        {
            await _accountLogic.Delete(username,dto.Password);
            return Ok("Account deleted.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }

    [HttpPut, Route("edit/{username}/username")]
    public async Task<ActionResult> EditUsername([FromRoute]string username, [FromBody] UsernameChangeDTO dto)
    {
        try
        {
            await _accountLogic.EditUsername(username, dto.NewUsername,dto.Password);
            return Ok("Username changed.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpPut, Route("edit/{username}/password")]
        public async Task<ActionResult> EditPassword([FromRoute]string username,[FromBody] PasswordChangeDTO dto)
        {
            try
            {
                await _accountLogic.EditPassword(username,dto.OldPassword, dto.NewPassword);
                return Ok("Password changed.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    [HttpPut, Route("edit/{usernames}/admin"), Authorize(Policy = "Admin")]
    public async Task<ActionResult> ToggleAdmin([FromRoute]string username, [FromBody] ToggleAdminDTO dto)
    {
        try
        {
            await _accountLogic.ToggleAdmin(dto.AdminUsername,dto.AdminPassword,dto.Username);
            return Ok("Username changed.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
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
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, config["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.Name, member.Username),
            new Claim(ClaimTypes.Role, member.IsAdmin ? "Admin" : "Member"),
            new Claim("HouseId", member.Home?.Id)
        };
        return claims.ToList();
    }

    [HttpPatch, Route("houses/{houseId}/members"), Authorize(Policy = "Admin")]
    public async Task<ActionResult> AddMemberToHouse([FromRoute]string houseId, [FromQuery] string username)
    {
        try
        {
            await _accountLogic.AddMemberToHouse(username, houseId);
            return Ok("Member added to house.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpPatch, Route("houses/members/{username}"), Authorize(Policy = "Admin")]
    public async Task<ActionResult> RemoveMemberFromHouse([FromRoute] string username)
    {
        try
        {
            await _accountLogic.RemoveMemberFromHouse(username);
            return Ok("Member removed.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
