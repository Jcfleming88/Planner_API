using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Modules
{
    [Table("projectUsers")]
    [PrimaryKey(nameof(UserId), nameof(ProjectId))]
    public class ProjectUser
    {
        #region Primary Key
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ProjectId { get; set; }
        #endregion

        #region Properties
        [Required]
        public int Role { get; set; }
        #endregion

        #region Navigation Property
        public Project? Project { get; set; } = null;
        #endregion

        public ProjectUser(string userId, int projectId, int role)
        {
            UserId = userId;
            ProjectId = projectId;
            Role = role;
        }
    }
}
