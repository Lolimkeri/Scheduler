using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using MySchedulerWork.Models;
using MySchedulerWork.Data;

namespace MySchedulerWork.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public const string ISSUER = "IssuerHere";
        public const string AUDIENCE = "AudienceHere";
        public const string KEY = "MysuperSecretKeykey12345!!";
        public const int LIFETIME = 60;

        private readonly MyAppContext _context;

        public AuthorizationService(MyAppContext context)
        {
            _context = context;
        }
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }

        public List<User> GetUsers()
        {
            var users = _context.Users.ToList();

            return users;
        }

        public void AddUser(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
        }

        public string GetToken(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return null;
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: ISSUER,
                    audience: AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(LIFETIME)),
                    signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public ClaimsIdentity GetIdentity(string username, string password)
        {
            User user = _context.Users.FirstOrDefault(x => x.UserName == username && x.Password == password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}
