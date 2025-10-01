using FluentAssertions;
using System.Net.Http.Headers;

namespace Corporate.Cashflow.Integrations.Texts.Accounts
{
    [Collection("CashFlowApp")]
    public class AccountTests
    {
        private readonly TestContainerFixture _fixture;

        public AccountTests(TestContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAccount_Unauthenticated_ReturnsUnauthorized()
        {
            // Act
            var response = await _fixture.ClientCashFlowApi().GetAsync("/api/accounts/3fa85f64-5717-4562-b3fc-2c963f66afa6");
            // Assert
            response.Should().Be401Unauthorized();
        }

        [Fact]
        public async Task CreateAccount_Authenticated_ReturnsCreated()
        {
            // act
            var tokenForNewUser =
                await UserTests.GetTokenForNewUserAsync(_fixture.ClientIdentityServerApi());

            // assert
            tokenForNewUser!.AccessToken.Should().NotBeNullOrEmpty();

            // act
            var accountResponse =
                await CreateAccountAsync(_fixture.ClientCashFlowApi(), tokenForNewUser!.AccessToken, "Test Account");

            accountResponse.Should().Be201Created();
        }

        public static async Task<HttpResponseMessage?> CreateAccountAsync(HttpClient httpClient, string bearerToken, string name)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", bearerToken);


            var request = new
            {
                name = "Test Account",
                currency = "USD"
            };
            return await httpClient.PostAsJsonAsync("/api/accounts", request);
        }
    }
}
