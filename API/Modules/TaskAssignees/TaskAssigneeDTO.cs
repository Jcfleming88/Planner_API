namespace Modules
{
    public class TaskAssigneeDTO
    {
        public int TaskId { get; set; }
        public string UserId { get; set; }
        public TaskAssigneeDTO(TaskAssignee taskAssignee) =>
            (TaskId, UserId) = (taskAssignee.TaskId, taskAssignee.UserId);
    }
}
