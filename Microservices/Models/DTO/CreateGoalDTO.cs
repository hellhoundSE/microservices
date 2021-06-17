using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalMicroservice.Models.DTO {
    public class CreateGoalDTO {

        public int IdGoal { get; set; }
        public string Text { get; set; }
        public DateTime DeadlineTimeStamp { get; set; }
        public bool IsAdminOnly { get; set; }
    }
}
