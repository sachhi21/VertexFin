using System;


namespace UserService.Domain.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }  // Admin, User, Manager, etc.
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}