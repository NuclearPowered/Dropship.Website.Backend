using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dropship.Website.Backend.Database;
using Dropship.Website.Backend.Database.Entities;
using Dropship.Website.Backend.Models.Requests.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace Dropship.Website.Backend.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;
        private readonly DatabaseContext _database;

        public AuthService(DatabaseContext database, IConfiguration config)
        {
            _database = database;
            _config = config;
        }

        public Task<bool> UsernameExistsAsync(string username)
        {
            return _database.Users.AnyAsync(x => x.Username == username);
        }

        public Task<bool> EmailExistsAsync(string email)
        {
            return _database.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<UserEntity> AuthenticateAsync(AuthenticateRequest request)
        {
            var user = await _database.Users.AsNoTracking().FirstOrDefaultAsync(x =>
                x.Username == request.Username ||
                x.Email == request.Username);

            if (user == null) return null;

            if (!BC.Verify(request.Password, user.Password)) return null;

            return user;
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            var user = new UserEntity
            {
                Username = request.Username,
                Email = request.Email,
                Password = BC.HashPassword(request.Password),
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            await _database.AddAsync(user);
            await _database.SaveChangesAsync();
        }
        
        public async ValueTask ChangePasswordAsync(int userId, string password)
        {
            var user = await _database.Users.FirstOrDefaultAsync(u => u.Id == userId);
            user.Password = BC.HashPassword(password);

            _database.Users.Update(user);
            await _database.SaveChangesAsync();
        }

        public string GenerateJwt(UserEntity user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserEntity> GetUserById(int userId)
        {
            return await _database.Users.AsNoTracking()
                .Include(u => u.Mods)
                .Include(u => u.Plugins)
                .Include(u => u.Servers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}