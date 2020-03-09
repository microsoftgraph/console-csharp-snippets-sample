---
page_type: sample
products:
- ms-graph
languages:
- csharp
description: "この Windows コンソール アプリは、委任されたアクセス許可とアプリケーションのアクセス許可の両方が付与されている Microsoft Graph クライアント ライブラリを使用して、各種操作を実行する方法を示します。このサンプルでは、Microsoft 認証ライブラリ (MSAL) を使用して、Azure AD v 2.0 エンドポイントの認証を行います。"
extensions:
  contentType: samples
  technologies:
  - Microsoft Graph 
  - Microsoft identity platform
  services:
  - Microsoft identity platform
  createdDate: 9/8/2017 1:33:44 PM
---
# Microsoft Graph C# コンソール スニペット アプリ

## 目次

* [はじめに](#introduction)
* [前提条件](#prerequisites)
* [**委任されたアクセス許可**アプリケーションを登録する](#Register-the-delegated-permissions-application )
* [**アプリケーションのアクセス許可**アプリケーションを登録する](#Register-the-application-permissions-application )
* [サンプルのビルドと実行](#build-and-run-the-sample)
* [質問とコメント](#questions-and-comments)
* [投稿](#contributing)
* [その他のリソース](#additional-resources)

## はじめに

このサンプル アプリケーションでは、Windows コンソール アプリ内からのメール送信、グループ管理、および他のアクティビティなどの一般的なタスクを実行するために Microsoft Graph を使用する、コード スニペットのリポジトリを提供しています。[Microsoft Graph .NET クライアント SDK](https://github.com/microsoftgraph/msgraph-sdk-dotnet) を使用して、Microsoft Graph が返すデータを操作します。

サンプルでは Microsoft 認証ライブラリ (MSAL) を使用して認証を行います。このサンプルでは、委任されたアクセス許可とアプリケーションのアクセス許可の両方のデモンストレーションを行います。

**委任されたアクセス許可**は、サインインしているユーザーがいるアプリで使用します。これに該当するアプリの場合は、ユーザーまたは管理者がアプリの要求するアクセス許可に同意します。アプリには、Microsoft Graph の呼び出し時に、サインインしているユーザーとして動作するためのアクセス許可が委任されます。一部の委任されたアクセス許可は非管理ユーザーの同意によって付与されますが、高度な特権が付与されるアクセス許可には管理者の同意が必要になります。このアプリケーションには、管理者の同意を必要とするグループ関連の操作およびこれらの操作用の関連付けられたアクセス許可がいくつか含まれており、コメントが付いています。

**アプリケーションのアクセス許可**は、サインインしているユーザーなしで実行されるアプリで使用されます。この種類のアクセス許可は、バックグラウンド サービスまたはデーモンとして実行されるアプリで使用できます。このため、ユーザーの同意が付与されておらず、同意を必要ともしないアプリで使用できます。アプリケーションのアクセス許可への同意は、テナント管理者のみが行えます。このサンプルに管理者の同意を付与することにより、サンプルに大きな権限が付与されることを理解しておくことが重要です。たとえば、テナントに対してこのサンプルを **AppMode** で実行する場合、グループを作成し、グループのメンバーの追加に続いて削除を行い、グループの削除を行うことになります。

両方の種類のアクセス許可を使用する場合は、[Azure Active Directory 管理センター](https://aad.portal.azure.com)でアプリケーションを 2 つ (**委任されたアクセス許可**用に 1 つ、**アプリケーションのアクセス許可**用にもう 1 つ) 作成して構成する必要があります。このサンプルは、1 種類のアクセス許可のみを使用することを希望する場合は 1 つのアプリケーションのみを構成できる構造になっています。**委任されたアクセス許可**のみを希望する場合は **UserMode** クラスを使用し、**アプリケーションのアクセス許可**のみを希望する場合は **AppMode** クラスを使用します。

これらのアクセス許可の種類の詳細については、「[委任されたアクセス許可、アプリケーションのアクセス許可、有効なアクセス許可](https://docs.microsoft.com/en-us/graph/permissions-reference#delegated-permissions-application-permissions-and-effective-permissions)」を参照してください。また、特に**アプリケーションのアクセス許可**については「[ユーザーなしでアクセスする](https://docs.microsoft.com/en-us/graph/auth-v2-service)」も参照してください。

## 前提条件

このサンプルを実行するには次のものが必要です。

* [Visual Studio](https://www.visualstudio.com/en-us/downloads)

* [一般法人向け Office 365 のアカウント](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account)。管理者レベルの操作を実行し、アプリケーションのアクセス許可に同意するには、Office 365 の管理者アカウントが必要です。アプリのビルドを開始するために必要なリソースを含む、[Office 365 Developer サブスクリプション](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account)にサインアップできます。

## アプリケーションの登録

このサンプルには、委任されたアクセス許可とアプリケーションのアクセス許可の両方を使用する例が含まれているため、シナリオごとにアプリを個別に登録する必要があります。

<a name="Register-the-delegated-permissions-application"></a>
### **委任されたアクセス許可**アプリケーションを登録する

1. [Azure Active Directory 管理センター](https://aad.portal.azure.com)に移動します。アプリ登録を作成するためのアクセス許可が付与されている**個人用アカウント** (別名:Microsoft アカウントか**職場または学校のアカウント**を使用してログインします。

2. 左側のナビゲーションで [**Azure Active Directory**] を選択し、次に [**管理**] で [**アプリの登録 (プレビュー)**] を選択します。

3. [**新規登録**] を選択します。[**アプリケーションの登録**] ページで、次のように値を設定します。

    * [**名前**] を "`Console Snippets Sample (Delegated perms)`" に設定します。
    * [**サポート対象のアカウントの種類**] を [**任意の組織のディレクトリ内のアカウントと、個人用の Microsoft アカウント**] に設定します。
    * [**リダイレクト URI**] は空のままにします。.
	** [**登録**] を選択します。

4. [**Console Snippets Sample (Delegate perms)**] ページで、[**アプリケーション (クライアント) ID**] と [**Directory (テナント) ID**] の両方の値をコピーします。これら 2 つの値は後で必要になるため、保存します。

5. [**リダイレクト URI の追加**] リンク を選択します。[**リダイレクト URI**] ページで [**パブリック クライアント (モバイル、デスクトップ) に推奨されるリダイレクト URI**] セクションを見つけます。"`msal`" から始まる URI **および** "**urn:ietf:wg:oauth:2.0:oob**" という URI を選択します。

6. サンプル ソリューションを Visual Studio で開き、ファイル **Constants.cs** を開きます。[**テナント**] 文字列を先ほどコピーした **Directory (テナント) ID** 値に変更します。[**ClientIdForUserAuthn**] 文字列を **アプリケーション (クライアント) ID** 値に変更します。

<a name="Register-the-application-permissions-application"></a>
### **アプリケーションのアクセス許可**アプリケーションを登録する

1. [Azure Active Directory 管理センター](https://aad.portal.azure.com)に移動します。**職場または学校のアカウント**を使用してログインします。

2. 左側のナビゲーションで [**Azure Active Directory**] を選択し、次に [**管理**] で [**アプリの登録 (プレビュー)**] を選択します。

3. [**新規登録**] を選択します。[**アプリケーションの登録**] ページで、次のように値を設定します。

    * [**名前**] を "`Console Snippets Sample (Application perms)`" に設定します。
    * [**サポートされているアカウントの種類**] を [**任意の組織のディレクトリ内のアカウント**] に設定します。
    * [**リダイレクト URI**] は空のままにします。
    * [**登録**] を選択します。

4. [**Console Snippets Sample (Application perms)**] ページで、[**アプリケーション (クライアント) ID**] と [**Directory (テナント) ID**] の値をコピーして保存します。これらの値は、手順 7 で必要になります。

5. [**管理**] で [**証明書とシークレット**] を選択します。[**新しいクライアント シークレット**] ボタンを選択します。[**説明**] に値を入力し、[**有効期限**] で任意のオプションを選び、[**追加**] を選択します。

6. このページを離れる前に、クライアント シークレットの値をコピーします。この値は次の手順で必要になります。

7. サンプル ソリューションを Visual Studio で開き、ファイル **Constants.cs** を開きます。[**テナント**] 文字列を先ほどコピーした **Directory (テナント) ID** 値に変更します。同様に、[**ClientIdForAppAuthn**] 文字列を**アプリケーション (クライアント) ID** 値に変更し、[**ClientSecret**] 文字列をクライアント シークレット値に変更します。

8. Azure Active Directory 管理センターに戻ります。[**API のアクセス許可**]、[**アクセス許可の追加**] の順に選択します。表示されるパネルで、[**Microsoft Graph**] を選択し、[**アプリケーションのアクセス許可**] を選択します。 

9. [**アクセス許可の選択**] 検索ボックスを使用して次のアクセス許可を検索します。Directory.Read.All、Group.ReadWrite.All、Mail.Read、Mail.ReadWrite、および User.Read.All。それぞれのアクセス許可が表示されるたびに、チェック ボックスをオンにします (それぞれのアクセス許可を選択すると、そのアクセス許可は一覧の表示から消えます)。パネル下部にある [**アクセス許可の追加**] ボタンを選択します。

10. [**[テナント名] に管理者の同意を与えます**] ボタンを選択します。確認メッセージが表示されたら、[**はい**] を選択します。

## サンプルのビルドと実行

1. サンプル ソリューションを Visual Studio で開きます。
2. F5 キーを押して、サンプルをビルドして実行します。これにより、NuGet パッケージの依存関係が復元され、コンソール アプリケーションが開きます。
3. **委任されたアクセス許可**のみでアプリケーションを実行するには [ユーザー] モードを選択します。**アプリケーションのアクセス許可**のみでアプリケーションを実行するには、[アプリ] モードを選択します。両方のアクセス許可の種類を使用する場合は、両方のモードを選択します。
4. ユーザー モードを実行すると、Office 365 テナントのアカウントでサインインして、アプリケーションが要求するアクセス許可に同意するように求められます。グループ関連の操作を **UserMode** クラスで実行する場合は、**GetDetailsForGroups** メソッドを UserMode.cs ファイルでコメント解除し、**Group.Read.All** スコープを AuthenticationHelper.cs ファイルでコメント解除する必要があります。変更後は、管理者のみがサインインと同意を行えるようになります。変更を行わない場合、管理者以外のユーザーがサインインと同意を行えます。
5. アプリ モードを実行すると、管理者のみが実行できる一般的なグループ関連のタスクをアプリケーションが実行し始めます。これらの操作をアプリケーションが行うことを既に承認しているため、サインインと同意を求めるメッセージは表示されません。

## 質問とコメント

Microsoft Graph API コンソール アプリに関するフィードバックをぜひお寄せください。質問や提案は、このリポジトリの「[問題](https://github.com/microsoftgraph/console-csharp-snippets-sample/issues)」セクションで送信できます。

Microsoft Graph 開発全般の質問については、「[Stack Overflow](https://stackoverflow.com/questions/tagged/microsoftgraph)」に投稿してください。質問やコメントには、必ず "microsoftgraph" とタグを付けてください。

## 投稿

このサンプルに投稿する場合は、[CONTRIBUTING.MD](/CONTRIBUTING.md) を参照してください。

このプロジェクトでは、[Microsoft Open Source Code of Conduct (Microsoft オープン ソース倫理規定)](https://opensource.microsoft.com/codeofconduct/) が採用されています。詳細については、「[Code of Conduct の FAQ](https://opensource.microsoft.com/codeofconduct/faq/)」を参照してください。また、その他の質問やコメントがあれば、[opencode@microsoft.com](mailto:opencode@microsoft.com) までお問い合わせください。
  
## その他のリソース

* [ユーザーなしでアクセスする](https://docs.microsoft.com/en-us/graph/auth-v2-service)
* [委任されたアクセス許可、アプリケーションのアクセス許可、有効なアクセス許可](https://docs.microsoft.com/en-us/graph/permissions-reference#delegated-permissions-application-permissions-and-effective-permissions)
* [Microsoft Graph](https://developer.microsoft.com/en-us/graph)

## 著作権

Copyright (c) 2017 Microsoft.All rights reserved.
