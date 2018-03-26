using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace console_csharp_snippets_sample
{
    internal class UserMode
    {
        public static GraphServiceClient client;
        public static void UserModeRequests()
        {
            try
            {
                //*********************************************************************
                // setup Microsoft Graph Client for user.
                //*********************************************************************
                if (Constants.ClientIdForUserAuthn != "ENTER_YOUR_CLIENT_ID")
                {
                    client = AuthenticationHelper.GetAuthenticatedClientForUser();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You haven't configured a value for ClientIdForUserAuthn in Constants.cs. Please follow the Readme instructions for configuring this application.");
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

            Console.WriteLine("\nStarting user-mode requests...");
            Console.WriteLine("\n=============================\n\n");

            // GET current user

            try
            {
                User user = client.Me.Request().GetAsync().Result;
                Console.WriteLine("Current user:    Id: {0}  UPN: {1}", user.Id, user.UserPrincipalName);
            }

            catch (Exception e)
            {
                Console.WriteLine("\nError getting /me user {0} {1}",
                     e.Message, e.InnerException != null ? e.InnerException.Message : "");
            }

            // Get current user photo (work or school accounts only). Returns an exception if no photo exists.

            try
            {
                Stream userPhoto = client.Me.Photo.Content.Request().GetAsync().Result;
                Console.WriteLine("Got stream photo");
            }

            catch (Exception e)
            {
                Console.WriteLine("\nError getting user photo {0} {1}",
                     e.Message, e.InnerException != null ? e.InnerException.Message : "");
            }

            // Get current user's direct reports (work or school accounts only)
            try
            {

                IUserDirectReportsCollectionWithReferencesPage directReports = client.Me.DirectReports.Request().GetAsync().Result;

                if (directReports.Count == 0)
                {
                    Console.WriteLine("      no reports");
                }
                else
                {
                    foreach (User user in directReports)
                    {
                        Console.WriteLine("      Id: {0}  UPN: {1}", user.Id, user.UserPrincipalName);
                    }
                }

            }

            catch (Exception e)
            {
                Console.WriteLine("\nError getting directReports {0} {1}",
                     e.Message, e.InnerException != null ? e.InnerException.Message : "");
            }

            // Get current user's manager (work or school accounts only). Returns an exception if no manager exists.
            try
            {
                DirectoryObject currentUserManager = client.Me.Manager.Request().GetAsync().Result;

                User user = client.Users[currentUserManager.Id].Request().GetAsync().Result;
                Console.WriteLine("\nManager      Id: {0}  UPN: {1}", user.Id, user.UserPrincipalName);
            }

            catch (Exception e)
            {
                Console.WriteLine("\nError getting manager {0} {1}",
                     e.Message, e.InnerException != null ? e.InnerException.Message : "");
            }

            // Get current user's files

            try
            {
                List<DriveItem> items = client.Me.Drive.Root.Children.Request().GetAsync().Result.Take(5).ToList();

                foreach (DriveItem item in items)
                {

                    if (item.File != null)
                    {
                        Console.WriteLine("    This is a folder: Id: {0}  WebUrl: {1}", item.Id, item.WebUrl);
                    }
                    else
                    {

                        Console.WriteLine("    File Id: {0}  WebUrl: {1}", item.Id, item.WebUrl);
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\nError getting files {0} {1}",
                     e.Message, e.InnerException != null ? e.InnerException.Message : "");
            }

            // Get current user's messages

            try
            {
                List<Message> messages = client.Me.Messages.Request().GetAsync().Result.Take(5).ToList();

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


            // Get current user's events
            try
            {
                List<Event> events = client.Me.Events.Request().GetAsync().Result.Take(5).ToList();

                if (events.Count == 0)
                {
                    Console.WriteLine("    no events scheduled");
                }
                foreach (Event _event in events)
                {
                    Console.WriteLine("    Event: {0} starts {1} ", _event.Subject, _event.Start);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\nError getting events {0} {1}",
                     e.Message, e.InnerException != null ? e.InnerException.Message : "");
            }

            // Get current user's contacts
            try
            {
                List<Contact> contacts = client.Me.Contacts.Request().GetAsync().Result.Take(5).ToList();

                if (contacts.Count == 0)
                {
                    Console.WriteLine("    You don't have any contacts");
                }
                foreach (Contact contact in contacts)
                {
                    Console.WriteLine("    Contact: {0} ", contact.DisplayName);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\nError getting contacts {0} {1}",
                     e.Message, e.InnerException != null ? e.InnerException.Message : "");
            }

            // Create a recipient list.
            IList<Recipient> messageToList = new List<Recipient>();

            // Get 10 users
            List<User> users = client.Users.Request().GetAsync().Result.Take(10).ToList();
            foreach (User user in users)
            {
                Recipient messageTo = new Recipient();
                EmailAddress emailAdress = new EmailAddress();
                emailAdress.Address = user.UserPrincipalName;
                emailAdress.Name = user.DisplayName;
                messageTo.EmailAddress = emailAdress;

                // Only uncomment this next line if you want to send this mail to the 
                // first 10 accounts returned by a call to the graph.microsoft.com/v1.0/users endpoint.
                // Otherwise, this email will only be sent to the current user.

                //messageToList.Add(messageTo);
            }

            // Get current user
            User currentUser = client.Me.Request().GetAsync().Result;

            Recipient currentUserRecipient = new Recipient();
            EmailAddress currentUserEmailAdress = new EmailAddress();
            currentUserEmailAdress.Address = currentUser.UserPrincipalName;
            currentUserEmailAdress.Name = currentUser.DisplayName;
            currentUserRecipient.EmailAddress = currentUserEmailAdress;
            messageToList.Add(currentUserRecipient);

            // Send mail to signed in user and the recipient list

            Console.WriteLine();
            Console.WriteLine("Sending mail....");
            Console.WriteLine();

            try
            {
                ItemBody messageBody = new ItemBody();
                messageBody.Content = "<report pending>";
                messageBody.ContentType = BodyType.Text;

                Message newMessage = new Message();
                newMessage.Subject = "\nCompleted test run from console app.";
                newMessage.ToRecipients = messageToList;
                newMessage.Body = messageBody;

                client.Me.SendMail(newMessage, true).Request().PostAsync();
                Console.WriteLine("\nMail sent to {0}", currentUser.DisplayName);
            }
            catch (Exception)
            {
                Console.WriteLine("\nUnexpected Error attempting to send an email");
                throw;
            }

            // The operations in this method require admin-level consent. Uncomment this line
            // if you want to run the sample with a non-admin account.
            // You'll also need to uncomment the Group.Read.All permission scope in AuthenticationHelper.cs
            //GetDetailsForGroups();

        }

        public static void GetDetailsForGroups()
        {
            // Get the first 3 UNIFIED groups and view their associated content
            List<Group> unifiedGroupsForEnumerating = null;
            try
            {
                string unifiedFilter = "groupTypes/any(gt:gt+eq+'Unified')";
                unifiedGroupsForEnumerating = client.Groups.Request().Filter(unifiedFilter).GetAsync().Result.Take(3).ToList();

                foreach (Group group in unifiedGroupsForEnumerating)
                {
                    Console.WriteLine("    Unified Group: {0}", group.DisplayName);

                    // Get group members
                    try
                    {
                        // get group members
                        List<DirectoryObject> unifiedGroupMembers = client.Groups[group.Id].Members.Request().GetAsync().Result.CurrentPage.ToList();
                        if (unifiedGroupMembers.Count == 0)
                        {
                            Console.WriteLine("      no members for group");
                        }
                        foreach (DirectoryObject member in unifiedGroupMembers)
                        {
                            if (member is User)
                            {
                                User memberUser = (User)member;
                                Console.WriteLine("        User: {0} ", memberUser.DisplayName);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write("Unexpected exception when enumerating group members.  Error detail: {0}", e.InnerException.Message);
                    }

                    // Get group files
                    try
                    {
                        IList<DriveItem> unifiedGroupFiles = client.Groups[group.Id].Drive.Root.Children.Request().GetAsync().Result.Take(5).ToList();
                        if (unifiedGroupFiles.Count == 0)
                        {
                            Console.WriteLine("      no files for group");
                        }
                        foreach (DriveItem file in unifiedGroupFiles)
                        {
                            Console.WriteLine("        file: {0} url: {1}", file.Name, file.WebUrl);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write("Unexpected exception when enumerating group files. Error detail: {0}", e.InnerException.Message);
                    }

                    // Get group conversations
                    try
                    {
                        List<Conversation> unifiedGroupConversations = client.Groups[group.Id].Conversations.Request().GetAsync().Result.CurrentPage.ToList();
                        if (unifiedGroupConversations.Count == 0)
                        {
                            Console.WriteLine("      no conversations for group");
                        }
                        foreach (Conversation conversation in unifiedGroupConversations)
                        {
                            Console.WriteLine("        conversation topic: {0} ", conversation.Topic);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write("Unexpected exception when enumerating group conversations. Error detail: {0}", e.InnerException.Message);
                    }

                    // Get group events
                    try
                    {
                        List<Event> unifiedGroupEvents = client.Groups[group.Id].Events.Request().GetAsync().Result.CurrentPage.ToList();
                        if (unifiedGroupEvents.Count == 0)
                        {
                            Console.WriteLine("      no meeting events for group");
                        }
                        foreach (Event _event in unifiedGroupEvents)
                        {
                            Console.WriteLine("        meeting event subject: {0} ", _event.Subject);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write("Unexpected exception when enumerating group events. Error detail: {0}", e.InnerException.Message);
                    }

                }


            }

            catch (Exception e)
            {
                Console.Write("Couldn't get unified groups.  Error detail: {0}", e.InnerException.Message);
            }
        }
    }
}
