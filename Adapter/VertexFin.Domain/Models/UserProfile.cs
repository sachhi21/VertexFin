using System;

namespace VertexFin.Domain.Models
{
    public class UserProfile : BaseEntity
    {
        public Guid UserId { get; set; }  // Foreign Key to User
        public required string FirstName { get; set; }
        public  required string LastName { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public required string PhoneNumber { get; set; }

        public required User User { get; set; }

    }
}