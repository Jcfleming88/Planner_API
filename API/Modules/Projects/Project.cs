using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modules
{
    /// <summary>
    /// Represents a project with a unique identifier, name, description, and a collection of associated users.
    /// </summary>
    /// <remarks>Use the Project class to encapsulate information about a project, including its metadata and
    /// the users linked to it. The Users property can store either usernames or user IDs, depending on application
    /// requirements. When creating a new instance, you may omit the Id for new projects that have not yet been
    /// persisted. All properties are mutable, allowing updates to project details after instantiation.</remarks>
    [Table("projects")]
    public class Project
    {
        #region Primary Key
        /// <summary>
        /// The unique identifier for the project. Can be null for a new project.
        /// </summary>
        [Key]
        public int Id { get; set; }
        #endregion

        #region Properties
        /// <summary>
        /// The name of the project.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A brief description of the project.
        /// </summary>
        public string Description { get; set; }
        #endregion

        #region Navigation Property
        public List<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
        public List<PlannerTask> PlannerTasks { get; set; } = new List<PlannerTask>();
        #endregion

        /// <summary>
        /// Initializes a new instance of the Project class.
        /// </summary>
        /// <param name="id">The unique identifier for the project. Defaults to null.</param>
        /// <param name="name">The name of the project. Defaults to an empty string.</param>
        /// <param name="description">A brief description of the project. Defaults to an empty string.</param>
        /// <param name="users">An array of usernames/IDs. Defaults to a new, empty list.</param>
        public Project(
            int id = 0, 
            string name = "", 
            string description = ""
            )
        {
            this.Id = id;
            this.Name = name ?? "";
            this.Description = description ?? "";
        }
    }
}
