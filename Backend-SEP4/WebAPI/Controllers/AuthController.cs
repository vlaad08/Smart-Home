using System.Security.Claims;
using DBComm.Shared;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebAPI.DTOs;
using Microsoft.AspNetCore.Identity;

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


    [HttpPost, Route("register")]
    public async Task<ActionResult> Register([FromQuery] string username, [FromQuery] string password)
    {
        try
        {
            await _accountLogic.RegisterMember(username, password);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
        
        
    }

    [HttpPost, Route("login")]
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

    [HttpDelete, Route("delete")]
    public async Task<ActionResult> Delete([FromBody] UserGetterDTO dto)
    {
        try
        {
            await _accountLogic.Delete(dto.Username,dto.Password);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }

    [HttpPut, Route("edit/username")]
    public async Task<ActionResult> EditUsername([FromBody] UsernameChangeDTO dto)
    {
        try
        {
            await _accountLogic.EditUsername(dto.OldUsername, dto.NewUsername,dto.Password);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpPut, Route("edit/password")]
        public async Task<ActionResult> EditPassword([FromBody] PasswordChangeDTO dto)
        {
            try
            {
                await _accountLogic.EditPassword(dto.Username,dto.OldPassword, dto.NewPassword);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    [HttpPut, Route("edit/admin")]
    public async Task<ActionResult> ToggleAdmin([FromBody] ToggleAdminDTO dto)
    {
        try
        {
            await _accountLogic.ToggleAdmin(dto.AdminUsername,dto.AdminPassword,dto.Username);
            return Ok();
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
            new Claim(ClaimTypes.Role, member.IsAdmin ? "Admin" : "User")
            new Claim("HouseId", member.Home?.Id)
        };
        return claims.ToList();
    }

    [HttpPatch, Route("members/add")]
    public async Task<ActionResult> AddMemberToHouse([FromQuery] string username, [FromQuery] string houseId)
    {
        try
        {
            await _accountLogic.AddMemberToHouse(username, houseId);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpPatch, Route("members/remove")]
    public async Task<ActionResult> RemoveMemberFromHouse([FromQuery] string username)
    {
        try
        {
            await _accountLogic.RemoveMemberFromHouse(username);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
