using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserMicroservice.Models.DTO;

namespace UserService.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {

        private readonly ILogger<UsersController> _logger;
        private readonly UserService _service;

        public UsersController(ILogger<UsersController> logger, UserService service) {
            _logger = logger;
            _service = service;
        }
        #region CRUD

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(bool toEveryone) {
            return Ok(await _service.GetUsers(toEveryone));
        }
        [HttpGet("{idUser}")]
        public async Task<IActionResult> GetUser(int idUser) {
            return Ok(await _service.GetUserById(idUser));
        }

        [HttpGet("admins")]
        public async Task<IActionResult> GetAllAdmins() {
            return Ok(await _service.GetAllAdmins());
        }

        [HttpPost()]
        public async Task<IActionResult> CreateUser(CreateUserDTO userDTO) {
            return Ok(await _service.CreateUser(userDTO));
        } 

        [HttpPut("{idUser}")]
        public async Task<IActionResult> UpdateUser(int idUser, User user) {
            return Ok(await _service.UpdateUser(idUser,user));
        } 
    
        [HttpDelete("{idUser}")]
        public async Task<IActionResult> DeleteUser(int idUser) {
            return Ok(await _service.DeleteUserById(idUser));
        }
        #endregion

    }
}
