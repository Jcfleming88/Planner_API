namespace Modules
{
    public class PlannerTaskDTO
    {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string? Category { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int Urgency { get; set; }
        public bool AutomateUrgency { get; set; }
        public DateTime? BecomesUrgent { get; set; }
        public List<string> AssignedTo { get; set; }
        public int Status { get; set; }
        public PlannerTaskDTO(PlannerTask task) =>
            (Id, ParentId, ProjectId, Name, Category, CreatedBy, Start, End, Urgency, BecomesUrgent, AssignedTo, Status) =
            (task.Id, task.ParentId, task.ProjectId, task.Name, task.Category, task.CreatedBy, task.Start, task.End, task.Urgency,
            task.BecomesUrgent, task.AssignedTo, task.Status);
    }
}
