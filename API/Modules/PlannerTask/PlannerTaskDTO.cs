namespace Modules
{
    public class PlannerTaskDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }

        public PlannerTaskDTO() { }
        public PlannerTaskDTO(PlannerTask plannerTask) =>
            (Id, Name, Status) = (plannerTask.Id, plannerTask.Name, plannerTask.Status);
    }
}
