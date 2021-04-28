using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace JiraTimeBot.JiraIntegration.Authentificators
{
    public class JSessionAuthenticator : AuthenticatorBase
    {
        public JSessionAuthenticator(string jSessionId) : base(jSessionId)
        {

        }

        protected override Parameter GetAuthenticationParameter(string token)
            => new Parameter("JSESSIONID", token, ParameterType.Cookie);
    }
}
