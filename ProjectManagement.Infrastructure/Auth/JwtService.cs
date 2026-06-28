namespace ProjectManagement.Infrastructure.Auth
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        public JwtService(
            IOptions<JwtSettings> jwtSettings,
            UserManager<ApplicationUser> userManager)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
        }


        public async Task<string> GenerateAccessTokenAsync(ApplicationUser user, CancellationToken ct = default)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<System.Security.Claims.Claim>();

            foreach (var role in userRoles)
            {
                roleClaims.Add(new System.Security.Claims.Claim(ClaimTypes.Role, role));
            }

            var claims = new List<System.Security.Claims.Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Exp, _jwtSettings.ExpiryMinutes.ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email!)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId, CancellationToken ct = default)
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            return Task.FromResult(
                new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    UserId = userId
                });
        }
    }
}
