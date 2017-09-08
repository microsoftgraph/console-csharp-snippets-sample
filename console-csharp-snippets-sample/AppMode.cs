using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace console_csharp_snippets_sample
{
    internal class AppMode
    {
        public static GraphServiceClient client;
        public static void AppModeRequests()
        {
            try
            {
                //*********************************************************************
                // setup Microsoft Graph Client for app-only.
                //*********************************************************************
                if (Constants.ClientIdForAppAuthn != "ENTER_YOUR_APP_ONLY_CLIENT_ID" &&
                    Constants.Tenant != "ENTER_YOUR_TENANT_NAME" &&
                    Constants.ClientSecret!= "ENTER_YOUR_CLIENT_SECRET" )
                {
                    client = AuthenticationHelper.GetAuthenticatedClientForApp();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You haven't configured a value for ClientIdForAppAuthn, Tenant, and/or ClientSecret in Constants.cs. Please follow the Readme instructions for configuring this application.");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }


            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Acquiring a token failed with the following error: {0}", ex.Message);
                if (ex.InnerException != null)
                {
                    //You should implement retry and back-off logic per the guidance given here:http://msdn.microsoft.com/en-us/library/dn168916.aspx
                    //InnerException Message will contain the HTTP error status codes mentioned in the link above
                    Console.WriteLine("Error detail: {0}", ex.InnerException.Message);
                }
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nStarting app-mode requests...");
            Console.WriteLine("\n=============================\n\n");

            // Get first page of users in a tenant
            try
            {
                IGraphServiceUsersCollectionPage users = client.Users.Request().GetAsync().Result;
                foreach (User user in users)
                {
                    Console.WriteLine("Found user: " + user.Id);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\nError getting users {0} {1}",
                     e.Message, e.InnerException != null ? e.InnerException.Message : "");
            }

            // Get messages for a specific user (demonstrates $select operation)
            try
            {
                Console.WriteLine("\nEnter the email address of the user mailbox you want to retrieve:");
                String email = Console.ReadLine();
                List<Message> messages = client.Users[email].Messages.Request().Select("subject, receivedDateTime").GetAsync().Result.Take(3).ToList();
                if (messages.Count == 0)
                {
                    Console.WriteLine("    no messages in mailbox");
                }
                foreach (Message message in messages)
                {
                    Console.WriteLine("    Message: {0} received {1} ", message.Subject, message.ReceivedDateTime);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nError getting messages {0} {1}",
                     e.Message, e.InnerException != null ? e.InnerException.Message : "");
            }

            // Get groups for a specific user
            try
            {
                Console.WriteLine("\nEnter the email address of the user whose groups you want to retrieve:");
                String email = Console.ReadLine();

                IUserMemberOfCollectionWithReferencesPage userGroups = client.Users[email].MemberOf.Request().GetAsync().Result;

                if (userGroups.Count == 0)
                {
                    Console.WriteLine("    user is not a member of any groups");
                }
                foreach (DirectoryObject group in userGroups)
                {
                    if (group is Group)
                    {
                        Group _group = group as Group;
                        Console.WriteLine("    Id: {0}  UPN: {1}", _group.Id, _group.DisplayName);
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\nError getting group memberships {0} {1}",
                     e.Message, e.InnerException != null ? e.InnerException.Message : "");
            }

            //*********************************************************************
            // People picker
            // Search for a user using text string and match against userPrincipalName, displayName, giveName, surname
            //*********************************************************************
            Console.WriteLine("\nSearch for user (enter search string):");
            String searchString = Console.ReadLine();

            IGraphServiceUsersCollectionPage userCollection = null;
            try
            {
                string startsWithFilter = "startswith(displayName%2C+ '"
                    + searchString + "')+or+startswith(userPrincipalName%2C+ '"
                    + searchString + "')+or+startswith(givenName%2C+ '"
                    + searchString + "')+or+startswith(surname%2C+ '" + searchString + "')";
                userCollection = client.Users.Request().Filter(startsWithFilter).GetAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine("\nError getting User {0} {1}", e.Message,
                    e.InnerException != null ? e.InnerException.Message : "");
            }

            if (userCollection != null && userCollection.Count > 0)
            {

                foreach (User u in userCollection)
                {
                    Console.WriteLine("User: DisplayName: {0}  UPN: {1}",
                        u.DisplayName, u.UserPrincipalName);
                }
            }
            else
            {
                Console.WriteLine("User not found");
            }


            // Create a unified group 
            Console.WriteLine("\nDo you want to create a new unified group? Click y/n\n");
            ConsoleKeyInfo key = Console.ReadKey();
            // We need two Group variables: one to pass to the AddAsync and another that stores the
            // created group returned by AddAsync(). The createdGroup variable will have a value for the Id
            // property. We'll need to use that value later.
            Group uGroup = null;
            Group createdGroup = null;
            if (key.KeyChar == 'y')
            {
                string suffix = GetRandomString(5);
                uGroup = new Group
                {
                    GroupTypes = new List<string> { "Unified" },
                    DisplayName = "Unified group " + suffix,
                    Description = "Group " + suffix + " is the best ever",
                    MailNickname = "Group" + suffix,
                    MailEnabled = true,
                    SecurityEnabled = false
                };
                try
                {
                    createdGroup = client.Groups.Request().AddAsync(uGroup).Result;
                    Console.WriteLine("\nCreated unified group {0}", createdGroup.DisplayName);
                }
                catch (Exception)
                {
                    Console.WriteLine("\nIssue creating the group {0}", uGroup.DisplayName);
                    uGroup = null;
                }
            }

            // Add group members. If the user has chosen to create a group, use that one.
            // If the user has chosen not to create a group, find one in the tenant.

            Group groupToAddMembers = new Group();
            if (createdGroup != null)
            {
                groupToAddMembers = createdGroup;
            }
            else
            {
                string unifiedFilter = "groupTypes/any(gt:gt+eq+'Unified')";
                List<Group> unifiedGroups = client.Groups.Request().Filter(unifiedFilter).GetAsync().Result.Take(5).ToList();
                if (unifiedGroups != null && unifiedGroups.Count > 0)
                {
                    groupToAddMembers = unifiedGroups.First();
                }
            }

            // Get a set of users to add
            List<User> members = client.Users.Request().GetAsync().Result.Take(3).ToList();

            if (groupToAddMembers != null)
            {
                //Add users
                foreach (User user in members)
                {
                    try
                    {
                        client.Groups[groupToAddMembers.Id].Members.References.Request().AddAsync(user);
                        Console.WriteLine("\nAdding {0} to group {1}", user.UserPrincipalName, groupToAddMembers.DisplayName);
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine("\nError assigning member to group. {0} {1}",
                             e.Message, e.InnerException != null ? e.InnerException.Message : "");
                    }
                }

                // Now remove the added users
                foreach (User user in members)
                {
                    try
                    {
                        client.Groups[groupToAddMembers.Id].Members[user.Id].Reference.Request().DeleteAsync();
                        Console.WriteLine("\nRemoved {0} from group {1}", user.UserPrincipalName, groupToAddMembers.DisplayName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("\nError removing member from group. {0} {1}",
                             e.Message, e.InnerException != null ? e.InnerException.Message : "");
                    }
                }
            }

            else
            {
                Console.WriteLine("\nCan't find any unified groups to add members to.\n");
            }

            // If we created a group, remove it.
            if (createdGroup != null)
            {
                try
                {
                    client.Groups[createdGroup.Id].Request().DeleteAsync().Wait();
                    Console.WriteLine("\nDeleted group {0}", createdGroup.DisplayName);
                }
                catch (Exception e)
                {
                    Console.Write("Couldn't delete group.  Error detail: {0}", e.InnerException.Message);
                }
            }

            // Get first five groups.
            try
            {
                List<Group> groups = client.Groups.Request().GetAsync().Result.Take(5).ToList();

                foreach (Group group in groups)
                {
                    Console.WriteLine("    Group Id: {0}  upn: {1}", group.Id, group.DisplayName);
                    foreach (string type in group.GroupTypes)
                    {
                        if (type == "Unified")
                        {
                            Console.WriteLine(": This is a Unifed Group");
                        }
                    }
                }
            }

            catch (Exception e)
            {
                Console.Write("Couldn't get groups.  Error detail: {0}", e.InnerException.Message);
            }

        }


        public static string GetRandomString(int length = 32)
        {
            //because GUID can't be longer than 32
            return Guid.NewGuid().ToString("N").Substring(0, length > 32 ? 32 : length);
        }
    }
}
