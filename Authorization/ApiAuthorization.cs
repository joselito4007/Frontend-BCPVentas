using Frontend_BCPVentas.Models;
using FluentAssertions;
using RestSharp;
using RestSharp.Authenticators;
using static Frontend_BCPVentas.Models.Login;

namespace Frontend_BCPVentas.Authorization
{
    public class ApiAuthorization : AuthenticatorBase
    {
        private string _url;
        private Login _login;
        public ApiAuthorization(string url, Login login):base("")
        {
            _url = url;
            _login = login;
        }

        //public Login login = new Login();

        public string GetToken()
        {
            using (var client = new RestClient(_url)) {

                var authResponse = client.PostJson<Login, Access>("Login", _login);

                authResponse.Token.Should().NotBeNullOrEmpty();

                return $"Bearer {authResponse.Token}";
          }
        }

        protected override ValueTask<Parameter> GetAuthenticationParameter(string accesToken)
        {
            Token = string.IsNullOrEmpty(Token) ? GetToken() : Token;
            return new ValueTask<Parameter>(new HeaderParameter(KnownHeaders.Authorization, Token));
        }
    }
}
