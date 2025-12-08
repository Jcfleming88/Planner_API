using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modules
{
    /// <summary>
    /// Tasks are used to define particular work items within a project.
    /// </summary>
    [Table("tasks")]
    public class PlannerTask
    {
        #region Primary Key
        /// <summary>
        /// Unique identifier for the task. Nullable int allows for the 'null' default.
        /// </summary>
        [Key]
        public int Id { get; set; }
        #endregion

        #region Foreign Key
        /// <summary>
        /// ID of the project the task belongs to.
        /// </summary>
        [Required]
        public int ProjectId { get; set; }
        #endregion

        #region Properties
        /// <summary>
        /// ID of a parent task.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// The name or title of the task.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The task category. Nullable string allows for 'null'.
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// The user who created the task.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// The planned start date. Nullable DateTime allows for 'null'.
        /// </summary>
        public DateTime? Start { get; set; }

        /// <summary>
        /// The planned end/due date. Using DateTime.Today as a representative C# default 
        /// for 'new Date()' without time component, or you could use DateTime.Now.
        /// </summary>
        public DateTime? End { get; set; }

        /// <summary>
        /// Manual urgency level (0-5).
        /// </summary>
        public int Urgency { get; set; }

        /// <summary>
        /// Flag for automated urgency calculation.
        /// </summary>
        public bool AutomateUrgency { get; set; }

        /// <summary>
        /// Date when urgency is triggered. Nullable DateTime allows for 'null'.
        /// The complex logic from the JS constructor is typically handled 
        /// outside the class definition or in a specific method in C#.
        /// </summary>
        public DateTime? BecomesUrgent { get; set; }

        /// <summary>
        /// The task status (e.g., 0: Pending).
        /// </summary>
        public int Status { get; set; }
        #endregion

        #region Navigation Property
        public Project? Project { get; set; }
        public List<TaskAssignee> TaskAssignees { get; set; } = new List<TaskAssignee>();
        #endregion

        /// <summary>
        /// C# Default Constructor creating a blank task.
        /// </summary>
        public PlannerTask(
                int id = 0, 
                int? parentId = null, 
                int projectId = 0, 
                string name = "", 
                string category = "", 
                string owner= "", 
                DateTime? start = null, 
                DateTime? end = null, 
                int urgency = 0, 
                bool automateUrgency = false,
                DateTime? becomesUrgent = null,
                int status = 0
            )
        {
            this.Id = id;
            this.ParentId = parentId;
            this.ProjectId = projectId;
            this.Name = name;
            this.Category = category;
            this.Owner = owner;
            this.Start = start;
            this.End = end;
            this.Urgency = urgency;
            this.AutomateUrgency = automateUrgency;

            if (BecomesUrgent == null && Start.HasValue && AutomateUrgency)
            {
                BecomesUrgent = Start.Value.AddDays(1);
            } 
            else
            {
                BecomesUrgent = becomesUrgent ?? null;
            }

            this.Status = status;
        }
    }
}
