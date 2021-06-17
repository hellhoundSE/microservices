using System;
using System.Collections.Generic;

#nullable disable

namespace GoalMicroservice
{
    public partial class GoalTool
    {
        public int IdGoal { get; set; }
        public int IdTool { get; set; }

        public virtual Goal IdGoalNavigation { get; set; }
        public virtual Tool IdToolNavigation { get; set; }
    }
}
