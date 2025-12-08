namespace Modules
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserDTO(User user) =>
            (Id, Name, Email) = (user.Id, user.Name, user.Email);
    }
}
