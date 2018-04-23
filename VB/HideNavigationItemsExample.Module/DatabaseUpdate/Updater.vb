Imports Microsoft.VisualBasic
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
            Dim admin As SecuritySystemUser = ObjectSpace.FindObject(Of SecuritySystemUser)(New BinaryOperator("UserName", "Sam"))
            If admin Is Nothing Then
                admin = ObjectSpace.CreateObject(Of SecuritySystemUser)()
                admin.UserName = "Sam"
                admin.IsActive = True
                admin.SetPassword("")
                admin.Roles.Add(administratorRole)
                admin.Save()
            End If
            Dim user As SecuritySystemUser = ObjectSpace.FindObject(Of SecuritySystemUser)(New BinaryOperator("UserName", "John"))
            If user Is Nothing Then
                user = ObjectSpace.CreateObject(Of SecuritySystemUser)()
                user.UserName = "John"
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
                userRole.SetTypePermissions(Of Person)(SecurityOperations.FullAccess, SecuritySystemModifier.Allow)
                userRole.SetTypePermissions(Of PhoneNumber)(SecurityOperations.FullAccess, SecuritySystemModifier.Allow)
                userRole.SetTypePermissions(Of Address)(SecurityOperations.FullAccess, SecuritySystemModifier.Allow)
                userRole.SetTypePermissions(Of Country)(SecurityOperations.FullAccess, SecuritySystemModifier.Allow)
                userRole.AddObjectAccessPermission(Of SecuritySystemUser)("[Oid] = CurrentUserId()", SecurityOperations.ReadOnlyAccess)
                userRole.HiddenNavigationItems = "AddressGroup, Person_Varied"
            End If
            Return userRole
        End Function
    End Class
End Namespace
