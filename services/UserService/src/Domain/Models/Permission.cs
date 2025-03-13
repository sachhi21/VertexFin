using System;


namespace UserService.Domain.Models
{
    public class Permission
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }  // View Users, Edit Transactions, etc.
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}