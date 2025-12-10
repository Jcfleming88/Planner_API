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
        public const string DatabaseName = "BookMethodsTestDb";

        [SetUp]
        public void Setup()
        {
            using var context = InMemoryDbFactory.CreateAndSeedContext(DatabaseName);
        }
    }
}
