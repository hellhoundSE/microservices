using System;
using System.Collections.Generic;
using UserMicroservice.Models.DTO;

#nullable disable

namespace UserService
{
    public partial class User
    {
        public User(CreateUserDTO userDTO) {
            IdUser = userDTO.IdUser;
            Surname = userDTO.Surname;
            Name = userDTO.Name;
            Email = userDTO.Email;
            IsReceiveNotifications = userDTO.IsReceiveNotifications;
            IdRole = userDTO.IdRole;
            RegistrationDate = DateTime.Now; 
        }

        public User() {

        }

        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsReceiveNotifications { get; set; }
        public int IdRole { get; set; }

        public virtual Role IdRoleNavigation { get; set; }
    }
}
