using Microsoft.Extensions.Logging;

namespace InsightJournal.Services
{
    public class PasswordService
    {
        private readonly ILogger<PasswordService> _logger;
        private const string PasswordKey = "AppPassword";
        private const string PasswordEnabledKey = "PasswordEnabled";

        public PasswordService(ILogger<PasswordService> logger)
        {
            _logger = logger;
        }

        // Check if password protection is enabled
        public bool IsPasswordEnabled()
        {
            return Preferences.Get(PasswordEnabledKey, false);
        }

        // Enable or disable password protection
        public void SetPasswordEnabled(bool enabled)
        {
            Preferences.Set(PasswordEnabledKey, enabled);
            _logger.LogInformation("Password protection {Status}", enabled ? "enabled" : "disabled");
        }

        // Check if a password is set
        public bool HasPassword()
        {
            return !string.IsNullOrEmpty(Preferences.Get(PasswordKey, string.Empty));
        }

        // Set a new password (hashed with BCrypt)
        public bool SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("Attempted to set empty password");
                return false;
            }

            try
            {
                // Hash the password using BCrypt with default work factor (11)
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                Preferences.Set(PasswordKey, hashedPassword);
                _logger.LogInformation("Password set successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting password");
                return false;
            }
        }

        // Verify password using BCrypt
        public bool VerifyPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            try
            {
                string storedHash = Preferences.Get(PasswordKey, string.Empty);
                if (string.IsNullOrEmpty(storedHash))
                {
                    return false;
                }

                // Verify password using BCrypt
                return BCrypt.Net.BCrypt.Verify(password, storedHash);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying password");
                return false;
            }
        }

        // Change password (requires old password verification)
        public bool ChangePassword(string oldPassword, string newPassword)
        {
            if (!VerifyPassword(oldPassword))
            {
                _logger.LogWarning("Failed password change attempt - incorrect old password");
                return false;
            }

            return SetPassword(newPassword);
        }

        // Remove password
        public void RemovePassword()
        {
            Preferences.Remove(PasswordKey);
            Preferences.Remove(PasswordEnabledKey);
            _logger.LogInformation("Password removed");
        }
    }
}