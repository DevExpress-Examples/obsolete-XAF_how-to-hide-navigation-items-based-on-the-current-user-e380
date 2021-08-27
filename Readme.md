<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128590865/16.1.4%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E380)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* **[CustomShowNavigationItemController.cs](./CS/HideNavigationItemsExample.Module/Controllers/CustomShowNavigationItemController.cs) (VB: [CustomShowNavigationItemController.vb](./VB/HideNavigationItemsExample.Module/Controllers/CustomShowNavigationItemController.vb))**
* [Module.cs](./CS/HideNavigationItemsExample.Module/Module.cs)
* [CustomSecurityRole.cs](./CS/HideNavigationItemsExample.Module/Security/CustomSecurityRole.cs) (VB: [CustomSecurityRole.vb](./VB/HideNavigationItemsExample.Module/Security/CustomSecurityRole.vb))
* [NavigationItemPermission.cs](./CS/HideNavigationItemsExample.Module/Security/NavigationItemPermission.cs) (VB: [NavigationItemPermission.vb](./VB/HideNavigationItemsExample.Module/Security/NavigationItemPermission.vb))
* [Global.asax.cs](./CS/HideNavigationItemsExample.Web/Global.asax.cs) (VB: [Global.asax.vb](./VB/HideNavigationItemsExample.Web/Global.asax.vb))
* [Program.cs](./CS/HideNavigationItemsExample.Win/Program.cs) (VB: [Program.vb](./VB/HideNavigationItemsExample.Win/Program.vb))
<!-- default file list end -->
# How to hide navigation items based on the current user


<p>Our <a href="https://documentation.devexpress.com/#Xaf/CustomDocument2650">Security System</a> allows hiding navigation items for certain users by configuring their Navigation permissions. These permissions can be configured in two modes.<br><br>Starting with <strong>version 16.2</strong>, Navigation Permissions can be assigned to individual navigation items. This feature is described in the <strong>Navigation Permissions</strong> section of the <a href="https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument113366">Security System Overview</a> article. This feature is enabled by default in a new project created using the Wizard. To enable it when upgrading from an older version, it is necessary to set the <a href="https://documentation.devexpress.com/#eXpressAppFramework/DevExpressExpressAppSecuritySecurityStrategy_SupportNavigationPermissionsForTypestopic">SecurityStrategy.SupportNavigationPermissionsForTypes</a> option to <strong>false</strong>. If the project is based on Entity Framework, it is also necessary to update the database. For this, use the approach described in this Knowledge Base article: <a href="https://www.devexpress.com/Support/Center/p/T459507">How to: Add Navigation Permissions to an Entity Framework Application Created with XAF 16.1</a>.<br><br>In <strong>version</strong> <strong>16.1</strong> and older, navigation permissions can be assigned to a specific object type via the <strong>AllowNavigate</strong> option available in the <strong>Type Permissions</strong> settings. All navigation items specific to a corresponding type are removed from the navigation control if the current user does not have the permission for navigating to this type. This mode is enabled by default when upgrading an old project to version 16.2 and higher. To enable it in a new project created using the Wizard, open the application designer and change the <a href="https://documentation.devexpress.com/#eXpressAppFramework/DevExpressExpressAppSecuritySecurityStrategy_SupportNavigationPermissionsForTypestopic">SecurityStrategy.SupportNavigationPermissionsForTypes</a> property value to <strong>false.</strong></p>
<p><br>This example demonstrates how to implement the first mode manually. The approach demonstrated in this example is useful if the project uses an old XAF version that does not yet have the Navigation Permissions feature implemented.<strong><br><br>Note:</strong> If you use the solution provided in this example and <strong>upgrade to version 16.2</strong>, this code may stop working properly. In this case, use recommendations provided at the end of this article.<br><br>If your XAF version is <strong>less than 16.2</strong> and you need to grant permissions to individual navigation items (e.g., to a DashboardView or to a certain ListView model), use the solution described below to extend the Security System's functionality. In this example, the HiddenNavigationItems property allowing you to hide navigation items by their ID will be added to the role class.<br><br>The approach with overriding the ShowNavigationItemController.SynchItemWithSecurity method shown in this example can be also appropriate for tasks that are not related to the Security System directly. You can hide or customize any navigation item in this manner.</p>
<p> </p>
<p><strong>Steps to implement:</strong><br><br></p>
<p>1. Implement a custom permission type - <strong>NavigationItemPermission</strong> - that can be used to check access permissions for a certain navigation item by its ID.<br>2. Implement a custom permission request - <strong>NavigationItemPermissionRequest</strong> - that will be sent to check whether the current user has access to a certain navigation item.<br>3. Implement a custom permission request processor - <strong>NavigationItemPermissionRequestProcessor</strong> - that will determine whether the current user has permissions for the received permission request.<br>4. Implement a custom role with the <strong>HiddenNavigationItems</strong> property. Extend it with the <strong>GetPermissions</strong> method to create NavigationPermission instances based on the value of the HiddenNavigationItems property.<br>5. Specify the custom role in the Security System's <strong>RoleType</strong> property in the Application Designer, as described in the <a href="https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113384.aspx">How to: Implement Custom Security Objects (Users, Roles, Operation Permissions)</a> topic.<br>6. Register your permission request processor in the application by handling the <strong>SecurityStrategy.CustomizeRequestProcessors</strong> event in the Program.cs and Global.asax.cs files.<br>7. Implement a ShowNavigationItemController's descendant - <strong>CustomShowNavigationItemController</strong> - and override its <strong>SynchItemWithSecurity</strong> method to deactivate navigation items prohibited by the CustomSecurityRole.HiddenNavigationItems property.<br><br>After implementing these steps in your project, you will be able to assign a role with the HiddenNavigationItems property to required users to restrict their access to certain navigation items.<br><br></p>
<p><strong>Note:</strong> The example is based on the <strong>PermissionPolicyRole</strong> and <strong>PermissionPolicyUser</strong> classes. These classes are used by the Security System when selecting the Allow/Deny permissions policy in the Solution Wizard. If your project was created using an earlier XAF version (prior to 16.1), and the <strong>SecuritySystemRole</strong> and <strong>SecuritySystemUser</strong> classes are used in it, change the version number in the combo box below to see an example for these classes.<br><br><strong>Upgrade note for version 16.2:</strong><br>The code used in older versions of this example can stop working after upgrading to this version. To fix this issue, either copy the relevant code from the new version of the example, or modify the CustomShowNavigationItemController class by adding this method:</p>


```cs
protected override bool SyncItemsWithRequestSecurity(DevExpress.ExpressApp.Actions.ChoiceActionItemCollection items) {
	base.SyncItemsWithSecurity(items);
	return true;
}
```




```vb
Protected Overrides Function SyncItemsWithRequestSecurity(ByVal items As DevExpress.ExpressApp.Actions.ChoiceActionItemCollection) As Boolean
	MyBase.SyncItemsWithSecurity(items)
	Return True
End Function
```



<br/>


