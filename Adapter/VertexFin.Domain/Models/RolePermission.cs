using System;

namespace VertexFin.Domain.Models
{
    public class RolePermission : BaseEntity
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }

        public  required Role Role { get; set; }
        public required Permission Permission { get; set; }
    }
}
