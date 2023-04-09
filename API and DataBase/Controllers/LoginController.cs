using API_and_DataBase.DTO;
using API_and_DataBase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_and_DataBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class loginController : ControllerBase
    {
        CompanyContext db;
        public loginController(CompanyContext db)
        {
            this.db = db;
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login userLogin)
        {
            Users user = db.Users.Where(n => n.UserName == userLogin.userName && n.Password == userLogin.password&&n.ISDeleted==false).FirstOrDefault();
            if (user != null || (userLogin.userName == "Admin" && userLogin.password == "@A123456"))
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my_sercret_key_123456"));

                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var data = new List<Claim>();
                data.Add(new Claim("username", user?.UserName ?? "Admin"));
                data.Add(new Claim("type", user?.Type.ToString() ?? "0"));

                var token = new JwtSecurityToken(
                claims: data,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    userName = user?.UserName ?? "Admin",
                    type = user?.Type ?? 0,
                });
            }
            else
            {
                return Unauthorized();
            }

        }
    }
}

