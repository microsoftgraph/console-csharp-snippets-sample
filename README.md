# Microsoft Graph C# Console Snippets App

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

**Application permissions** are used by apps that run without a signed-in user present; you can use this type of permission for apps that run as background services or daemons and that therefore will neither have nor require user consent. Application permissions can only be consented to by a tenant administrator. It is important that you understand that you give this sample a lot of power by providing it admin consent. For example, if you run this sample in **AppMode** against your tenant, you will create a group, add and then remove members of the group, and then delete the group.

If you want to use both types of permissions, you'll need to create and configure two applications in the [Application Registration Portal](https://apps.dev.microsoft.com/), one for **delegated permisssions** and another fro **application permissions**. The sample is structured so that you can configure only one application if you're interested in only one type of permission. Use the **UserMode"** class if you're interested only in **delegated permissions** and the **AppMode** class if you're interested only in **application permissions**.

See [Delegated permissions, Application permissions, and effective permissions](https://developer.microsoft.com/en-us/graph/docs/concepts/permissions_reference#delegated-permissions-application-permissions-and-effective-permissions) for more information about these permission types. Also see [Get access without a user](https://developer.microsoft.com/en-us/graph/docs/concepts/auth_v2_service) for more information on **application permissions** specifically.

## Prerequisites

This sample requires the following:

* [Visual Studio](https://www.visualstudio.com/en-us/downloads)

* An [Office 365 for business account](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account). An Office 365 administrator account is required to run admin-level operations and to consent to application permissions. You can sign up for [an Office 365 Developer subscription](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account) that includes the resources that you need to start building apps.

## Application registration

This sample contains examples that use both delegated permissions and application permissions, so you'll need register the app separately for each scenario.

<a name="Register-the-delegated-permissions-application"></a>
### Register the **delegated permissions** application

1. Navigate to the [Azure Active Directory admin center](https://aad.portal.azure.com). Login using a **personal account** (aka: Microsoft Account) or **Work or School Account**.

2. Select **Azure Active Directory** in the left-hand navigation, then select **App registrations (Preview)** under **Manage**.

3. Select **New registration**. On the **Register an application** page, set the values as follows.

    * Set **Name** to `Console Snippets Sample (Delegated perms)`.
    * Set **Supported account types** to **Accounts in any organizational directory and personal Microsoft accounts**.
    * Leave **Redirect URI** empty.
    ** Choose **Register**.

4. On the **Console Snippets Sample (Delegate perms)** page, copy the values of both the **Application (client) ID** and the **Directory (tenant) ID**. Save these two values, since you will need them later.

5. Select the **Add a Redirect URI** link. On the **Redirect URIs** page, locate the **Suggested Redirect URIs for public clients (mobile, desktop)** section. Select the URI that begins with `msal` **and** the **urn:ietf:wg:oauth:2.0:oob** URI.

6. Open the sample solution in Visual Studio and then open the **Constants.cs** file. Change the **Tenant** string to the **Directory (tenant) ID** value you copied earlier. Change the **ClientIdForUserAuthn** string to the **Application (client) ID** value.

<a name="Register-the-application-permissions-application"></a>
### Register the **application permissions** application

1. Navigate to the [Azure Active Directory admin center](https://aad.portal.azure.com). Login using a **Work or School Account**.

2. Select **Azure Active Directory** in the left-hand navigation, then select **App registrations (Preview)** under **Manage**.

3. Select **New registration**. On the **Register an application** page, set the values as follows.

    * Set **Name** to `Console Snippets Sample (Application perms)`.
    * Set **Supported account types** to **Accounts in any organizational directory**.
    * Leave **Redirect URI** empty.
    * Choose **Register**.

4. On the **Console Snippets Sample (Application perms)** page, copy and save the values for the **Application (client) ID** and the **Directory (tenant) ID**. You will need them in step 7.

5. Select **Certificates & secrets** under **Manage**. Select the **New client secret** button. Enter a value in **Description**, select any option for **Expires** and choose **Add**.

6. Copy the client secret value before leaving the page. You will need it in the next step.

7. Open the sample solution in Visual Studio and then open the **Constants.cs** file. Change the **Tenant** string to the **Directory (tenant) ID** value you copied earlier. Similarly, change the **ClientIdForAppAuthn** string to the **Application (client) ID** value and change the **ClientSecret** string to the client secret value.

8. Return to the Azure Active Directory management center. Select **API permisions** and then select  **Add a permission**. On the panel that appears, choose **Microsoft Graph** and then choose **Application permissions**. 

9. Use the **Select permissions** search box to search for the following permissions: Directory.Read.All, Group.ReadWrite.All, Mail.Read, Mail.ReadWrite, and User.Read.All. Select the check box for each permissions as it appears (note that the permissions will not remain visible in the list as you select each one). Select the **Add permissions** button at the bottom of the panel.

10. Choose the **Grant admin consent for [tenant name]** button. Select **Yes** for the confirmation that appears.

## Build and run the sample

1. Open the sample solution in Visual Studio.
2. Press F5 to build and run the sample. This will restore the NuGet package dependencies and open the console application.
3. Select User mode to run the application with **delegated permissions** only. Select App mode to run the application with **application permissions** only. Select both to run using both types of permissions.
4. When you run User mode, you'll be prompted to sign in with an account on your Office 365 tenant and consent to the permissions that the application requests. If you want to run the groups-related operations in the **UserMode** class, you'll need to uncomment the **GetDetailsForGroups** method in the UserMode.cs file and the **Group.Read.All** scope in the AuthenticationHelper.cs file. After you make those changes only an admin will be able to sign in and consent. Otherwise, you can sign in and consent with a non-admin user.
5. When you run App mode, the application will begin performing a number of common groups-related tasks that only an admin can do. Since you've already authorized the application to make these operations, you won't be prompted to sign in and consent.

## Questions and comments

We'd love to get your feedback about the Microsoft Graph API Console App. You can send your questions and suggestions in the [Issues](https://github.com/microsoftgraph/console-csharp-snippets-sample/issues) section of this repository.

Questions about Microsoft Graph development in general should be posted to [Stack Overflow](https://stackoverflow.com/questions/tagged/microsoftgraph). Make sure that your questions or comments are tagged with [microsoftgraph].

## Contributing

If you'd like to contribute to this sample, see [CONTRIBUTING.MD](/CONTRIBUTING.md).

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
  
## Additional resources

* [Get access without a user](https://developer.microsoft.com/en-us/graph/docs/concepts/auth_v2_service)
* [Delegated permissions, Application permissions, and effective permissions](https://developer.microsoft.com/en-us/graph/docs/concepts/permissions_reference#delegated-permissions-application-permissions-and-effective-permissions)
* [Microsoft Graph](https://developer.microsoft.com/en-us/graph)

## Copyright

Copyright (c) 2017 Microsoft. All rights reserved.
