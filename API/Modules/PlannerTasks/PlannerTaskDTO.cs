namespace Modules
{
    public class PlannerTaskDTO
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string? Category { get; set; }
        public string Owner { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int Urgency { get; set; }
        public bool AutomateUrgency { get; set; }
        public DateTime? BecomesUrgent { get; set; }
        public List<string> AssignedTo { get; set; } = new List<string>();
        public int Status { get; set; }
        public PlannerTaskDTO(PlannerTask task) =>
            (Id, ParentId, ProjectId, Name, Category, Owner, Start, End, Urgency, BecomesUrgent, Status) =
            (task.Id, task.ParentId, task.ProjectId, task.Name, task.Category, task.Owner, task.Start, task.End, task.Urgency,
            task.BecomesUrgent, task.Status);
    }
}
