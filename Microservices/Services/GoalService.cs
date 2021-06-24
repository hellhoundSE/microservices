using GoalMicroservice.Models.DTO;
using GoalMicroservice;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models.Exceptions;
using MassTransit;
using EmailMictoservice.DTO;

namespace GoalMicroservice {
    public class GoalService {

        private readonly GoalDbContext _context;
        private readonly IBus _bus;

        public GoalService(GoalDbContext context, IBus bus) {
            _context = context;
            _bus = bus;
        }

        #region CRUD

        public async Task<List<Goal>> GetAllGoals() {
            InformUsers("test", "testtest", false);
            return await _context.Goals.ToListAsync();
        }

        public async Task<Goal> GetGoalById(int idGoal) {
            var goal = await _context.Goals.SingleOrDefaultAsync(goal => goal.IdGoal == idGoal);
            if (goal is null)
                throw new NotFoundException($"Goal with id {idGoal} not found");
            return goal;
        }

        public async Task<Goal> CreateGoal(CreateGoalDTO goalDTO) {

            if (_context.Goals.Any(goal => goal.IdGoal == goalDTO.IdGoal))
                throw new BadRequestException("Goal with given id already exists");


            Goal goal = new Goal(goalDTO);
            _context.Add(goal);
            await _context.SaveChangesAsync();

            InformUsers("New Goal!", "New goal was created, check this out!", true);

            return goal;
        }
        public async Task<Goal> UpdateGoal(int idGoal, Goal goal) {

            if (idGoal != goal.IdGoal)
                throw new BadRequestException("You can't change goalId");

            var goalToUpdate = await _context.Goals.SingleOrDefaultAsync(g => g.IdGoal == idGoal);

            if (goalToUpdate is null)
                throw new NotFoundException($"Goal with id {idGoal} not found");

            _context.Update(goal);
            await _context.SaveChangesAsync();

            InformUsers("New Update!", $"The {goal.Text} was updated, check this out!", true);

            return goal;
        }

        public async Task<Goal> DeleteGoalById(int idGoal) {
            var goal = await _context.Goals.SingleOrDefaultAsync(goal => goal.IdGoal == idGoal);
            if (goal is null)
                throw new NotFoundException($"User with id {idGoal} not found");

            _context.Remove(goal);
            await _context.SaveChangesAsync();

            InformUsers("One less :(", $"The {goal.Text} was deleted", true);

            return goal;
        }
        #endregion
        #region Tools
        internal async Task<GoalTool> AddTool(int idGoal, int idTool) {

            var goal = await _context.Goals.SingleOrDefaultAsync(goal => goal.IdGoal == idGoal);
            if (goal is null)
                throw new NotFoundException($"User with id {idGoal} not found");

            var tool = await _context.Tools.SingleOrDefaultAsync(tool => tool.IdTool == idTool);
            if (tool is null)
                throw new NotFoundException($"Tool with id {idTool} not found");

            GoalTool gt = new GoalTool() {
                IdGoal = idGoal,
                IdTool = idTool,
            };
            _context.Add(gt);
            await _context.SaveChangesAsync();
            return gt;
        }
        internal async Task<GoalTool> RemoveTool(int idGoal, int idTool) {

            var goal = await _context.Goals.SingleOrDefaultAsync(goal => goal.IdGoal == idGoal);
            if (goal is null)
                throw new NotFoundException($"User with id {idGoal} not found");

            var tool = await _context.Tools.SingleOrDefaultAsync(tool => tool.IdTool == idTool);
            if (tool is null)
                throw new NotFoundException($"Tool with id {idTool} not found");

            var gt = await _context.GoalTools.SingleOrDefaultAsync(gt => gt.IdTool == idTool && gt.IdGoal == idGoal);
            if (gt is null)
                throw new NotFoundException($"Tool with id {idTool}, are not needed for goal with id {idGoal}");

            _context.Remove(gt);
            await _context.SaveChangesAsync();
            return gt;
        }

        #endregion

        public async void InformUsers(string subject, string text,bool toEveryone) {
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
