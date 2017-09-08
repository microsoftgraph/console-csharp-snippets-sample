# Microsoft Graph API Console App

Console App using Microsoft Graph Client Library.

This console application is a .NET sample, using the Microsoft Graph Client library. It demonstrates common calls to the Graph API including Getting Users, Groups, Group Membership, and Mail. The sample app demonstrates both delegated authentication (user mode, requiring a user to sign in to the app and application authentication (app mode, where the app access is made using the application's identity without a user needing to be present).  When running in user mode, some operations that the sample performs will **only** be possible if the signed-in user is a company administrator.

The sample app demonstrates both delegated authentication (user mode, requiring a user to sign in to the app and application authentication (app mode, where the app access is made using the application's identity without a user needing to be present). When running in user mode, some operations that the sample performs will only be possible if the signed-in user is a company administrator.

The sample uses the Microsoft Authentication Library (MSAL) for authentication. It requires you to configure two applications in the Application Registration Portal: one using *application permissions* or OAuth Client Credentials, and a second one using user *delegated permissions* - to execute update operations, you will need to sign-in with an account that has Administrative permissions. Configuring the console sample against your own tenant is described in Step 3 below).


## Step 1: Clone or download this repository


## Step 2: Run the sample in Visual Studio
The sample app is preconfigured to read data from an Office 365 tenant. 
Run the sample application by selecting F5, selecting to run in both modes (user and app mode).  The app will require Admin credentials to perform *all* operations.

## Step 3: Running this application
Register the Sample app for your own tenant 

1. Sign in to the [Application Registration Portal](https://apps.dev.microsoft.com/) using your Microsoft account.

3. Click **Add an app**, and enter a friendly name for the application, for example **Console App for Microsoft Graph**. Click **Create**.

4. Click **Service and Daemon App** and **Skip the guided setup**.


5. Under **Application Secrets**, click **Generate New Password**. Be sure to copy this secret/password before closing the popup window that displays it. This is the only time you'll be able to see all of it.

6. Open the solution and then the Constants.cs file in Visual Studio. Make this application secret the value of the **ClientSecret** string. Make the **Application Id** value for this app the value of the **ClientIdForAppAuthn** string. The **Application Id** value appears near the top of the app registration page.

7. Under **Platforms** and **Redirect URLs**, click **Add URL** and add **https://login.microsoftonline.com** as a new redirect URL. This matches the **RedirectUriForAppAuthn** value in this solution's Constants.cs file. Click the **Save** button that appears when you make this change.

8. Under **Microsoft Graph permissions** and **Application permissions**, add the following permissions: Directory.ReadAll, Group.ReadWrite.All, Mail.Read, Mail.ReadWrite, and User.Read.All. All of these are "admin-only" permissions. Click the **Save** button that appears when you make this change.

9. Now we'll need to configure a second application for the user mode portion of the console app. Follow steps 1-3, and then click **Mobile and Desktop App** before clicking **Skip the guided setup**.

10. Make the **Application Id** value for this app the value of the **ClientIdForUserAuthn** string.

14. Build and run your application.  
+ You might run into some "missing assembly reference?" errors when you build. Make sure NuGet package restore is enabled, and that the packages in packages.config are installed. Sometimes, Visual Studio doesn't immediately find the package, so try building again. If all else fails, you can add the references manually.
+ When you run it, select user mode the first time. You will need to authenticate with valid tenant administrator credentials for your company when you run the application (required for the Create/Update/Delete operations), and consent the first time you use the sample app.
+ If you want to run the console app in app mode, you'll need to force consent manually, beforehand. Here, an admin user will need to consent.  You can force consent by opening a browser, and going to the following URL, replacing **{app-mode-application-id}** with the Application ID for your app mode application and **{tenant}** with your tenant Id:
   ```
   https://login.microsoftonline.com/{tenant}/adminconsent?
   client_id={app-mode-application-id}
   &state=12345
   &redirect_uri=https://login.microsoftonline.com
   ```
After signing in, click **Accept** in the consent page.  You can then close the browser.  Now that you've pre-consented, you can try running the console sample in app mode.

## Questions and comments

We'd love to get your feedback about the Microsoft Graph API Console App. You can send your questions and suggestions in the [Issues](https://github.com/microsoftgraph/console-csharp-snippets-sample/issues) section of this repository.

Questions about Microsoft Graph development in general should be posted to [Stack Overflow](https://stackoverflow.com/questions/tagged/microsoftgraph). Make sure that your questions or comments are tagged with [microsoftgraph].

## Contributing ##

If you'd like to contribute to this sample, see [CONTRIBUTING.MD](/CONTRIBUTING.md).

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
  
## Additional resources

- [Get access without a user](https://developer.microsoft.com/en-us/graph/docs/concepts/auth_v2_service)
- [Other Microsoft Graph Connect samples](https://github.com/MicrosoftGraph?utf8=%E2%9C%93&query=-Connect)
- [Microsoft Graph](https://graph.microsoft.io)

## Copyright
Copyright (c) 2017 Microsoft. All rights reserved.
