---
page_type: sample
products:
- ms-graph
languages:
- csharp
description: "Esta aplicación de consola de Windows muestra cómo realizar diversas tareas con permisos delegados y de aplicación en la Biblioteca cliente de Microsoft Graph. Este ejemplo utiliza la biblioteca de autenticación de Microsoft (MSAL) para la autenticación en el extremo de Azure AD v2.0."
extensions:
  contentType: samples
  technologies:
  - Microsoft Graph 
  - Microsoft identity platform
  services:
  - Microsoft identity platform
  createdDate: 9/8/2017 1:33:44 PM
---
# Aplicación de fragmentos de consola para Microsoft Graph y C#

## Tabla de contenido

* [Introducción](#introduction)
* [Requisitos previos](#prerequisites)
* [Registrar los **permisos delegados de** la aplicación](#Register-the-delegated-permissions-application )
* [Registrar los **permisos de aplicación** de la aplicación](#Register-the-application-permissions-application )
* [Compilar y ejecutar el ejemplo](#build-and-run-the-sample)
* [Preguntas y comentarios](#questions-and-comments)
* [Colaboradores](#contributing)
* [Recursos adicionales](#additional-resources)

## Introducción

Esta aplicación de ejemplo proporciona un repositorio de fragmentos de código que usa Microsoft Graph para realizar tareas comunes, como enviar correos electrónicos, administrar grupos y otras actividades desde una aplicación de consola de Windows. Usa el [SDK del cliente de Microsoft Graph .NET](https://github.com/microsoftgraph/msgraph-sdk-dotnet) para trabajar con los datos devueltos por Microsoft Graph.

El ejemplo usa la biblioteca de autenticación de Microsoft (MSAL) para la autenticación. El ejemplo muestra tanto permisos delegados como de aplicación.

Los **permisos delegados** se usan en las aplicaciones donde un usuario debe haber iniciado sesión. En estas aplicaciones, el usuario o un administrador dan su consentimiento a los permisos que solicita la aplicación, y se delega permiso a la aplicación para que actúe como usuario que ha iniciado sesión cuando se hacen llamadas a Microsoft Graph. En ocasiones, los usuarios no administrativos pueden recibir permisos delegados, pero existen permisos con privilegios mayores que requieren el consentimiento del administrador. Esta aplicación contiene algunas operaciones relacionadas con los grupos que requieren consentimiento administrativo. Los permisos asociados necesarios aparecen comentados de forma predeterminada.

**Los permisos de la aplicación** se usan en aplicaciones que se ejecutan sin la presencia de un usuario que haya iniciado sesión. Puede usar este tipo de permisos para aplicaciones que se ejecutan como servicios en segundo plano o daemons y que, por lo tanto, no tendrán ni requerirán consentimiento por parte de los usuarios. Los permisos de aplicación solo pueden ser aceptados por un administrador de espacio empresarial. Tenga en cuenta que este ejemplo tiene mucho poder, al obtener consentimiento del administrador de TI. Por ejemplo, si ejecuta este ejemplo en **AppMode** en su espacio empresarial, creará un grupo, agregará y quitará a miembros del grupo y, a continuación, lo eliminará.

Si desea usar ambos tipos de permisos, tendrá que crear y configurar dos aplicaciones en el [Centro de administración de Azure Active Directory](https://aad.portal.azure.com), una para **permisos delegados** y otra para **permisos de aplicación**. El ejemplo está estructurado para que solo pueda configurar una aplicación si está interesado en un solo tipo de permiso. Use la clase **UserMode** si solo le interesan **los permisos delegados** y la clase **AppMode** si le interesan **los permisos de aplicación**.

Para obtener más información sobre estos tipos de permisos, vea [Permisos delegados, permisos de la aplicación y permisos efectivos](https://docs.microsoft.com/en-us/graph/permissions-reference#delegated-permissions-application-permissions-and-effective-permissions). Para obtener información más específica sobre los **permisos de aplicación**, vea [Obtener acceso sin un usuario](https://docs.microsoft.com/en-us/graph/auth-v2-service).

## Requisitos previos

Este ejemplo necesita lo siguiente:

* [Visual Studio](https://www.visualstudio.com/en-us/downloads)

* Una cuenta de [Office 365 para Empresas.](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account) Es necesaria una cuenta de administrador de Office 365 para ejecutar operaciones de nivel de administrador y autorizar los permisos de aplicación. Puede realizar [una suscripción a Office 365 Developer](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account) que incluye los recursos que necesita para comenzar a crear aplicaciones.

## Registro de la aplicación

En este ejemplo se usan permisos delegados y permisos de la aplicación, por lo que deberá registrar la aplicación por separado para cada escenario.

<a name="Register-the-delegated-permissions-application"></a>
### Registrar los **permisos delegados** de la aplicación

1. Vaya al [Centro de administración de Azure Active Directory](https://aad.portal.azure.com). Inicie sesión con una **cuenta personal** (por ejemplo: una cuenta de Microsoft) o una **cuenta profesional o educativa**.

2. Seleccione **Azure Active Directory** en el panel de navegación izquierdo y, después, seleccione **Registros de aplicaciones (versión preliminar)** en **Administrar**.

3. Haga clic en **Nuevo registro**. En la página **Registrar una aplicación**, establezca los valores siguientes.

    * Establezca **Nombre** como `Ejemplo de fragmentos de la consola (permisos delegados)`.
    * Establezca **Tipos de cuenta admitidos** en **Cuentas en cualquier directorio de organización y cuentas personales de Microsoft**.
    * Deje vacía la opción de **URI de redirección**.
	**Seleccione **Registrar**.

4. En la página **Ejemplo de fragmentos de consola (permisos delegados)**, copie los valores de **Id. de aplicación (cliente)** e **Id. de directorio (espacio empresarial)**. Guarde estos dos valores, ya que los necesitará más adelante.

5. Haga clic en el vínculo **Agregar un URI de redirección**. En la página **URI de redirección**, localice la sección **URI de redirección sugeridos para clientes públicos (para dispositivos móviles o de escritorio)**. Seleccione el URI que comience por `msal` **y** el URI **urn:ietf:wg:oauth:2.0:oob**.

6. Abra la solución de ejemplo en Visual Studio y, a continuación, abra el archivo **Constants.cs**. Cambie la cadena de **Espacio empresarial** por el valor de la ID de directorio (espacio empresarial) que copió anteriormente. Cambie la cadena de ClientIdForUserAuthn por el **valor de la ID. de aplicación de (cliente) que copió antes**.

<a name="Register-the-application-permissions-application"></a>
### Registrar los **permisos de aplicación** de la aplicación

1. Vaya al [Centro de administración de Azure Active Directory](https://aad.portal.azure.com). Inicie sesión con **una cuenta profesional o educativa**.

2. Seleccione **Azure Active Directory** en el panel de navegación izquierdo y, después, seleccione **Registros de aplicaciones (versión preliminar)** en **Administrar**.

3. Haga clic en **Nuevo registro**. En la página **Registrar una aplicación**, establezca los valores siguientes.

    * Establezca **Nombre** como `Ejemplo de fragmentos de la consola (permisos de aplicación)`.
    * Establezca **los tipos de cuenta compatibles** en las**cuentas en cualquier directorio de la organización**.
    * Deje vacía la opción de **URI de redirección**.
    * Elija **Registrar**.

4. En la página **Ejemplo de fragmentos de consola (permisos de aplicación)**, copie y guarde los valores de **Id. de aplicación (cliente)** e **Id. de directorio (espacio empresarial)**. Los necesitará más adelante en el paso 7.

5. Seleccione **Certificados y secretos** en **Administrar**. Seleccione el botón **Nuevo secreto de cliente**. Escriba un valor en **Descripción**, seleccione una opción para **Expira** y luego seleccione **Agregar**.

6. Copie el valor del secreto de cliente antes de salir de la página. Lo necesitará en el siguiente paso.

7. Abra la solución de ejemplo en Visual Studio y, a continuación, abra el archivo **Constants.cs**. Cambie la cadena de **Espacio empresarial** por el valor de la ID de directorio (espacio empresarial) que copió anteriormente. De forma similar, cambie la cadena de ClientIdForAppAuthn por el **valor identificador de la aplicación (cliente)** y cambie la cadena ClientSecret por el valor de secreto de cliente.

8. Volver al Centro de administración de Azure Active Directory Seleccione **Permisos de API** y después, seleccione **Agregar un permiso**. En el panel que se abre, elija **Microsoft Graph** y luego elija **Permisos de aplicación**. 

9. Use el cuadro de búsqueda **Seleccionar permisos** para buscar los siguientes permisos: Directory.Read.All, Group.ReadWrite.All, Mail.Read, Mail.ReadWrite y User.Read.All. Seleccione la casilla de verificación para cada permiso a medida que aparece (tenga en cuenta que los permisos no permanecerán visibles en la lista al seleccionar cada uno). Seleccione el botón **Agregar permisos** de la parte inferior del cuadro de diálogo.

10. Elija el botón **Conceder permisos de administrador para \[nombre del espacio empresarial]**. Seleccione **Sí** para la confirmación que aparece.

## Compilar y ejecutar el ejemplo

1. Abra la solución del ejemplo en Visual Studio.
2. Pulse F5 para compilar y ejecutar el ejemplo. Esto restaurará las dependencias del paquete de NuGet y abrirá la aplicación de la consola.
3. Seleccione el modo de usuario para ejecutar la aplicación con solo **permisos delegados**. Seleccione el modo de aplicación para ejecutar la aplicación con solo **permisos de aplicación**. Seleccione ambos para ejecutar con ambos tipos de permisos.
4. Cuando ejecute el modo de usuario, se le pedirá que inicie sesión con una cuenta en su espacio empresarial de Office 365 y que autorice los permisos que la aplicación solicita. Si desea ejecutar las operaciones relacionadas con los grupos en la clase **UserMode**, tendrá que hacer que el código de GetDetailsForGroups en el archivo UserMode.cs y el ámbito Group.Read.All en el archivo AuthenticationHelper.cs no aparezcan como comentarios. Cuando haga estos cambios, solo un administrador podrá iniciar sesión y autorizar. De otro modo, tendrá que iniciar sesión y conceder permisos a un usuario que no sea administrador.
5. Cuando ejecute el modo de aplicación, la aplicación empezará a llevar a cabo una serie de tareas comunes relacionadas con los grupos que solo puede ejecutar un administrador. Como ya autorizó a la aplicación a que realice estas operaciones, no se le pedirá que inicie sesión y autorice.

## Preguntas y comentarios

Nos encantaría recibir sus comentarios acerca de la aplicación de consola para la API de Microsoft Graph. Puede enviar sus preguntas y sugerencias en la sección de [Problemas](https://github.com/microsoftgraph/console-csharp-snippets-sample/issues) de este repositorio.

Las preguntas sobre el desarrollo de Microsoft Graph en general deben enviarse a [Stack Overflow](https://stackoverflow.com/questions/tagged/microsoftgraph). Asegúrese de que sus preguntas o comentarios estén etiquetados con \[microsoftgraph].

## Colaboradores

Si quiere hacer su aportación a este ejemplo, vea [CONTRIBUTING.MD](/CONTRIBUTING.md).

Este proyecto ha adoptado el [Código de conducta de código abierto de Microsoft](https://opensource.microsoft.com/codeofconduct/). Para obtener más información, vea [Preguntas frecuentes sobre el código de conducta](https://opensource.microsoft.com/codeofconduct/faq/) o póngase en contacto con [opencode@microsoft.com](mailto:opencode@microsoft.com) si tiene otras preguntas o comentarios.
  
## Recursos adicionales

* [Obtener acceso sin un usuario](https://docs.microsoft.com/en-us/graph/auth-v2-service)
* [Permisos delegados, permisos de la aplicación y permisos efectivos](https://docs.microsoft.com/en-us/graph/permissions-reference#delegated-permissions-application-permissions-and-effective-permissions)
* [Microsoft Graph](https://developer.microsoft.com/en-us/graph)

## Derechos de autor

Copyright (c) 2017 Microsoft. Todos los derechos reservados.
