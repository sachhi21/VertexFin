using System;


namespace UserService.Domain.Models
{
    public class KYC
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }  // Foreign Key to User
        public required string DocumentType { get; set; }  // Passport, ID, etc.
        public required string DocumentNumber { get; set; }
        public string? DocumentImageUrl { get; set; }
        public bool IsApproved { get; set; } = false;  // KYC approval status
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public required User User { get; set; }

    }
}