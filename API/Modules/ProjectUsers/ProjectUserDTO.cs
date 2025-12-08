namespace Modules
{
    public class ProjectUserDTO
    {
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        public int Role { get; set; }

        public ProjectUserDTO(ProjectUser projectUser) =>             
            (ProjectId, UserId, Role) = (projectUser.ProjectId, projectUser.UserId, projectUser.Role);
    }
}
