using System;
using System.Collections.Generic;

#nullable disable

namespace GoalMicroservice
{
    public partial class Tool
    {
        public Tool()
        {
            GoalTools = new HashSet<GoalTool>();
        }

        public int IdTool { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<GoalTool> GoalTools { get; set; }
    }
}
