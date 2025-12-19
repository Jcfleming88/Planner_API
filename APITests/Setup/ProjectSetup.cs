using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using NUnit.Framework;
using API;
using Modules;

namespace APITests
{
    public partial class ProjectsTests
    {
        internal PlannerDb Context { get; set; }      
        private ProjectDTO? FirstProject { get; set; } = null;

        internal const string DatabaseName = "ProjectsDb";

        private readonly ProjectDTO NewProject = new ProjectDTO(new Project(
            name: "New Project",
            description: "This is a new project."
        ));

        [OneTimeSetUp]
        public void Setup()
        {
            Context = InMemoryDbFactory.CreateAndSeedContext(DatabaseName);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Context?.Dispose();
        }
    }
}
