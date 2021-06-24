using EmailMictoservice.DTO;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserMicroservice.Models.DTO;
using UserService.Models.Exceptions;

namespace UserService {

    public class UserService{

        private readonly UserDbContext _context;
        private readonly IBus _bus;
        public UserService(UserDbContext context, IBus bus) {
            _context = context;
            _bus = bus;
        }

        #region CRUD
        public async Task<List<User>> GetAllAdmins() {
            return await _context.Users
                .Include(u => u.IdRoleNavigation)
                .Where(u => u.IdRoleNavigation.Name
                .ToLower() == "admin")
                .ToListAsync();
        }
        public async Task<List<string>> GetUsers(bool toEveryone) {
            if(toEveryone)
                return await _context.Users
                    .Where(u => u.IsReceiveNotifications)
                    .Select(u => u.Email)
                    .ToListAsync();

            return await _context.Users
                .Include(u => u.IdRoleNavigation)
                .Where(u => u.IdRoleNavigation.Name.ToLower() == "admin" && u.IsReceiveNotifications)
                .Select(u => u.Email)
                .ToListAsync();
        }

        public async Task<User> GetUserById(int idUser) {
            var user = await _context.Users.SingleOrDefaultAsync(user => user.IdUser == idUser);
            if (user is null)
                throw new NotFoundException($"user with id {idUser} not found");
            return user;
        }

        public async Task<User> CreateUser(CreateUserDTO userDTO) {

            if (_context.Users.Any(u => u.IdUser == userDTO.IdUser))
                throw new BadRequestException("User with given id already exists");

            User user = new User(userDTO);
            _context.Add(user);
            await _context.SaveChangesAsync();

            InformUsers("User Created!", $"Newby user {user.Name} {user.Surname} join our team", false);


            return user;
        }
        public async Task<User> UpdateUser(int idUser, User user) {

            if (idUser != user.IdUser)
                throw new BadRequestException("You can't change userId");

            var usetrToUpdate = await _context.Users.SingleOrDefaultAsync(user => user.IdUser == idUser);

            if (usetrToUpdate is null)
                throw new NotFoundException($"User with id {idUser} not found");

            if(!_context.Roles.Any(role => role.IdRole == user.IdRole)){
                throw new NotFoundException($"Role with id {user.IdRole} not found");
            }

            _context.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> DeleteUserById(int idUser) {
            var user = await _context.Users.SingleOrDefaultAsync(user => user.IdUser == idUser);
            if (user is null)
                throw new NotFoundException($"User with id {idUser} not found");

            InformUsers("User Removed!", $"User {user.Name} {user.Surname} left out service", false);

            _context.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }
        #endregion
        #region Roles
        internal async Task<User> SetUserRole(int idUser, int idRole) {

            var user = await _context.Users.SingleOrDefaultAsync(user => user.IdUser == idUser);
            if (user is null)
                throw new NotFoundException($"User with id {idUser} not found");

            var role = await _context.Roles.SingleOrDefaultAsync(role => role.IdRole == idRole);
            if (role is null)
                throw new NotFoundException($"Role with id {idRole} not found");

            user.IdRole = idRole;
            await _context.SaveChangesAsync();
            return user;
        }

        #endregion

        public async void InformUsers(string subject, string text, bool toEveryone) {
            Uri uri = new Uri("rabbitmq://localhost:5672/emailQueue");
            var endPoint = await _bus.GetSendEndpoint(uri);
            EmailDTO dto = new EmailDTO() {
                Text = text,
                Header = subject,
                ToEveryone = toEveryone,
            };
            await endPoint.Send(dto);
        }
    }

}
