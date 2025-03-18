﻿using System;


namespace VertexFin.Domain.Models
{
    public class Permission : BaseEntity
    {
        public required string Name { get; set; }  // View Users, Edit Transactions, etc.
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}