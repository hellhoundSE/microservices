using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserMicroservice.Models.DTO {
    public class CreateUserDTO {

        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public bool IsReceiveNotifications { get; set; }
        public int IdRole { get; set; }

    }
}
