using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using NUnit.Framework;
using API;
using Modules;

namespace APITests
{
    public partial class UsersTests
    {
        [Test, Category("Users"), Order(1)]
        public async Task CreateUser_CreatesUserAndReturnsCreated()
        {
            var user = new User ( id: Guid.CreateVersion7().ToString(), name: "Alice", email: "alice@example.com" );

            var result = await UserAPI.CreateUser(user, Context);

            Assert.That(result, Is.InstanceOf<Created<User>>());
            var created = (result as Created<User>)!;
            Assert.That(created.Value, Is.Not.Null);
            Assert.That(created.Value.Name, Is.EqualTo("Alice"));
            Assert.That(created.Value.Email, Is.EqualTo("alice@example.com"));
            Assert.That(created.Location, Does.Contain("/users/"));
        }

        [Test, Category("Users"), Order(2)]
        public async Task GetAllUsers_ReturnsAllUsers()
        {
            // Arrange
            Context.User.Add(new User(id: Guid.CreateVersion7().ToString(),name: "Bob", email: "bob@example.com" ));
            await Context.SaveChangesAsync();

            // Act
            var result = await UserAPI.GetAllUsers(Context);

            // Assert
            Assert.That(result, Is.InstanceOf<Ok<List<User>>>());
            var users = (result as Ok<List<User>>)?.Value;
            Assert.That(users, Is.Not.Null);
            Assert.That(users!.Any(u => u.Name == "Bob"), Is.True);
        }

        [Test, Category("Users"), Order(3)]
        public async Task GetUserById_ReturnsUser_WhenExists()
        {
            var user = new User(id: Guid.CreateVersion7().ToString(), name: "Charlie", email: "charlie@example.com" );
            Context.User.Add(user);
            await Context.SaveChangesAsync();

            var result = await UserAPI.GetUserById(user.Id, Context);

            Assert.That(result, Is.InstanceOf<Ok<User>>());
            var found = (result as Ok<User>)?.Value;
            Assert.That(found, Is.Not.Null);
            Assert.That(found!.Name, Is.EqualTo("Charlie"));
        }

        [Test, Category("Users"), Order(4)]
        public async Task GetUserById_ReturnsNotFound_WhenNotExists()
        {
            var result = await UserAPI.GetUserById("No id", Context);
            Assert.That(result, Is.InstanceOf<NotFound>());
        }

        [Test, Category("Users"), Order(5)]
        public async Task UpdateUser_UpdatesUserAndReturnsNoContent()
        {
            string id = Guid.CreateVersion7().ToString();
            var user = new User(id: id, name: "David", email: "david@old.com" );
            Context.User.Add(user);
            await Context.SaveChangesAsync();

            var updated = new User(id: id, name: "David Updated", email: "david@new.com" );
            var result = await UserAPI.UpdateUser(user.Id, updated, Context);

            Assert.That(result, Is.InstanceOf<NoContent>());
            var dbUser = await Context.User.FindAsync(user.Id);
            Assert.That(dbUser!.Name, Is.EqualTo("David Updated"));
            Assert.That(dbUser.Email, Is.EqualTo("david@new.com"));
        }

        [Test, Category("Users"), Order(6)]
        public async Task UpdateUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var updated = new User (id: Guid.CreateVersion7().ToString(), name: "Ghost", email: "ghost@none.com" );
            var result = await UserAPI.UpdateUser("Wrong Id", updated, Context);
            Assert.That(result, Is.InstanceOf<NotFound>());
        }

        [Test, Category("Users"), Order(7)]
        public async Task DeleteUser_DeletesUserAndReturnsNoContent()
        {
            var user = new User(id: Guid.CreateVersion7().ToString(), name: "Eve", email: "eve@example.com" );
            Context.User.Add(user);
            await Context.SaveChangesAsync();

            var result = await UserAPI.DeleteUser(user.Id, Context);

            Assert.That(result, Is.InstanceOf<NoContent>());
            var dbUser = await Context.User.FindAsync(user.Id);
            Assert.That(dbUser, Is.Null);
        }

        [Test, Category("Users"), Order(8)]
        public async Task DeleteUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var result = await UserAPI.DeleteUser("No user", Context);
            Assert.That(result, Is.InstanceOf<NotFound>());
        }
    }
}
