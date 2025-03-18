using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;


namespace VertexFin.Domain.Models
{
    public class User : BaseEntity
    {
        [StringLength(250)]
        public required string Username { get; set; }

        public  string? Email { get; set; }
        public required string PasswordHash { get; set; }  // Store hashed passwords
        public bool IsEmailVerified { get; set; } = false;    // Email verification status
        public bool IsKYCCompleted { get; set; } = false; // KYC completion status

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Many-to-Many with Role
        public ICollection<UserRole>? UserRoles { get; set; } = new List<UserRole>();

        // One-to-One
        public KYC? KYC { get; set; }
        public UserProfile? UserProfile { get; set; }

    }
}