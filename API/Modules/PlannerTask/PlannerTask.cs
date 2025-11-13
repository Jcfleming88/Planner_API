namespace Modules
{
    /// <summary>
    /// Tasks are used to define particular work items within a project.
    /// </summary>
    public class PlannerTask
    {
        /// <summary>
        /// Unique identifier for the task. Nullable int allows for the 'null' default.
        /// </summary>
        public int? Id { get; set; } = null;

        /// <summary>
        /// ID of a parent task.
        /// </summary>
        public int? ParentId { get; set; } = null;

        /// <summary>
        /// ID of the project the task belongs to.
        /// </summary>
        public int ProjectId { get; set; } = 0;

        /// <summary>
        /// The name or title of the task.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The task category. Nullable string allows for 'null'.
        /// </summary>
        public string? Category { get; set; } = null;

        /// <summary>
        /// The user who created the task.
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// The planned start date. Nullable DateTime allows for 'null'.
        /// </summary>
        public DateTime? Start { get; set; } = null;

        /// <summary>
        /// The planned end/due date. Using DateTime.Today as a representative C# default 
        /// for 'new Date()' without time component, or you could use DateTime.Now.
        /// </summary>
        public DateTime End { get; set; } = DateTime.Today;

        /// <summary>
        /// Manual urgency level (0-5).
        /// </summary>
        public int Urgency { get; set; } = 0;

        /// <summary>
        /// Flag for automated urgency calculation.
        /// </summary>
        public bool AutomateUrgency { get; set; } = false;

        /// <summary>
        /// Date when urgency is triggered. Nullable DateTime allows for 'null'.
        /// The complex logic from the JS constructor is typically handled 
        /// outside the class definition or in a specific method in C#.
        /// </summary>
        public DateTime? BecomesUrgent { get; set; } = null;

        /// <summary>
        /// Array of users assigned to the task. Using List of strings for flexibility.
        /// Initializing with one empty string
        /// </summary>
        public List<string> AssignedTo { get; set; } = new List<string> { string.Empty };

        /// <summary>
        /// The task status (e.g., 0: Pending).
        /// </summary>
        public int Status { get; set; } = 0;


        /// <summary>
        /// C# Default Constructor creating a blank task.
        /// </summary>
        public PlannerTask()
        {
            if (BecomesUrgent == null && Start.HasValue && AutomateUrgency)
            {
                BecomesUrgent = Start.Value.AddDays(1);
            }
        }
    }
}
