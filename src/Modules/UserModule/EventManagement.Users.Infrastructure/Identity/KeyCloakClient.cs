using System.Net.Http.Json;

namespace EventManagement.Users.Infrastructure.Identity
{
    internal class KeyCloakClient(HttpClient httpClient)
    {
        public async Task<string> RegisterUserAsync(UserRepresentation userRepresentation, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PostAsJsonAsync("users", userRepresentation, cancellationToken);

            response.EnsureSuccessStatusCode();
            return ExtractIdentityIdFromLocationHeader(response);

        }

        private string ExtractIdentityIdFromLocationHeader(HttpResponseMessage response)
        {
            const string usersSegmentName = "users/";

            var locationHeader = response.Headers.Location?.PathAndQuery;
            if (locationHeader == null)
            {
                throw new InvalidOperationException("Location header is null");
            }

            var usersSegmentIndex = locationHeader.IndexOf(usersSegmentName, StringComparison.InvariantCultureIgnoreCase);

            return locationHeader.Substring(usersSegmentIndex + usersSegmentName.Length);
        }
    }
}
