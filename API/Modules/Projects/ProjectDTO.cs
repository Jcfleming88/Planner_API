using System.Net.NetworkInformation;

namespace Modules
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Users { get; set; } = new List<string>();
        public ProjectDTO(Project project) =>
            (Id, Name, Description) = (project.Id, project.Name, project.Description);
    }
}
