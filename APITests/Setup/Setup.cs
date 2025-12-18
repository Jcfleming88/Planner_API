using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using NUnit.Framework;
using API;
using Modules;

namespace APITests
{
    public partial class Tests
    {
        internal const string DatabaseName = "BookMethodsTestDb";
        internal PlannerDb Context { get; set; }

        [SetUp]
        public void Setup()
        {
            Context = InMemoryDbFactory.CreateAndSeedContext(DatabaseName);
        }

        [TearDown]
        public void TearDown()
        {
            Context?.Dispose();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Context?.Dispose();
        }
    }
}
