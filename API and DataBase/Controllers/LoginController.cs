﻿using API_and_DataBase.DTO;
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
            if (user != null)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my_sercret_key_123456"));

                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var data = new List<Claim>();
                data.Add(new Claim("username", user.UserName));
                data.Add(new Claim("type", user.Type.ToString()));

                var token = new JwtSecurityToken(
                claims: data,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    userName = user.UserName,
                    type = user.Type,
                });
            }
            else
            {
                return Unauthorized();
            }

        }
    }
}

