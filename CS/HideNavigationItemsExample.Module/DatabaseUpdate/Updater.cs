using System;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using HideNavigationItemsExample.Module.Security;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;

namespace HideNavigationItemsExample.Module.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            CustomSecurityRole userRole = CreateUserRole();
            CustomSecurityRole administratorRole = CreateAdministratorRole();
            PermissionPolicyUser admin = ObjectSpace.FindObject<PermissionPolicyUser>(new BinaryOperator("UserName", "Admin"));
            if (admin == null) {
                admin = ObjectSpace.CreateObject<PermissionPolicyUser>();
                admin.UserName = "Admin";
                admin.IsActive = true;
                admin.SetPassword("");
                admin.Roles.Add(administratorRole);
                admin.Save();
            }
            PermissionPolicyUser user = ObjectSpace.FindObject<PermissionPolicyUser>(new BinaryOperator("UserName", "User"));
            if (user == null) {
                user = ObjectSpace.CreateObject<PermissionPolicyUser>();
                user.UserName = "User";
                user.IsActive = true;
                user.SetPassword("");
                user.Roles.Add(userRole);
                user.Save();
            }
        }
        private CustomSecurityRole CreateAdministratorRole() {
            CustomSecurityRole administratorRole = ObjectSpace.FindObject<CustomSecurityRole>(
                new BinaryOperator("Name", SecurityStrategyComplex.AdministratorRoleName));
            if (administratorRole == null) {
                administratorRole = ObjectSpace.CreateObject<CustomSecurityRole>();
                administratorRole.Name = SecurityStrategyComplex.AdministratorRoleName;
                administratorRole.IsAdministrative = true;
            }
            return administratorRole;
        }
        private CustomSecurityRole CreateUserRole() {
            CustomSecurityRole userRole = ObjectSpace.FindObject<CustomSecurityRole>(
                new BinaryOperator("Name", "Users"));
            if (userRole == null) {
                userRole = ObjectSpace.CreateObject<CustomSecurityRole>();
                userRole.Name = "Users";
                userRole.SetTypePermission<Person>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                userRole.SetTypePermission<PhoneNumber>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                userRole.SetTypePermission<Address>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                userRole.SetTypePermission<Country>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                userRole.AddObjectPermission<SecuritySystemUser>(SecurityOperations.ReadOnlyAccess, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                userRole.HiddenNavigationItems = "AddressGroup, Person_Varied";
            }
            return userRole;
        }
    }
}
