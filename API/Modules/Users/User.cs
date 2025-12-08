using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modules
{
    [Table("users")]
    public class User
    {
        #region Primary Key
        [Key]
        public string Id { get; set; }
        #endregion

        #region Properties
        public string Name { get; set; }
        public string Email { get; set; }
        #endregion

        #region Navigation Property
        public ProjectUser? ProjectUser { get; set; }
        public List<TaskAssignee> Assignments { get; set; } = new List<TaskAssignee>();
        #endregion

        public User(string id, string name, string email)
        {
            Id = id;
            Name = name ?? "";
            Email = email ?? "";
        }
    }
}
