namespace SurveyBasket.DAL.IRepository;

public interface IUserRepository
{
    // Queries
    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<ApplicationUser?> FindByIdAsync(string userId);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> EmailExistsForOtherUserAsync(string email, string excludedUserId, CancellationToken cancellationToken);
    Task<IList<string>> GetUserPermissionsAsync(IList<string> userRoles, CancellationToken cancellationToken);
    Task<IList<string>> GetRolesAsync(ApplicationUser user);
    Task<List<ApplicationUser>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<List<UserWithRoles>> GetUsersWithRolesAsync(CancellationToken cancellationToken);

    // User lifecycle
    Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
    Task<IdentityResult> UpdateAsync(ApplicationUser user);
    Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
    Task<IdentityResult> AddToRolesAsync(ApplicationUser user, IEnumerable<string> roles);
    Task DeleteUserRolesAsync(string userId, CancellationToken cancellationToken);
    Task<IdentityResult> SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset? lockoutEnd);
    Task UpdateUserProfileAsync(string userId, string firstName, string lastName);

    // Email confirmation
    Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
    Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string code);

    // Password reset
    Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
    Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string code, string newPassword);

    // Password change
    Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
}