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
        [Test]
        [Category("Ping")]
        public async Task Ping()
        {
            var result = await PingAPI.Ping();

            Assert.That(
                result,
                Is.InstanceOf<Ok<PingPong>>(),
                "The result should be an HTTP 200 Ok response."
                );

            return;
        }
    }
}
