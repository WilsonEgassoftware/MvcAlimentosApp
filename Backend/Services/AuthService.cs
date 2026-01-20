using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using SupermarketAPI.DTOs;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IEmailService emailService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null) return null;

            // Verificar contraseña
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return null;

            // Permitir login pero indicar si el email no está confirmado
            // (Los admins siempre pueden entrar sin verificación)

            // Actualizar último login
            user.LastLogin = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            // Generar token JWT
            var token = GenerateJwtToken(user);

            return new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Role = user.Role,
                Email = user.Email,
                UserId = user.Id,
                IsEmailConfirmed = user.IsEmailConfirmed,
                Message = user.Role != "Admin" && !user.IsEmailConfirmed 
                    ? "Por favor verifica tu email para completar la activación de tu cuenta" 
                    : null
            };
        }

        public async Task<LoginResponse?> RegisterAsync(RegisterRequest request)
        {
            // Validar que el username y email no existan
            if (await _userRepository.UsernameExistsAsync(request.Username))
                return null;

            if (await _userRepository.EmailExistsAsync(request.Email))
                return null;

            // Generar token de verificación
            var verificationToken = GenerateVerificationToken();
            var tokenExpiry = DateTime.UtcNow.AddHours(24); // Token válido por 24 horas

            // Crear nuevo usuario (siempre como "User" a menos que sea creado manualmente)
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "User", // Siempre "User" para nuevos registros
                FullName = request.FullName,
                IsEmailConfirmed = false,
                EmailVerificationToken = verificationToken,
                EmailVerificationTokenExpiry = tokenExpiry,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.CreateAsync(user);

            // Enviar email de verificación
            try
            {
                await _emailService.SendVerificationEmailAsync(createdUser.Email, createdUser.Username, verificationToken);
            }
            catch (Exception ex)
            {
                // Log el error pero no fallar el registro
                // En producción, podrías querer manejar esto de manera diferente
                Console.WriteLine($"Error al enviar email de verificación: {ex.Message}");
            }

            // NO generar token JWT aquí - el usuario necesita verificar su email primero
            // Retornar respuesta sin token o con un flag indicando que necesita verificar
            return new LoginResponse
            {
                Token = null, // Sin token hasta verificar email
                Username = createdUser.Username,
                Role = createdUser.Role,
                Email = createdUser.Email,
                UserId = createdUser.Id,
                IsEmailConfirmed = false,
                Message = "Por favor verifica tu email para activar tu cuenta"
            };
        }

        public async Task<bool> VerifyEmailAsync(string token)
        {
            var user = await _userRepository.GetByVerificationTokenAsync(token);
            if (user == null)
                return false;

            // Verificar que el token no haya expirado
            if (user.EmailVerificationTokenExpiry.HasValue && user.EmailVerificationTokenExpiry.Value < DateTime.UtcNow)
                return false;

            // Activar cuenta
            user.IsEmailConfirmed = true;
            user.EmailVerificationToken = null;
            user.EmailVerificationTokenExpiry = null;

            await _userRepository.UpdateAsync(user);
            return true;
        }

        private string GenerateVerificationToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyForJWTTokenGenerationThatShouldBeAtLeast32CharactersLong";
            var issuer = jwtSettings["Issuer"] ?? "SupermarketAPI";
            var audience = jwtSettings["Audience"] ?? "SupermarketClient";
            var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
