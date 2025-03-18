using System;


namespace VertexFin.Domain.Models
{
    public class UserRole : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public required User User { get; set; }
        public required Role Role { get; set; }
    }
}