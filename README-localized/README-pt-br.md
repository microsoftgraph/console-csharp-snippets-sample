---
page_type: sample
products:
- ms-graph
languages:
- csharp
description: "Esse aplicativo de console do Windows mostra como executar várias operações usando a biblioteca de cliente do Microsoft Graph com ambos os tipos de permissões, tanto delegada quanto de aplicativo. Esta amostra usa a Biblioteca de Autenticação da Microsoft (MSAL) para autenticação do ponto de extremidade do Azure AD v2.0."
extensions:
  contentType: samples
  technologies:
  - Microsoft Graph 
  - Microsoft identity platform
  services:
  - Microsoft identity platform
  createdDate: 9/8/2017 1:33:44 PM
---
# Aplicativo trechos de console C# do Microsoft Graph

## Sumário

* [Introdução](#introduction)
* [Pré-requisitos](#prerequisites)
* [Registrar o **aplicativo** permissões delegadas](#Register-the-delegated-permissions-application )
* [Registrar o **aplicativo** permissões de aplicativo](#Register-the-application-permissions-application )
* [Criar e executar o exemplo](#build-and-run-the-sample)
* [Perguntas e comentários](#questions-and-comments)
* [Colaboração](#contributing)
* [Recursos adicionais](#additional-resources)

## Introdução

Este exemplo de projeto fornece um repositório de trechos de código que usa o Microsoft Graph para realizar tarefas comuns, como envio de emails, gerenciamento de grupos e outras atividades diretamente de um aplicativo de console do Windows. O exemplo usa o [SDK de cliente do Microsoft Graph .NET](https://github.com/microsoftgraph/msgraph-sdk-dotnet) para trabalhar com dados retornados pelo Microsoft Graph.

O exemplo usa a Biblioteca de Autenticação da Microsoft (MSAL) para autenticação. O exemplo demonstra tanto as permissões delegadas quanto as do aplicativo.

As **Permissões delegadas**são usadas pelos aplicativos que têm um usuário conectado presente. Para esses aplicativos, o usuário ou um administrador concorda com as permissões que o aplicativo solicita e o aplicativo tem permissão delegada para agir como se fosse o usuário conectado fazendo chamadas para o Microsoft Graph. Algumas permissões delegadas podem ser autorizadas por usuários não administrativos, mas algumas permissões com privilégios mais altos exigem o consentimento do administrador. Esse aplicativo contém algumas operações relacionadas a grupos que exigem consentimento administrativo e as permissões associadas necessárias para executá-las, são comentadas por padrão.

As**permissões de aplicativo** são usadas por aplicativos que funcionam sem a presença de um usuário conectado. Você pode usar esse tipo de permissão para aplicativos executados como serviços de tela de fundo ou daemons, e que portanto, nem têm nem exigem consentimento do usuário. As permissões de aplicativo só podem ser concedidas por um administrador de locatários. É importante que você entenda que estará dando a esse aplicativo de exemplo um grande poder ao fornecer a ele consentimento de administrador de ti. Por exemplo, se você executar esse exemplo na **AppMode** no seu locatário, criará um grupo, adicionará e removerá os membros do grupo e, em seguida, excluirá o grupo.

Caso pretenda usar os dois tipos de permissões, você precisará criar e configurar dois aplicativos na [centro de administração do Azure Active Directory](https://aad.portal.azure.com), um para **permissões delegadas** e outro para **permissões de aplicativo**. O aplicativo de amostra é estruturado para que você possa configurar somente um aplicativo caso estiver interessado em apenas um tipo de permissão. Use a classe **UserMode** se você estiver interessado apenas em **permissões delegadas** e a classe **AppMode** se você estiver interessado apenas em **permissões de aplicativo**.

Para obter mais informações sobre esses tipos de permissões, confira [Permissões delegadas, Permissões de aplicativo e permissões efetivas](https://docs.microsoft.com/en-us/graph/permissions-reference#delegated-permissions-application-permissions-and-effective-permissions). Confira também [Obter acesso sem um usuário](https://docs.microsoft.com/en-us/graph/auth-v2-service) para obter mais informações especificamente sobre **permissões de aplicativo**.

## Pré-requisitos

Este exemplo requer o seguinte:

* [Visual Studio](https://www.visualstudio.com/en-us/downloads)

* Uma [conta do Office 365 for business](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account). Uma conta de administrador do Office 365 é necessária para executar operações de administrador e para dar o consentimento para as permissões de aplicativo. Você pode se inscrever para uma [assinatura de Desenvolvedor do Office 365](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account)que inclui os recursos necessários para começar a criar aplicativos.

## Registro de aplicativo

Este exemplo contém exemplos que usam tanto as permissões delegadas quanto as permissões do aplicativo, dessa forma você precisará registrar o aplicativo separadamente para cada cenário.

<a name="Register-the-delegated-permissions-application"></a>
### Registre o aplicativo **permissões delegadas**

1. Acesse o [ centro de administração do Azure Active Directory](https://aad.portal.azure.com). Faça logon usando uma **conta pessoal** (também conhecida como: Conta Microsoft) **Conta Corporativa ou de Estudante**.

2. Selecione **Azure Active Directory** na navegação à esquerda e, em seguida, selecione **Registros de aplicativo (Visualizar)** em **Gerenciar**.

3. Selecione **Novo registro**. Na página **Registrar um aplicativo**, defina os valores da forma descrita abaixo.

    * Defina o **Nome** como `Exemplo de Trechos de Console (Permissões delegadas)`.
    * Defina **Tipos de contas com suporte** para **Contas em qualquer diretório organizacional e contas pessoais da Microsoft**.
    * Deixe o **URI de Redirecionamento** em branco.
	** Escolha **Registrar**.

4. Na página **Exemplo de Trechos de Console (Permissões delegadas)**, copie os valores do **ID do aplicativo (cliente)** e do **ID do Diretório (locatário)**. Salve esses dois valores, porque você vai precisar deles mais tarde.

5. Selecione **adicionar uma URI de redirecionamento**. Na página **Redirecionamento de URIs**, localize a seção**URIs de redirecionamento sugeridas para clientes públicos (móvel, área de trabalho)**. Selecione a URI que começa com `msal` **e**a URI **urn:ietf:wg:oauth:2.0:oob**.

6. Abra a solução de exemplo no Visual Studio e, em seguida, abra o arquivo **Constants.cs**. Altere a cadeia de caracteres do**Locatário** para o valor do **ID de Directory (locatário)** que você copiou anteriormente. Altere a cadeia de caracteres de **ClientIdForUserAuthn**para o valor da ** ID do aplicativo (cliente)**.

<a name="Register-the-application-permissions-application"></a>
### Registre o aplicativo **permissões de aplicativo**

1. Acesse o [ centro de administração do Azure Active Directory](https://aad.portal.azure.com). Faça logon usando uma **Conta Corporativa ou de Estudante**.

2. Selecione **Azure Active Directory** na navegação à esquerda e, em seguida, selecione **Registros de aplicativo (Visualizar)** em **Gerenciar**.

3. Selecione **Novo registro**. Na página **Registrar um aplicativo**, defina os valores da forma descrita abaixo.

    * Defina o **Nome** como `Exemplo de Trechos de Console (Permissões de aplicativo)`.
    * Defina os **Tipos de conta com suporte** para **Contas em qualquer diretório organizacional**.
    * Deixe o **URI de Redirecionamento** em branco.
    * Escolha **Registrar**.

4. Na página **Exemplo de Trechos de Console (Permissões de aplicativo)**, copie e salve os valores do **ID do aplicativo (cliente)** e do **ID do Diretório (locatário)**. Você precisará desses valores mais tarde na etapa 7.

5. Selecione **Certificados e segredos** em **Gerenciar**. Selecione o botão **Novo segredo do cliente**. Insira um valor em **Descrição**, selecione qualquer uma das opções para **Expira** e escolha **Adicionar**.

6. Copie o valor do segredo do cliente antes de sair desta página. Você precisará dele na próxima etapa.

7. Abra a solução de exemplo no Visual Studio e, em seguida, abra o arquivo **Constants.cs**. Altere a cadeia de caracteres do**Locatário** para o valor do **ID de Directory (locatário)** que você copiou anteriormente. Da mesma forma, altere a cadeia de caracteres **ClientIdForAppAuthn** para o valor da **ID do aplicativo (cliente)** e altere a cadeia de caracteres **ClientSecret** para o valor de segredo do cliente.

8. Retorne para o centro de gerenciamento do Azure Active Directory.  Selecione **permissões para API** e, em seguida, selecione **Adicionar uma permissão**. No painel que se abre, escolha **Microsoft Graph**e, em seguida, escolha **Permissões de aplicativo**. 

9. Use a caixa de pesquisa **Selecionar permissões** para pesquisar as seguintes permissões: Directory.Read.All, Group.ReadWrite.All, Mail.Read, Mail.ReadWrite e User.Read.All. Marque a caixa de seleção para cada permissão á medida que elas forem aparecendo (observe que as permissões vão desaparecendo da lista ao se selecionar cada uma delas). Selecione o botão **Adicionar permissões** na parte inferior do painel.

10. Escolha o botão **Conceder consentimento de administrador para \[nome do locatário]**. Marque **Sim** para a confirmação exibida.

## Criar e executar o exemplo

1. Abra a solução de exemplo no Visual Studio.
2. Pressione F5 para criar e executar o exemplo. Isso restaurará as dependências do pacote NuGet e abrirá o aplicativo de console.
3. Selecione o Modo de usuário para executar o aplicativo somente com **permissões delegadas**. Selecione o Modo de aplicativo para executar o aplicativo somente com **permissões de aplicativo**. Selecione ambos os modos para executar usando os dois tipos de permissões.
4. Ao executar o Modo de usuário, você será solicitado a entrar com uma conta do seu locatário do Office 365 e concordar com as permissões que o aplicativo solicitar. Se desejar executar as operações relacionadas a grupos da classe **UserMode**, você precisará remover o comentário do método **GetDetailsForGroups** no arquivo UserMode.cs e o escopo **Group.Read.All**no arquivo AuthenticationHelper.cs. Após fazer essas alterações, somente um administrador poderá entrar e dar consentimentos. Caso contrário, você pode entrar e concordar com um usuário não administrador.
5. Ao executar o Modo de aplicativo, o aplicativo começará a executar várias tarefas comuns relacionadas a grupos que somente um administrador pode fazer. Como você já autorizou o aplicativo a fazer essas operações, não será pedido para entrar e consentir.

## Perguntas e comentários

Gostaríamos muito de saber sua opinião sobre ao Aplicativo de Console da API do Microsoft Graph. Você pode enviar perguntas e sugestões na seção [Problemas](https://github.com/microsoftgraph/console-csharp-snippets-sample/issues) deste repositório.

As perguntas sobre o desenvolvimento do Microsoft Graph em geral devem ser postadas no [Stack Overflow](https://stackoverflow.com/questions/tagged/microsoftgraph). Não deixe de marcar as perguntas ou comentários com a tag \[microsoftgraph].

## Colaboração

Se quiser contribuir para esse exemplo, confira [CONTRIBUTING.MD](/CONTRIBUTING.md).

Este projeto adotou o [Código de Conduta de Código Aberto da Microsoft](https://opensource.microsoft.com/codeofconduct/).  Para saber mais, confira as [Perguntas frequentes sobre o Código de Conduta](https://opensource.microsoft.com/codeofconduct/faq/) ou entre em contato pelo [opencode@microsoft.com](mailto:opencode@microsoft.com) se tiver outras dúvidas ou comentários.
  
## Recursos adicionais

* [Obter acesso sem um usuário](https://docs.microsoft.com/en-us/graph/auth-v2-service)
* [Permissões delegadas, Permissões de aplicativo e permissões efetivas](https://docs.microsoft.com/en-us/graph/permissions-reference#delegated-permissions-application-permissions-and-effective-permissions)
* [Microsoft Graph](https://developer.microsoft.com/en-us/graph)

## Direitos autorais

Copyright (c) 2017 Microsoft. Todos os direitos reservados.
