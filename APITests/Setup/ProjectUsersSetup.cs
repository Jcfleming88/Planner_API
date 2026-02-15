using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Modules;

namespace APITests
{
    public partial class ProjectUserTests
    {
        private PlannerDb Context { get; set; }
        private ProjectUser? FirstProjectUser { get; set; } // Used for chaining ordered tests, similar to Projects.cs

        internal const string DatabaseName = "ProjectUserDb";

        [OneTimeSetUp]
        public void Setup()
        {
            Context = InMemoryDbFactory.CreateAndSeedContext(DatabaseName);
            Context.Database.EnsureCreated();
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Context.Database.EnsureDeleted(); // Clean up after each test
            Context.Dispose();
            FirstProjectUser = null; // Clear shared project user data
        }
    }
}
