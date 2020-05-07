---
page_type: sample
products:
- ms-graph
languages:
- csharp
description: "此 Windows 控制台应用演示如何使用具有委派权限和应用程序权限的 Microsoft Graph 客户端库执行各种操作。此示例使用 Microsoft 身份验证库 (MSAL) 在 Azure AD v2.0 终结点上进行身份验证。"
extensions:
  contentType: samples
  technologies:
  - Microsoft Graph 
  - Microsoft identity platform
  services:
  - Microsoft identity platform
  createdDate: 9/8/2017 1:33:44 PM
---
# Microsoft Graph C# Console Snippets 应用程序

## 目录

* [简介](#introduction)
* [先决条件](#prerequisites)
* [注册 **委派权限**应用程序](#Register-the-delegated-permissions-application )
* [注册 **应用权限**应用程序](#Register-the-application-permissions-application )
* [生成和运行示例](#build-and-run-the-sample)
* [问题和意见](#questions-and-comments)
* [参与](#contributing)
* [其他资源](#additional-resources)

## 简介

此示例应用提供使用 Microsoft Graph 执行常见任务的代码段存储库，例如发送电子邮件、管理组和 Windows 控制台应用内的其他活动。它使用 [Microsoft Graph .NET Client SDK](https://github.com/microsoftgraph/msgraph-sdk-dotnet) 以结合使用由 Microsoft Graph 返回的数据。

此示例使用 Microsoft 身份验证库 (MSAL) 进行身份验证。此示例演示了委派权限和应用程序权限。

**委派权限**供已有用户登录的应用使用。对于这些应用，用户或管理员同意应用请求的权限，并向应用委派调用 Microsoft Graph 时代表已登录用户的权限。某些委派权限可以由非管理用户同意，但一些较高特权权限需要管理员同意。此应用程序包含需要管理员许可的部分组相关操作，并默认备注执行操作的相关权限。

**应用程序权限**由无需用户登录就可运行的应用程序使用，因此可针对应用程序使用这种类型的权限，以作为后台服务或守护程序运行，而且无需用户许可或需要用户许可。应用程序权限只能由租户管理员进行许可。务必了解，通过提供管理员许可，为此示例提供了诸多权限。例如，如果针对租户在**应用程序模式**中运行此示例，将创建组，添加并随后删除组成员，并随后删除组。

如果希望使用两种类型的权限，将需要在[Azure Active Directory 管理中心](https://aad.portal.azure.com)中创建并配置两个应用程序，一个用于**委派权限**，另一个用于**应用程序权限**。示例采用结构化设计，因此如果关注一种类型的权限，只能配置一个应用程序。如果只关注“**委派权限**”，使用“**用户模式**”，如果只关注“**应用程序权限**”，使用“**应用程序模式**”。

有关权限类型的更多信息，请参阅[“委派权限、应用程序权限和有效权限”](https://docs.microsoft.com/en-us/graph/permissions-reference#delegated-permissions-application-permissions-and-effective-permissions)。请参阅“[无用户访问](https://docs.microsoft.com/en-us/graph/auth-v2-service)”，具体了解有关“**应用程序权限**”的详细信息。

## 先决条件

此示例要求如下：

* [Visual Studio](https://www.visualstudio.com/en-us/downloads)

* [Office 365 商业版帐户](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account)。需要 Office 365 管理员帐户才能运行管理员级别的操作以及许可应用程序权限。可以注册 [Office 365 开发人员订阅](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account)，其中包含开始生成应用所需的资源。

## 应用程序注册

此示例包含使用委派权限和应用程序权限的两个示例，因此需要针对每个方案分别注册应用程序。

<a name="Register-the-delegated-permissions-application"></a>
### 注册**委派权限**应用程序

1. 导航至 [Azure Active Directory 管理中心](https://aad.portal.azure.com)。使用具有应用注册创建权限的**个人帐户**（也称为：“Microsoft 帐户”）或**工作/学校帐户**登录。

2. 选择左侧导航栏中的“**Azure Active Directory**，再选择“**管理**”下的“**应用注册(预览版)**”。

3. 选择“**新注册**”。在“**注册应用**”页上，按如下方式设置值。

    * 将“**名称**”设定为“`控制台代码片段示例（委派权限）`”。
    * 将“**受支持的帐户类型**”设置为“**任何组织目录中的帐户和个人 Microsoft 帐户**”。
    * 保留“**重定向 URI**”为空。
	** 选择“**注册**”。

4. 在“**控制台代码片段示例（委派权限）**”页面上，复制“**应用程序（客户端）ID**”值和“**目录（租户）ID**”值。保存这两个值，因为随后需要它们。

5. 选择“**添加重定向 URI**” 链接。在“**重定向 URL**”页面上，找到“**建议用于公共客户端（移动、桌面）的重定向 URI**”部分。选择以 `msal` **为开头的 URL 和** **urn:ietf:wg:oauth:2.0:oob** URI。

6. 在 Visual Studio 中打开示例解决方案，然后打开 **Constants.cs** 文件。将“**租户**”字符串更改为之前复制的 “**目录（租户） ID**” 值。更改 **ClientIdForUserAuthn** 字符串为 “**应用程序（客户端）ID**”值。

<a name="Register-the-application-permissions-application"></a>
### 注册**应用权限**应用程序

1. 导航至 [Azure Active Directory 管理中心](https://aad.portal.azure.com)。使用**工作或学校帐户**登录。

2. 选择左侧导航栏中的“**Azure Active Directory**，再选择“**管理**”下的“**应用注册(预览版)**”。

3. 选择“**新注册**”。在“**注册应用**”页上，按如下方式设置值。

    * 将“**名称**”设定为“`控制台代码片段示例（应用程序权限）`”。
    * 将**“受支持的帐户类型”**设置为**“任何组织目录中的帐户”**。
    * 保留“**重定向 URI**”为空。
    * 选择“**注册**”。

4. 在“**控制台代码片段示例（应用程序权限）**”页面上，复制并保存“**应用程序（客户端）ID**”值和“**目录（租户）ID**”值。在第 7 步上将需要它们。

5. 选择“**管理**”下的“**证书和密码**”。选择“**新客户端密码**”按钮。在“**说明**”中输入数值，随后选择任意“**过期**”选项并选择“**添加**”。

6. 离开页面前，先复制客户端密码值。将在下一步中用到它。

7. 在 Visual Studio 中打开示例解决方案，然后打开 **Constants.cs** 文件。将“**租户**”字符串更改为之前复制的 “**目录（租户） ID**” 值。同样，将 **ClientIdForAppAuthn** 字符串更改为 **应用程序（客户端） ID** 值，并将 **ClientSecret** 字符串更改为客户端密码值。

8. 返回 Azure Active Directory 管理中心。选择“**API 权限**”并随后选择“**添加权限**”。在出现的面板上，选择 **Microsoft Graph** 并随后选择“**应用程序权限”**。 

9. 使用“**选择权限**”搜索框来搜索下列权限：Directory.Read.All、Group.ReadWrite.All、Mail.Read、Mail.ReadWrite、和 User.Read.All。在每个权限显示时，选择其复选框（请注意，选择每个权限后，它不会在列表中保持可见）。选择窗格下方选择“**添加权限**”。

10. 选择“**为\[租户名称]授予管理员许可**”按钮。选择“**是**”确认显示的内容。

## 生成和运行示例

1. 在 Visual Studio 中打开示例解决方案。
2. 按 F5 生成和运行此示例。这将还原 NuGet 包依赖项，并打开控制台应用程序。
3. 选择“用户模式”，以只使用“**委派权限**”运行应用程序。选择“应用程序模式”，以只使用“**应用程序权限**”运行应用程序。选择“两者”，以使用两种类型权限运行。
4. 运行“用户模式”时，系统将提示使用 Office 365 租户帐户登录，并同意应用程序请求的权限。如果以**用户模式**级运行组相关操作，需要对 UserMode.cs 文件中的 **GetDetailsForGroups** 方法和 AuthenticationHelper.cs 文件中的 **Group.Read.All** 范围取消注释。执行这些更改后，只有管理员才能登录并许可。否则，只能以非管理员用户登录并许可。
5. 运行“应用程序模式”时，应用程序开始运行大量只有管理员才能执行的组相关常规任务。由于已授权应用程序执行这些操作，因此不会提示登录和许可。

## 问题和意见

我们乐意倾听你有关 Microsoft Graph API Console 应用程序的反馈。你可以在该存储库中的“[问题](https://github.com/microsoftgraph/console-csharp-snippets-sample/issues)”部分将问题和建议发送给我们。

与 Microsoft Graph 开发相关的一般问题应发布到 [Stack Overflow](https://stackoverflow.com/questions/tagged/microsoftgraph)。请确保你的问题或意见标记有 \[microsoftgraph]。

## 参与

如果想要参与本示例，请参阅 [CONTRIBUTING.MD](/CONTRIBUTING.md)。

此项目已采用 [Microsoft 开放源代码行为准则](https://opensource.microsoft.com/codeofconduct/)。有关详细信息，请参阅[行为准则常见问题解答](https://opensource.microsoft.com/codeofconduct/faq/)。如有其他任何问题或意见，也可联系 [opencode@microsoft.com](mailto:opencode@microsoft.com)。
  
## 其他资源

* [无用户访问](https://docs.microsoft.com/en-us/graph/auth-v2-service)
* [委派权限、应用程序权限和有效权限](https://docs.microsoft.com/en-us/graph/permissions-reference#delegated-permissions-application-permissions-and-effective-permissions)
* [Microsoft Graph](https://developer.microsoft.com/en-us/graph)

## 版权信息

版权所有 (c) 2017 Microsoft。保留所有权利。
