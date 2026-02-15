using System;
using System.Collections.Generic;
using System.Text;
using Modules;

namespace APITests
{
    public partial class TaskAssignees
    {
        internal PlannerDb Context { get; set; }
        internal const string DatabaseName = "TaskAssigneeDb";

        [OneTimeSetUp]
        public void Setup()
        {
            Context = InMemoryDbFactory.CreateAndSeedContext(DatabaseName);
            // Seed a task and a user for assignment
            var user = new User ( id: Guid.CreateVersion7().ToString(), name: "Test User", email: "testuser@example.com" );
            Context.User.Add(user);
            var task = new PlannerTask(projectId: 1, name: "Test Task", owner: user.Id.ToString());
            Context.PlannerTask.Add(task);
            Context.SaveChanges();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Context?.Dispose();
        }
    }
}
