namespace Modules
{
    /// <summary>
    /// Represents a project with a unique identifier, name, description, and a collection of associated users.
    /// </summary>
    /// <remarks>Use the Project class to encapsulate information about a project, including its metadata and
    /// the users linked to it. The Users property can store either usernames or user IDs, depending on application
    /// requirements. When creating a new instance, you may omit the Id for new projects that have not yet been
    /// persisted. All properties are mutable, allowing updates to project details after instantiation.</remarks>
    public class Project
    {
        /// <summary>
        /// The unique identifier for the project. Can be null for a new project.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// The name of the project.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A brief description of the project.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An array of usernames or IDs associated with the project.
        /// </summary>
        public List<string> Users { get; set; }

        /// <summary>
        /// Initializes a new instance of the Project class.
        /// </summary>
        /// <param name="id">The unique identifier for the project. Defaults to null.</param>
        /// <param name="name">The name of the project. Defaults to an empty string.</param>
        /// <param name="description">A brief description of the project. Defaults to an empty string.</param>
        /// <param name="users">An array of usernames/IDs. Defaults to a new, empty list.</param>
        public Project(
            int? id = null, 
            string name = "", 
            string description = "", 
            List<string>? users = null
            )
        {
            this.Id = id;
            this.Name = name ?? "";
            this.Description = description ?? "";
            this.Users = users ?? new List<string>();
        }
    }
}
