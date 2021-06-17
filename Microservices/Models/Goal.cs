using GoalMicroservice.Models.DTO;
using System;
using System.Collections.Generic;

#nullable disable

namespace GoalMicroservice
{
    public partial class Goal
    {
        public Goal()
        {
            GoalTools = new HashSet<GoalTool>();
        }
        
        public Goal(CreateGoalDTO dto)
        {
            IdGoal = dto.IdGoal;
            Text = dto.Text;
            DeadlineTimeStamp = dto.DeadlineTimeStamp;
            IsAdminOnly = dto.IsAdminOnly;
            GoalTools = new HashSet<GoalTool>();
        }

        public int IdGoal { get; set; }
        public string Text { get; set; }
        public DateTime? FinishedTimeStamp { get; set; }
        public DateTime DeadlineTimeStamp { get; set; }
        public bool IsAdminOnly { get; set; }

        public virtual ICollection<GoalTool> GoalTools { get; set; }
    }
}
