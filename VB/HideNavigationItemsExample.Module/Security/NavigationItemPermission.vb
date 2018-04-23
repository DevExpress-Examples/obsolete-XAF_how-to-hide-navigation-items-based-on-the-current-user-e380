Imports System
Imports DevExpress.ExpressApp.Security

Namespace HideNavigationItemsExample.Module.Security
	Public Class NavigationItemPermission
		Implements IOperationPermission

		Public Sub New(ByVal hiddenNavigationItem As String)
			Me._HiddenNavigationItem = hiddenNavigationItem
		End Sub
		Public ReadOnly Property Operation() As String Implements IOperationPermission.Operation
			Get
				Return "NavigateToItem"
			End Get
		End Property
		Private _HiddenNavigationItem As String
		Public Property HiddenNavigationItem() As String
			Get
				Return _HiddenNavigationItem
			End Get
			Set(ByVal value As String)
				_HiddenNavigationItem = value
			End Set
		End Property
	End Class
	Public Class NavigationItemPermissionRequest
		Implements IPermissionRequest

		Public Sub New(ByVal navigationItem As String)
			Me._NavigationItem = navigationItem
		End Sub
		Private _NavigationItem As String
		Public Property NavigationItem() As String
			Get
				Return _NavigationItem
			End Get
			Set(ByVal value As String)
				_NavigationItem = value
			End Set
		End Property
		Public Function GetHashObject() As Object Implements IPermissionRequest.GetHashObject
			Return String.Format("{0} - {1}", Me.GetType().FullName, NavigationItem)
		End Function
	End Class
	Public Class NavigationItemPermissionRequestProcessor
		Inherits PermissionRequestProcessorBase(Of NavigationItemPermissionRequest)

		Private permissions As IPermissionDictionary
		Public Sub New(ByVal permissions As IPermissionDictionary)
			Me.permissions = permissions
		End Sub
		Public Overrides Function IsGranted(ByVal permissionRequest As NavigationItemPermissionRequest) As Boolean
			For Each permission As NavigationItemPermission In permissions.GetPermissions(Of NavigationItemPermission)()
				If permission.HiddenNavigationItem = permissionRequest.NavigationItem Then
					Return False
				End If
			Next permission
			Return True
		End Function
	End Class
End Namespace
