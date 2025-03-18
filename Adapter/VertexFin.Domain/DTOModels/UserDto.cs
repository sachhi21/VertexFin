using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VertexFin.Common.Library.Helper;

namespace VertexFin.Domain.DTOModels
{
    public class UserDto
    {
        public Guid Id { get; set; } = GuidV7.NewGuid();
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
