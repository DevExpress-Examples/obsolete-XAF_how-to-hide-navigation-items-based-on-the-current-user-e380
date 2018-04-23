Imports Microsoft.VisualBasic
Imports System
Imports System.Configuration
Imports System.Windows.Forms

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Win
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports HideNavigationItemsExample.Module.Security
Imports System.Collections.Generic

Namespace HideNavigationItemsExample.Win
	Friend NotInheritable Class Program
		''' <summary>
		''' The main entry point for the application.
		''' </summary>
		Private Sub New()
		End Sub
		<STAThread> _
		Shared Sub Main()
#If EASYTEST Then
			DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register()
#End If
			Application.EnableVisualStyles()
			Application.SetCompatibleTextRenderingDefault(False)
			EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached
			Dim winApplication As New HideNavigationItemsExampleWindowsFormsApplication()
			' Refer to the http://documentation.devexpress.com/#Xaf/CustomDocument2680 help article for more details on how to provide a custom splash form.
			'winApplication.SplashScreen = new DevExpress.ExpressApp.Win.Utils.DXSplashScreen("YourSplashImage.png");
			If ConfigurationManager.ConnectionStrings("ConnectionString") IsNot Nothing Then
				winApplication.ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
			End If
#If EASYTEST Then
			If ConfigurationManager.ConnectionStrings("EasyTestConnectionString") IsNot Nothing Then
				winApplication.ConnectionString = ConfigurationManager.ConnectionStrings("EasyTestConnectionString").ConnectionString
			End If
#End If
			Try
				AddHandler (CType(winApplication.Security, SecurityStrategy)).CustomizeRequestProcessors, Function(sender, e) AnonymousMethod1(sender, e)
				winApplication.Setup()
				winApplication.Start()
			Catch e As Exception
				winApplication.HandleException(e)
			End Try
		End Sub
		
		Private Shared Function AnonymousMethod1(ByVal sender As Object, ByVal e As CustomizeRequestProcessorsEventArgs) As Boolean
				Dim result As New List(Of IOperationPermission)()
				Dim security As SecurityStrategyComplex = TryCast(sender, SecurityStrategyComplex)
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
				e.Processors.Add(GetType(NavigationItemPermissionRequest), New NavigationItemPermissionRequestProcessor(permissionDictionary))
			Return True
		End Function
	End Class
End Namespace
