using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DW.EnterpriseAPI.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DW.EnterpriseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signManager;

        public AuthController(UserManager<ApplicationUser> userManager,
                               SignInManager<ApplicationUser> signManager)
        {
            _userManager = userManager;
            _signManager = signManager;
        }

        [Route("CreateUser")]
        [HttpPost]
        public async Task<ActionResult> CreateUser(
                    [FromBody] UserInfo model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return BuildToken(model);
                }
                else
                {
                    return BadRequest("usurio / Clave Invalida");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserInfo model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signManager.PasswordSignInAsync(
                    model.Email, model.Email, true, false);
                if (result.Succeeded)
                {
                    return BuildToken(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                                        "Login Invalid, Verifique!!");
                    return BadRequest();
                }
            }
            else
                return BadRequest("usuario/clave invalida!");
        }

        private ActionResult BuildToken(UserInfo user)
        {
            // creamos los claims
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Email ),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("DigitalWareClaim","Esto es propiedad de Digital Ware"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
            };
            // TODO: Realizar la clase que lee variables de ambiente
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            //            _configuration["PrivateKeyAPI"]));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
               "Cartagena Barranquilla Monteria Bogota Cali Medellin Pasto Leticia"));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer:"www.digitalware.com.co",
                audience:"www.digitalware.com.co",
                claims: claims,
                expires: expiration,
                signingCredentials: cred);

            return Ok(new { 
             token = new JwtSecurityTokenHandler().WriteToken(token),
             expiration= expiration
            } );
        }
    }
}