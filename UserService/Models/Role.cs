using System;
using System.Collections.Generic;
using UserMicroservice.Models.DTO;

#nullable disable

namespace UserService
{
    public partial class Role
    {
        public Role(CreateRoleDTO roleDTO)
        {
            IdRole = roleDTO.IdRole;
            Name = roleDTO.Name;
            Users = new HashSet<User>();
        }

        public Role() {
            Users = new HashSet<User>();
        }

        public int IdRole { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
