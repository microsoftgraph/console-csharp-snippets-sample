# Microsoft Graph Console Snippets App

## Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Register the **delegated permissions** application](#Register-the-delegated-permissions-application )
* [Register the **application permissions** application](#Register-the-application-permissions-application )
* [Build and run the sample](#build-and-run-the-sample)
* [Questions and comments](#questions-and-comments)
* [Contributing](#contributing)
* [Additional resources](#additional-resources)

## Introduction

This sample application provides a repository of code snippets that use the Microsoft Graph to perform common tasks, such as sending email, managing groups, and other activities from within a Windows console application. It uses the [Microsoft Graph .NET Client SDK](https://github.com/microsoftgraph/msgraph-sdk-dotnet) to work with data returned by the Microsoft Graph. 

The sample uses the Microsoft Authentication Library (MSAL) for authentication. The sample demonstrates both delegated and application permissions.

**Delegated permissions** are used by apps that have a signed-in user present. For these apps either the user or an administrator consents to the permissions that the app requests and the app is delegated permission to act as the signed-in user when making calls to Microsoft Graph. Some delegated permissions can be consented to by non-administrative users, but some higher-privileged permissions require administrator consent. This application contains some groups-related operations that require administrative consent, and the associated permissions required to do them, are commented by default.

**Application permissions** are used by apps that run without a signed-in user present; you can use this type of permission for apps that run as background services or daemons and that therefore will neither have nor require user consent. Application permissions can only be consented by an administrator. 

If you want to use both types of permissions, you'll need to create and configure two applications in the [Application Registration Portal](https://apps.dev.microsoft.com/), one for **delegated permisssions** and another fro **application permissions**. The sample is structured so that you can configure only one application if you're interested in only one type of permission. Use the **UserMode"** class if you're interested only in **delegated permissions** and the **AppMode** class if you're interested only in **application permissions**.

See [Delegated permissions, Application permissions, and effective permissions](https://developer.microsoft.com/en-us/graph/docs/concepts/permissions_reference#delegated-permissions-application-permissions-and-effective-permissions) for more information about these permission types. Also see [Get access without a user](https://developer.microsoft.com/en-us/graph/docs/concepts/auth_v2_service) for more information on **application permissions** specifically.


## Prerequisites

This sample requires the following:

- [Visual Studio](https://www.visualstudio.com/en-us/downloads) 

-  An [Office 365 for business account](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account). An Office 365 administrator account is required to run admin-level operations and to consent to application permissions. You can sign up for [an Office 365 Developer subscription](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account) that includes the resources that you need to start building apps.

<a name="Register-the-delegated-permissions-application"></a>
## Register the **delegated permissions** application 

1. Sign in to the [Application Registration Portal](https://apps.dev.microsoft.com/) using your Microsoft account.

2. Select **Add an app**, and enter a friendly name for the application (such as **Console App for Microsoft Graph (Delegated perms)**). Click **Create**.

3. ON the application registration page, select "Add Platform." Select the **Native App** tile and save your change. The **delegated permissions** operations in this sample use permissions that are specified in the AuthenticationHelper.cs file. This is why you don't need to assign any permissions to the app on this page.

4. Open the solution and then the Constants.cs file in Visual Studio. 

5. Make the **Application Id** value for this app the value of the **ClientIdForUserAuthn** string.

<a name="Register-the-application-permissions-application"></a>
## Register the **application permissions** application 

1. Sign in to the [Application Registration Portal](https://apps.dev.microsoft.com/) using your Microsoft account.

2. Select **Add an app**, and enter a friendly name for the application (such as **Console App for Microsoft Graph (Application perms)**). Click **Create**.

3. Select the **Web** tile and then after the app is created, click the **Skip the guided setup** link in the top right-hand corner.

4. Under **Application Secrets**, select **Generate New Password**. This will create the value you'll supply for **ClientSecret** in the Constants.cs file.  Be sure to copy this secret/password before closing the popup window that displays it. This is the only time you'll be able to see all of it.

5. Open the solution and then the Constants.cs file in Visual Studio. Make this application secret the value of the **ClientSecret** string. Make the **Application Id** value for this app the value of the **ClientIdForAppAuthn** string. The **Application Id** value appears near the top of the app registration page.

6. Under **Platforms** and **Redirect URLs**, click **Add URL** and add **https://login.microsoftonline.com** as a new redirect URL. This matches the **RedirectUriForAppAuthn** value in this solution's Constants.cs file. Click the **Save** button that appears when you make this change.

7. Under **Microsoft Graph permissions** and **Application permissions**, add the following permissions: Directory.Read.All, Group.ReadWrite.All, Mail.Read, Mail.ReadWrite, and User.Read.All. All of these are "admin-only" permissions that will be used only by the **application permissions** operations in the sample. Click the **Save** button that appears when you make this change.

After you've created  and configured the application, you'll need to force consent manually with an administrator account on your [Office 365 for business account](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account).

To do this, open a browser, and navigate to the following URL, replacing **{application-id}** with the Application ID for your application and **{tenant}** with your tenant Id:

   ```
   https://login.microsoftonline.com/{tenant}/adminconsent?
   client_id={application-id}
   &state=12345
   &redirect_uri=https://login.microsoftonline.com
   ```

After signing in, click **Accept** in the consent page.  You can then close the browser.  Now that you've pre-consented, you can try running the console sample in app mode. 

## Build and run the sample 

1. Open the sample solution in Visual Studio.
2. Press F5 to build and run the sample. This will restore the NuGet package dependencies and open the console application.
3. Select User mode to run the application with **delegated permissions** only. Select App mode to run the application with **application permissions** only. Select both to run using both types of permissions.
4. When you run User mode, you'll be prompted to sign in with an account on your Office 365 tenant and consent to the permissions that the application requests. If you want to run the groups-related operations in the **UserMode** class, you'll need to uncomment the **GetDetailsForGroups** method in the UserMode.cs file and the **Group.Read.All** scope in the AuthenticationHelper.cs file. After you make those changes only an admin will be able to sign in and consent. Otherwise, you can sign in and consent with a non-admin user.
5. When you run App mode, the application will begin performing a number of common groups-related tasks that only an admin can do. Since you've already authorized the application to make these operations, you won't be prompted to sign in and consent.
   
## Questions and comments

We'd love to get your feedback about the Microsoft Graph API Console App. You can send your questions and suggestions in the [Issues](https://github.com/microsoftgraph/console-csharp-snippets-sample/issues) section of this repository.

Questions about Microsoft Graph development in general should be posted to [Stack Overflow](https://stackoverflow.com/questions/tagged/microsoftgraph). Make sure that your questions or comments are tagged with [microsoftgraph].

## Contributing ##

If you'd like to contribute to this sample, see [CONTRIBUTING.MD](/CONTRIBUTING.md).

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
  
## Additional resources

- [Get access without a user](https://developer.microsoft.com/en-us/graph/docs/concepts/auth_v2_service)
- [Delegated permissions, Application permissions, and effective permissions](https://developer.microsoft.com/en-us/graph/docs/concepts/permissions_reference#delegated-permissions-application-permissions-and-effective-permissions)
- [Other Microsoft Graph Connect samples](https://github.com/MicrosoftGraph?utf8=%E2%9C%93&query=-Connect)
- [Microsoft Graph](https://graph.microsoft.io)

## Copyright
Copyright (c) 2017 Microsoft. All rights reserved.
