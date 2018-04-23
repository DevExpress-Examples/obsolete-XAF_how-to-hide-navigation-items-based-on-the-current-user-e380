Imports Microsoft.VisualBasic
Imports System
Imports System.Configuration
Imports System.Web.Configuration
Imports System.Web

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Web
Imports DevExpress.Web
Imports HideNavigationItemsExample.Module.Security
Imports System.Collections.Generic

Namespace HideNavigationItemsExample.Web
	Public Class [Global]
		Inherits System.Web.HttpApplication
		Public Sub New()
			InitializeComponent()
		End Sub
		Protected Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
			AddHandler ASPxWebControl.CallbackError, AddressOf Application_Error
#If EASYTEST Then
			DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = True
#End If
		End Sub
		Protected Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
			WebApplication.SetInstance(Session, New HideNavigationItemsExampleAspNetApplication())
			If ConfigurationManager.ConnectionStrings("ConnectionString") IsNot Nothing Then
				WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
			End If
#If EASYTEST Then
			If ConfigurationManager.ConnectionStrings("EasyTestConnectionString") IsNot Nothing Then
				WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings("EasyTestConnectionString").ConnectionString
			End If
#End If
			AddHandler (CType(WebApplication.Instance.Security, SecurityStrategy)).CustomizeRequestProcessors, Function(sender2, e2) AnonymousMethod1(sender2, e2)
			WebApplication.Instance.Setup()
			WebApplication.Instance.Start()
		End Sub
		
		Private Function AnonymousMethod1(ByVal sender2 As Object, ByVal e2 As CustomizeRequestProcessorsEventArgs) As Boolean
					 Dim result As New List(Of IOperationPermission)()
					 Dim security As SecurityStrategyComplex = TryCast(sender2, SecurityStrategyComplex)
					 If security IsNot Nothing Then
						 Dim user As ISecurityUserWithRoles = TryCast(security.User, ISecurityUserWithRoles)
						 If user IsNot Nothing Then
							 For Each role As ISecurityRole In user.Roles
								 If TypeOf role Is CustomSecurityRole Then
									 result.AddRange((CType(role, CustomSecurityRole)).GetPermissions())
								 End If
							 Next role
						 End If
					 End If
					 Dim permissionDictionary As IPermissionDictionary = New PermissionDictionary(CType(result, IEnumerable(Of IOperationPermission)))
					 e2.Processors.Add(GetType(NavigationItemPermissionRequest), New NavigationItemPermissionRequestProcessor(permissionDictionary))
			Return True
		End Function
		Protected Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
			Dim filePath As String = HttpContext.Current.Request.PhysicalPath
			If (Not String.IsNullOrEmpty(filePath)) AndAlso (filePath.IndexOf("Images") >= 0) AndAlso (Not System.IO.File.Exists(filePath)) Then
				HttpContext.Current.Response.End()
			End If
		End Sub
		Protected Sub Application_EndRequest(ByVal sender As Object, ByVal e As EventArgs)
		End Sub
		Protected Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
		End Sub
		Protected Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
			ErrorHandling.Instance.ProcessApplicationError()
		End Sub
		Protected Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
			WebApplication.LogOff(Session)
			WebApplication.DisposeInstance(Session)
		End Sub
		Protected Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
		End Sub
		#Region "Web Form Designer generated code"
		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
		End Sub
		#End Region
	End Class
End Namespace
