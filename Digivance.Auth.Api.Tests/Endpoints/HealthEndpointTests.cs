using Digivance.Auth.Api.Endpoints;
using System.Net;
using HttpResults = Microsoft.AspNetCore.Http.HttpResults;

namespace Digivance.Auth.Api.Tests.Endpoints
{
    /// <summary>
    /// Unit tests for our HealthEndpoint
    /// </summary>
    [TestFixture]
    public class HealthEndpointTests
    {
        /// <summary>
        /// HealthEndpoint.ReadyAync
        /// </summary>
        [Test]
        public async Task CanBeReady()
        {
            var cancellationToken = new CancellationToken();
            var res = await HealthEndpoint.ReadyAsync(cancellationToken) as HttpResults.Ok<string>;

            Assert.That(res, Is.Not.Null);
            Assert.That(res.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        /// <summary>
        /// HealthEndpoint.LivenessAsync
        /// </summary>
        [Test]
        public async Task CanBeLive()
        {
            var cancellationToken = new CancellationToken();
            var res = await HealthEndpoint.LivenessAsync(cancellationToken) as HttpResults.Ok<string>;

            Assert.That(res, Is.Not.Null);
            Assert.That(res.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }
    }
}
