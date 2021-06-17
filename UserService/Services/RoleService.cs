using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserMicroservice.Models.DTO;
using UserService;
using UserService.Models.Exceptions;

namespace UserMicroservice.Services {
    public class RoleService {

        private readonly UserDbContext _context;
        public RoleService(UserDbContext context) {
            _context = context;
        }

        #region CRUD
        public async Task<List<Role>> GetAllRoles() {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleById(int idRole) {
            var role = await _context.Roles.SingleOrDefaultAsync(role => role.IdRole == idRole);
            if (role is null)
                throw new NotFoundException($"Role with id {idRole} not found");
            return role;
        }

        public async Task<Role> CreateRole(CreateRoleDTO roleDTO) {

            if (_context.Users.Any(role => role.IdRole == roleDTO.IdRole))
                throw new NotFoundException($"Role with id {roleDTO.IdRole} not found");

            Role role = new Role(roleDTO);
            _context.Add(role);
            await _context.SaveChangesAsync();

            return role;
        }
        public async Task<Role> UpdateRole(int idRole, Role role) {

            if (idRole != role.IdRole)
                throw new BadRequestException("You can't change roleId");

            var roleToUpdate = await _context.Roles.SingleOrDefaultAsync(role => role.IdRole == idRole);

            if (roleToUpdate is null)
                throw new NotFoundException($"Role with id {idRole} not found");

            _context.Update(role);
            await _context.SaveChangesAsync();

            return role;
        }

        public async Task<Role> DeleteRoleById(int idRole) {
            var role = await _context.Roles.SingleOrDefaultAsync(role => role.IdRole == idRole);
            if (role is null)
                throw new NotFoundException($"Role with id {idRole} not found");
            _context.Remove(role);
            await _context.SaveChangesAsync();
            return role;
        }
        #endregion
    }
}
