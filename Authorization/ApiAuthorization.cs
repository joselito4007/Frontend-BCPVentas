using Frontend_BCPVentas.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
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

        public Login login = new Login();
        public Access access = new Access();
        public string GetToken()
        {
            string token;
            login.Email = _login.Email;
            login.Pwd = _login.Pwd;

            RestClient Login = new RestClient(_url);
            string json = JsonConvert.SerializeObject(login);

            var request = new RestRequest("Login", Method.Get);

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("login", login.Email);
            request.AddParameter("password", login.Pwd);

            //request.RequestFormat = RestSharp.DataFormat.Json;
            //request.AddBody(new Login
            //{
            //    Email = "carlitos.4007",
            //    Pwd = "123456"
            //});

            var respuesta = Login.Execute(request);

            string jwtToken = respuesta.Content;

            var ojwtToken = JsonConvert.DeserializeObject<Access>(jwtToken);

            return $"Bearer {ojwtToken.token}";
        }

        protected override ValueTask<Parameter> GetAuthenticationParameter(string accesToken)
        {
            Token = string.IsNullOrEmpty(Token) ? GetToken() : Token;
            return new ValueTask<Parameter>(new HeaderParameter(KnownHeaders.Authorization, Token));
        }
    }
}
