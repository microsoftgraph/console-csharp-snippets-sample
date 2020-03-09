---
page_type: sample
products:
- ms-graph
languages:
- csharp
description: "Cette application de console Windows montre comment effectuer diverses opérations à l’aide de la bibliothèque cliente Microsoft Graph avec des autorisations d’application et des autorisations déléguées. Cet exemple utilise la Bibliothèque d'authentification Microsoft (MSAL) pour l’authentification sur le point de terminaison Azure AD v2.0."
extensions:
  contentType: samples
  technologies:
  - Microsoft Graph 
  - Microsoft identity platform
  services:
  - Microsoft identity platform
  createdDate: 9/8/2017 1:33:44 PM
---
# Microsoft Graph C# : extraits de console de l'application

## Table des matières

* [Introduction](#introduction)
* [Conditions préalables](#prerequisites)
* [Inscrire les **autorisations déléguées** de l'application](#Register-the-delegated-permissions-application )
* [Inscrire les **autorisations d'application** de l'application](#Register-the-application-permissions-application )
* [Créer et exécuter l’exemple](#build-and-run-the-sample)
* [Questions et commentaires](#questions-and-comments)
* [Contribution](#contributing)
* [Ressources supplémentaires](#additional-resources)

## Introduction

Cet exemple d'application constitue un référentiel des extraits de code qui utilisent Microsoft Graph pour effectuer des tâches courantes, telles que l’envoi de messages électroniques, la gestion de groupes et d’autres activités au sein d’une application de console Windows. Il utilise le [Kit de développement logiciel Microsoft Graph .NET Client](https://github.com/microsoftgraph/msgraph-sdk-dotnet) pour travailler avec les données renvoyées par Microsoft Graph.

L’exemple utilise la Bibliothèque d’authentification Microsoft (MSAL) pour l’authentification. L’exemple illustre des autorisations déléguées et des autorisations d’application.

Les **Autorisations déléguées** sont utilisées par les applications qui ont un utilisateur connecté présent. Pour ces applications, l’utilisateur ou un administrateur accepte les autorisations demandées par l’application et cette dernière obtient l’autorisation déléguée pour agir en tant qu’utilisateur connecté lors d'appels vers Microsoft Graph. Certaines autorisations déléguées peuvent être consenties par des utilisateurs non administrateurs, mais certaines autorisations ayant des privilèges plus élevés nécessitent le consentement de l’administrateur. Cette application inclut certaines opérations liées aux groupes nécessitant un consentement administratif et les autorisations associées requises pour les effectuer sont commentées par défaut.

Les **Autorisations d’application** sont utilisées par des applications qui s’exécutent sans la présence d'une connexion utilisateur. Vous pouvez utiliser ce type d’autorisation pour des applications s'exécutant en tant que services en arrière-plan et qui, par conséquent, n’ont pas et n'exigent pas l'accord de l'utilisateur. Les autorisations d’application peuvent uniquement être accordées par un administrateur client. Il est important de prendre conscience que vous donnez beaucoup de pouvoir à cet échantillon en lui octroyant un accord de l’administrateur informatique. Par exemple, si vous exécutez cet exemple dans **AppMode** chez votre client, vous créez un groupe, ajoutez, puis supprimez des membres du groupe, et supprimez ensuite ce groupe.

Si vous souhaitez utiliser ces deux types d’autorisations, vous devez créer et configurer deux applications dans le [Centre d'administration Azure Active Directory](https://aad.portal.azure.com), une pour les **autorisations déléguées** et une autre pour les **autorisations d’application**. L’exemple est organisé de sorte que vous ne puissiez configurer qu’une seule application si vous n’avez besoin que d’un type d’autorisation. Utilisez la classe **UserMode** si vous êtes uniquement intéressé par les **autorisation déléguées** et la classe **AppMode** si vous êtes seulement intéressé par les **autorisations d’application**.

Pour plus d’informations sur ces types d'autorisation, reportez-vous aux [Autorisations déléguées, autorisations de l’application et aux autorisations effectives](https://docs.microsoft.com/en-us/graph/permissions-reference#delegated-permissions-application-permissions-and-effective-permissions). Consultez également [Obtenir un accès sans utilisateur](https://docs.microsoft.com/en-us/graph/auth-v2-service) pour plus d'informations spécifiques sur les **autorisations d'application**.

## Conditions préalables

Cet exemple nécessite les éléments suivants :

* [Visual Studio](https://www.visualstudio.com/en-us/downloads)

* Un [compte professionnel Office 365](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account). Un compte d’administrateur Office 365 est obligatoire pour exécuter des opérations de niveau administrateur et pour accorder des autorisations d'applications. Vous pouvez souscrire un [ abonnement pour développeur Office 365](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account) incluant les ressources dont vous avez besoin pour commencer à créer des applications.

## Inscription de l’application

Cet exemple comprend des exemples qui utilisent les autorisations déléguées et les autorisations d’application, vous devez donc inscrire l’application séparément pour chaque scénario.

<a name="Register-the-delegated-permissions-application"></a>
### Inscription des **autorisations déléguées** de l'application

1. Accédez au [Centre d’administration Azure Active Directory](https://aad.portal.azure.com). Connectez-vous à l’aide d’un **compte personnel** (alias : compte Microsoft) ou d’un **compte professionnel ou scolaire**.

2. Sélectionnez **Azure Active Directory** dans le volet de navigation gauche, puis sélectionnez **Inscriptions d’applications (préversion)** sous **Gérer**.

3. Sélectionnez **Nouvelle inscription**. Sur la page **Inscrire une application**, définissez les valeurs comme suit.

    * Définissez le **Nom** pour l'`Exemple des extraits de console (autorisations déléguées)`.
    * Définissez les **Types de comptes pris en charge** sur les **Comptes figurant dans un annuaire organisationnel et comptes Microsoft personnels**.
    * Laissez **Redirect URI** vide.
	** Choisissez **Inscrire**.

4. Sur la page **Exemple d'extraits de console (autorisations déléguées)**, copiez les valeurs de l' **ID d’application (client)** et de l'**ID Directory (locataire)**. Enregistrez ces deux valeurs, car vous en aurez besoin plus tard.

5. Sélectionnez le lien **Ajouter un URI de redirection**. Dans la page **URI de redirection** Recherchez la section des **URI de redirection suggérés pour les clients publics (mobile, bureau)**. Sélectionnez l’URI commençant par `msal` **et** l’URI **urn:ietf:wg:oauth:2.0:oob**.

6. Ouvrez la solution d'exemple dans Visual Studio, puis ouvrez le fichier **Constants.cs**. Remplacez la chaîne **Client **par la valeur d'**ID de Directory (locataire)** que vous avez précédemment copiée. Remplacez la chaîne **ClientIdForUserAuthn** par la valeur de l'**ID de l’application (client)**.

<a name="Register-the-application-permissions-application"></a>
### Enregistrer les **Autorisations d'application** de l'application

1. Accédez au [Centre d’administration Azure Active Directory](https://aad.portal.azure.com). Connectez-vous en utilisant un **Compte professionnel ou scolaire**.

2. Sélectionnez **Azure Active Directory** dans le volet de navigation gauche, puis sélectionnez **Inscriptions d’applications (préversion)** sous **Gérer**.

3. Sélectionnez **Nouvelle inscription**. Sur la page **Inscrire une application**, définissez les valeurs comme suit.

    * Définissez le **Nom** pour l'`Exemple des extraits de console (autorisations d'application)`.
    * Définissez les **Types de comptes pris en charge** sur **Comptes figurant dans un annuaire organisationnel**.
    * Laissez **Redirect URI** vide.
    * Choisissez **Inscrire**.

4. Sur la page **Exemple d'extraits de console (autorisations d'applications)**, copiez et enregistrez les valeurs de l'**ID d’application (client)** et de l'**ID Directory (locataire)**. Vous en aurez besoin lors de l'étape 7.

5. Sélectionnez **Certificats & secrets** sous **Gérer**. Sélectionnez le bouton **Nouvelle clé secrète client**. Entrez une valeur dans la **Description**, sélectionnez une option pour **Expire le**, puis choisissez **Ajouter**.

6. Copiez la valeur de la clé secrète client avant de quitter cette page. Vous en aurez besoin à l’étape suivante.

7. Ouvrez la solution d'exemple dans Visual Studio, puis ouvrez le fichier **Constants.cs**. Remplacez la chaîne **Client **par la valeur d'**ID de Directory (locataire)** que vous avez précédemment copiée. De la même façon, remplacez la chaîne de **ClientIdForAppAuthn** par la valeur de l'**ID de l’application (client)** et remplacez la chaîne **ClientSecret** par la valeur de la clé secrète cliente.

8. Retournez au Portail de gestion Azure Active Directory. Sélectionnez **Autorisations API**, puis sélectionnez **Ajouter une autorisation**. Dans le volet qui apparaît, sélectionnez **Microsoft Graph**, puis choisissez **Autorisations d'application**. 

9. Utilisez la zone de recherche **Sélectionnez les autorisations** pour trouver les autorisations qui suivent : Directory.Read.All, Group.ReadWrite.All, Mail.Read, Mail.ReadWrite, and User.Read.All. Sélectionnez la case à cocher pour chacune des autorisations lorsqu'elle apparaît (veuillez noter que les autorisations ne restent pas visibles dans la liste lorsque vous les sélectionnez). Sélectionner le bouton **Ajouter des autorisations** en bas de la boîte de dialogue.

10. Sélectionnez le bouton **Accorder l’autorisation administrateur pour \[nom du client]**. Sélectionnez **Oui** pour confirmer ce qui s’affiche.

## Créer et exécuter l’exemple

1. Ouvrez l’exemple de solution dans Visual Studio.
2. Appuyez sur F5 pour créer et exécuter l’exemple. Cela entraîne la restauration des dépendances du package NuGet et l’ouverture de l’application de la console.
3. Sélectionnez le mode Utilisateur pour exécuter l’application avec les **autorisations déléguées** uniquement. Sélectionnez le mode Application pour exécuter l’application avec les **autorisations d'application** uniquement. Sélectionnez les deux types d'autorisations pour les exécuter.
4. Lorsque vous exécutez le mode Utilisateur, vous êtes invité à vous connecter à l’aide d’un compte de votre client Office 365 et à accepter les autorisations exigées par l’application. Si vous voulez exécuter des opérations liées à des groupes dans la classe **UserMode**, vous devez supprimer les marques de commentaire de la méthode **GetDetailsForGroups** dans le fichier UserMode.cs et l'étendue **Group.Read.All** dans le fichier AuthenticationHelper.cs. Une fois que ces modifications sont effectuées, seul un administrateur pourra se connecter et donner son accord. Dans le cas contraire, vous pouvez vous connecter et donner votre consentement avec un utilisateur non-administrateur.
5. Lorsque vous exécutez le mode Application, celle-ci commence à effectuer un certain nombre de tâches courantes relatives à des groupes que seul un administrateur peut effectuer. Puisque vous avez déjà autorisé l’application à effectuer ces opérations, vous n’êtes pas invité à vous connecter et à donner votre accord.

## Questions et commentaires

Nous serions ravis de connaître votre opinion sur l'application de console API Microsoft Graph. Vous pouvez nous faire part de vos questions et suggestions dans la rubrique [Problèmes](https://github.com/microsoftgraph/console-csharp-snippets-sample/issues) de ce référentiel.

Les questions générales sur le développement de Microsoft Graph doivent être publiées sur [Stack Overflow](https://stackoverflow.com/questions/tagged/microsoftgraph). Veillez à poser vos questions ou à rédiger vos commentaires en utilisant la balise \[microsoftgraph].

## Contribution

Si vous souhaitez contribuer à cet exemple, voir [CONTRIBUTING.MD](/CONTRIBUTING.md).

Ce projet a adopté le [code de conduite Open Source de Microsoft](https://opensource.microsoft.com/codeofconduct/). Pour en savoir plus, reportez-vous à la [FAQ relative au code de conduite](https://opensource.microsoft.com/codeofconduct/faq/) ou contactez [opencode@microsoft.com](mailto:opencode@microsoft.com) pour toute question ou tout commentaire.
  
## Ressources supplémentaires

* [Obtenir l’accès sans utilisateur](https://docs.microsoft.com/en-us/graph/auth-v2-service)
* [Autorisations déléguées, autorisations de l’application et autorisations effectives](https://docs.microsoft.com/en-us/graph/permissions-reference#delegated-permissions-application-permissions-and-effective-permissions)
* [Microsoft Graph](https://developer.microsoft.com/en-us/graph)

## Copyright

Copyright (c) 2017 Microsoft. Tous droits réservés.
