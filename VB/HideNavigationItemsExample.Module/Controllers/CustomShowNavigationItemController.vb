Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp.SystemModule
Imports DevExpress.ExpressApp
Imports HideNavigationItemsExample.Module.Security

Namespace HideNavigationItemsExample.Module.Controllers
	Public Class CustomShowNavigationItemController
		Inherits ShowNavigationItemController
		Protected Overrides Sub SynchItemWithSecurity(ByVal item As DevExpress.ExpressApp.Actions.ChoiceActionItem)
			If (Not SecuritySystem.IsGranted(New NavigationItemPermissionRequest(item.Id))) Then
				item.Active(SecurityVisibleKey) = False
			Else
				MyBase.SynchItemWithSecurity(item)
			End If
		End Sub
	End Class
End Namespace
