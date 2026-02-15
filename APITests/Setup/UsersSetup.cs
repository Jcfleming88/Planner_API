using System;
using System.Collections.Generic;
using System.Text;
using Modules;

namespace APITests
{
    public partial class UsersTests
    {
        internal PlannerDb Context { get; set; }

        internal const string DatabaseName = "UsersDb";

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
