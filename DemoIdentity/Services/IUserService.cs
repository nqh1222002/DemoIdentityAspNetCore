using DemoIdentity.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoIdentity.Services
{
    public interface IUserService
    {
        public Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
        public Task<UserManagerResponse> LogInUserAsync(LogInViewModel model);


    }
    public class UserService : IUserService
    {
        private UserManager<IdentityUser> _userManager { get; set; }

        private IConfiguration configuration { get; set; }

        public UserService(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            configuration = config;
        }


        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if(model == null)
                throw new NullReferenceException("Register model is null");

            if (model.Password != model.ConfirmPassword)
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the password",
                    IsSuccess = false,
                };

            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };
            
            var result = await _userManager.CreateAsync(identityUser,model.Password);

            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "Create new user successfully",
                    IsSuccess = true,
                };
            }
            return new UserManagerResponse
            {
                Message = "Cannot create new user",
                IsSuccess = false,
                Errors = result.Errors.Select(d => d.Description)
            };
        }

        public async Task<UserManagerResponse> LogInUserAsync(LogInViewModel model)
        {
            //check email exist
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                return new UserManagerResponse
                {
                    Message = "The user doesn't exist",
                    IsSuccess = false,
                };
            }
            //check password
            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
            {
                return new UserManagerResponse
                {
                    Message = "Invalid password",
                    IsSuccess = false,
                };
            }
            // claims
            var claims = new[]
            {
                new Claim("Email",model.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                /*issuer: configuration["ValidIssuer"],
                audience: configuration["ValidAudience"],*/
                claims: claims,
                expires:DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message = tokenAsString,
                IsSuccess = true,
                Expired = token.ValidTo
            };
        }
    }
}
