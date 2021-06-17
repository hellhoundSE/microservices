using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserMicroservice.Models.DTO {
    public class CreateRoleDTO {
        public int IdRole { get; set; }
        public string Name { get; set; }
    }
}
