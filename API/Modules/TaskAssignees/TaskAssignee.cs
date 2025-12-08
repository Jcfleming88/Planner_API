using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Modules
{
    [Table("taskAssignees")]
    [PrimaryKey(nameof(TaskId), nameof(UserId))]
    public class TaskAssignee
    {

        #region Primary Key
        [Required]
        public int TaskId { get; set; }
        [Required]
        public string UserId { get; set; }
        #endregion

        #region Navigation Property
        public PlannerTask? PlannerTask { get; set; }
        public User? User { get; set; }
        #endregion

        public TaskAssignee(int taskId, string userId)
        {
            TaskId = taskId;
            UserId = userId;
        }
    }
}
