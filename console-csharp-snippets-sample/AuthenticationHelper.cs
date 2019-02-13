using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Collections.Generic;

namespace console_csharp_snippets_sample
{
    class AuthenticationHelper
    {
        // The Client ID is used by the application to uniquely identify itself to the v2.0 authentication endpoint.
        static string clientIdForUser = Constants.ClientIdForUserAuthn;
        static string clientIdForApp = Constants.ClientIdForAppAuthn;
        // The Group.Read.All permission is an admin-only scope, so authorization will fail if you 
        // want to sign in with a non-admin account. Remove that permission and comment out the group operations in 
        // the UserMode() method if you want to run this sample with a non-admin account.
        public static string[] Scopes = { "User.Read",
                                           "User.ReadWrite",
                                           "User.ReadBasic.All",
                                           "Calendars.ReadWrite",
                                           "Contacts.Read",
                                           "Mail.Send",
                                           "Mail.ReadWrite",
                                           "Files.ReadWrite",
                                            // Group.Read.All is an admin-only scope. It allows you to read Group details.
                                            // Uncomment this scope if you want to run the application with an admin account
                                            // and perform the group operations in the UserMode class.
                                            // You'll also need to uncomment the UserMode.UserModeRequests.GetDetailsForGroups() method.
                                            //"Group.Read.All" 
                                        };

        public static PublicClientApplication IdentityClientApp = new PublicClientApplication(clientIdForUser);
        public static ConfidentialClientApplication IdentityAppOnlyApp = new ConfidentialClientApplication(clientIdForApp, Constants.AuthorityUri, Constants.RedirectUriForAppAuthn, new ClientCredential(Constants.ClientSecret), new TokenCache(), new TokenCache());

        private static GraphServiceClient graphClient = null;

        // Get an access token for the given context and resourceId. An attempt is first made to 
        // acquire the token silently. If that fails, then we try to acquire the token by prompting the user.
        public static GraphServiceClient GetAuthenticatedClientForUser()
        {
            // Create Microsoft Graph client.
            try
            {
                graphClient = new GraphServiceClient(
                    "https://graph.microsoft.com/v1.0",
                    new DelegateAuthenticationProvider(
                        async (requestMessage) =>
                        {
                            var token = await GetTokenForUserAsync();
                            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
                            // This header has been added to identify our sample in the Microsoft Graph service.  If extracting this code for your project please remove.
                            requestMessage.Headers.Add("SampleID", "console-csharp-snippets-sample");

                        }));
                return graphClient;
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Could not create a graph client: " + ex.Message);
            }

            return graphClient;
        }


        /// <summary>
        /// Get Token for User.
        /// </summary>
        /// <returns>Token for user.</returns>
        public static async Task<string> GetTokenForUserAsync()
        {
            AuthenticationResult authResult;
            IEnumerable<IAccount> accounts = await IdentityClientApp.GetAccountsAsync();
            IAccount firstAccount = accounts.FirstOrDefault();
            try
            {
                authResult = await IdentityClientApp.AcquireTokenSilentAsync(Scopes, firstAccount);
            }
            catch (MsalUiRequiredException)
            {
                authResult = await IdentityClientApp.AcquireTokenAsync(Scopes);
            }
            return authResult.AccessToken;
        }

        public static GraphServiceClient GetAuthenticatedClientForApp()
        {

            // Create Microsoft Graph client.
            try
            {
                graphClient = new GraphServiceClient(
                    "https://graph.microsoft.com/v1.0",
                    new DelegateAuthenticationProvider(
                        async (requestMessage) =>
                        {
                            var token = await GetTokenForAppAsync();
                            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
                            // This header has been added to identify our sample in the Microsoft Graph service.  If extracting this code for your project please remove.
                            requestMessage.Headers.Add("SampleID", "console-csharp-snippets-sample");

                        }));
                return graphClient;
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Could not create a graph client: " + ex.Message);
            }


            return graphClient;
        }

        /// <summary>
        /// Get Token for App.
        /// </summary>
        /// <returns>Token for app.</returns>
        public static async Task<string> GetTokenForAppAsync()
        {
            AuthenticationResult authResult;
            authResult = await IdentityAppOnlyApp.AcquireTokenForClientAsync(new string[] { "https://graph.microsoft.com/.default" });
            return authResult.AccessToken;
        }

        /// <summary>
        /// Signs the user out of the service.
        /// </summary>
        public static async void SignOut()
        {
            IEnumerable<IAccount> accounts = await IdentityClientApp.GetAccountsAsync();
          
            foreach (var account in accounts.ToArray())
            {
                await IdentityClientApp.RemoveAsync(account);
            }
            graphClient = null;
        }
    }
}
