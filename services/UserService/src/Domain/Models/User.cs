using System;


namespace UserService.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }  // Store hashed passwords
        public bool IsEmailVerified { get; set; }      // Email verification status
        public bool IsKYCCompleted { get; set; }  // KYC completion status

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Many-to-Many with Role
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // One-to-One
        public KYC? KYC { get; set; }
        public UserProfile? UserProfile { get; set; }

    }
}