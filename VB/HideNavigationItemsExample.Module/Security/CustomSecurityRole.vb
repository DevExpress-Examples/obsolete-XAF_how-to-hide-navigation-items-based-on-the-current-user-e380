Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Security.Strategy
Imports DevExpress.Persistent.Base
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp.ConditionalAppearance
Imports DevExpress.ExpressApp.Editors
Imports DevExpress.Persistent.BaseImpl.PermissionPolicy

Namespace HideNavigationItemsExample.Module.Security
	<DefaultClassOptions, ImageName("BO_Role"), Appearance("HideHiddenNavigationItemsForAdministrators", AppearanceItemType:="LayoutItem", TargetItems := "HiddenNavigationItems", Visibility := ViewItemVisibility.Hide, Criteria := "IsAdministrative")> _
	Public Class CustomSecurityRole
		Inherits PermissionPolicyRole
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Private _HiddenNavigationItems As String
		Public Property HiddenNavigationItems() As String
			Get
				Return _HiddenNavigationItems
			End Get
			Set(ByVal value As String)
				SetPropertyValue("HiddenNavigationItems", _HiddenNavigationItems, value)
			End Set
		End Property
		Public Function GetPermissions() As IEnumerable(Of IOperationPermission)
			Dim result As New List(Of IOperationPermission)()
			If (Not String.IsNullOrEmpty(HiddenNavigationItems)) Then
				For Each hiddenNavigationItem As String In HiddenNavigationItems.Split(";"c, ","c)
					result.Add(New NavigationItemPermission(hiddenNavigationItem.Trim()))
				Next hiddenNavigationItem
			End If
			Return result
		End Function
	End Class
End Namespace
