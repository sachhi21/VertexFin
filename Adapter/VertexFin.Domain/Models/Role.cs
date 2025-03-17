using System;


namespace VertexFin.Domain.Models
{
    public class Role : BaseEntity
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }  // Admin, User, Manager, etc.
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}