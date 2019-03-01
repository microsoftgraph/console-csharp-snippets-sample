using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_csharp_snippets_sample
{
    internal class Constants
    {
        public const string Tenant = "ENTER_YOUR_TENANT_NAME";
        
        // User consent flow
        public const string ClientIdForUserAuthn = "ENTER_YOUR_CLIENT_ID";

        // Admin consent flow
        public const string AuthorityUri = "https://login.microsoftonline.com/" + Tenant;
        public const string RedirectUriForAppAuthn = "https://login.microsoftonline.com";
        public const string ClientIdForAppAuthn = "ENTER_YOUR_APP_ONLY_CLIENT_ID";
        public const string ClientSecret = "ENTER_YOUR_CLIENT_SECRET";
        // Consent URI: 
        // https://login.microsoftonline.com/{tenant name}/adminconsent?client_id={application id}&state=12345&redirect_uri=https://login.microsoftonline.com
    }
}
