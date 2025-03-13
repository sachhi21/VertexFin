using System;

namespace UserService.Domain.Models
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }

        public  required Role Role { get; set; }
        public required Permission Permission { get; set; }
    }
}
