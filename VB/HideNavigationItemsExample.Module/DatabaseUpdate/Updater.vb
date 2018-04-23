Imports System
Imports System.Linq
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.ExpressApp.Security
Imports HideNavigationItemsExample.Module.Security
Imports DevExpress.ExpressApp.Security.Strategy
Imports DevExpress.Persistent.BaseImpl.PermissionPolicy

Namespace HideNavigationItemsExample.Module.DatabaseUpdate
	Public Class Updater
		Inherits ModuleUpdater

		Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
			MyBase.New(objectSpace, currentDBVersion)
		End Sub
		Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
			MyBase.UpdateDatabaseAfterUpdateSchema()
			Dim userRole As CustomSecurityRole = CreateUserRole()
			Dim administratorRole As CustomSecurityRole = CreateAdministratorRole()
			Dim admin As PermissionPolicyUser = ObjectSpace.FindObject(Of PermissionPolicyUser)(New BinaryOperator("UserName", "Admin"))
			If admin Is Nothing Then
				admin = ObjectSpace.CreateObject(Of PermissionPolicyUser)()
				admin.UserName = "Admin"
				admin.IsActive = True
				admin.SetPassword("")
				admin.Roles.Add(administratorRole)
				admin.Save()
			End If
			Dim user As PermissionPolicyUser = ObjectSpace.FindObject(Of PermissionPolicyUser)(New BinaryOperator("UserName", "User"))
			If user Is Nothing Then
				user = ObjectSpace.CreateObject(Of PermissionPolicyUser)()
				user.UserName = "User"
				user.IsActive = True
				user.SetPassword("")
				user.Roles.Add(userRole)
				user.Save()
			End If
		End Sub
		Private Function CreateAdministratorRole() As CustomSecurityRole
			Dim administratorRole As CustomSecurityRole = ObjectSpace.FindObject(Of CustomSecurityRole)(New BinaryOperator("Name", SecurityStrategyComplex.AdministratorRoleName))
			If administratorRole Is Nothing Then
				administratorRole = ObjectSpace.CreateObject(Of CustomSecurityRole)()
				administratorRole.Name = SecurityStrategyComplex.AdministratorRoleName
				administratorRole.IsAdministrative = True
			End If
			Return administratorRole
		End Function
		Private Function CreateUserRole() As CustomSecurityRole
			Dim userRole As CustomSecurityRole = ObjectSpace.FindObject(Of CustomSecurityRole)(New BinaryOperator("Name", "Users"))
			If userRole Is Nothing Then
				userRole = ObjectSpace.CreateObject(Of CustomSecurityRole)()
				userRole.Name = "Users"
				userRole.SetTypePermission(Of Person)(SecurityOperations.FullAccess, SecurityPermissionState.Allow)
				userRole.SetTypePermission(Of PhoneNumber)(SecurityOperations.FullAccess, SecurityPermissionState.Allow)
				userRole.SetTypePermission(Of Address)(SecurityOperations.FullAccess, SecurityPermissionState.Allow)
				userRole.SetTypePermission(Of Country)(SecurityOperations.FullAccess, SecurityPermissionState.Allow)
				userRole.AddObjectPermission(Of SecuritySystemUser)(SecurityOperations.ReadOnlyAccess, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow)
				userRole.HiddenNavigationItems = "AddressGroup, Person_Varied"
			End If
			Return userRole
		End Function
	End Class
End Namespace
