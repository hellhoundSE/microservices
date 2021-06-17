using GoalMicroservice;
using GoalMicroservice.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalMicroservice.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class GoalsController : ControllerBase {

        private readonly GoalService _service;

        public GoalsController(GoalService service) {
            _service = service;
        }
        #region CRUD

        [HttpGet]
        public async Task<IActionResult> GetAllGoals() {
            return Ok(await _service.GetAllGoals());
        }
        [HttpGet("{idGoal}")]
        public async Task<IActionResult> GetGoal(int idGoal) {
            return Ok(await _service.GetGoalById(idGoal));
        } 
   
        [HttpPost()]
        public async Task<IActionResult> CreateGoal(CreateGoalDTO goalDTO) {
            return Ok(await _service.CreateGoal(goalDTO));
        } 

        [HttpPut("{idGoal}")]
        public async Task<IActionResult> UpdateUser(int idGoal, Goal goal) {
            return Ok(await _service.UpdateGoal(idGoal, goal));
        } 
    
        [HttpDelete("{idGoal}")]
        public async Task<IActionResult> DeleteGoal(int idGoal) {
            return Ok(await _service.DeleteGoalById(idGoal));
        }
        #endregion

        [HttpPost("{idGoal}/{idTool}")]
        public async Task<IActionResult> AddTool(int idGoal, int idTool) {
            return Ok(await _service.AddTool(idGoal, idTool));
        }
        [HttpDelete("{idGoal}/{idTool}")]
        public async Task<IActionResult> RemoveTool(int idGoal, int idTool) {
            return Ok(await _service.RemoveTool(idGoal, idTool));
        }


    }
}
