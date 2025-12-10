using Modules;

namespace API
{
    /// <summary>
    /// A simple ping response class used to test the connection to the API.
    /// </summary>
    public class PingAPI
    {
        /// <summary>
        /// Simply returns a Ping object to confirm the API is reachable.
        /// </summary>
        /// <returns>
        /// Returns a Ping object.
        /// </returns>
        /// <response code="200">Returns a Ping object.</response>
        public static async Task<IResult> Ping()
        {
            return TypedResults.Ok(new PingPong());
        }
    }
}
