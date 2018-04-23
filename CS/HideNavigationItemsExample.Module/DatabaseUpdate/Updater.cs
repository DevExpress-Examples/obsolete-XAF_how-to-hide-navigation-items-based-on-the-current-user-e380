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

namespace HideNavigationItemsExample.Module.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            CustomSecurityRole userRole = CreateUserRole();
            CustomSecurityRole administratorRole = CreateAdministratorRole();
            SecuritySystemUser admin = ObjectSpace.FindObject<SecuritySystemUser>(new BinaryOperator("UserName", "Sam"));
            if (admin == null) {
                admin = ObjectSpace.CreateObject<SecuritySystemUser>();
                admin.UserName = "Sam";
                admin.IsActive = true;
                admin.SetPassword("");
                admin.Roles.Add(administratorRole);
                admin.Save();
            }
            SecuritySystemUser user = ObjectSpace.FindObject<SecuritySystemUser>(new BinaryOperator("UserName", "John"));
            if (user == null) {
                user = ObjectSpace.CreateObject<SecuritySystemUser>();
                user.UserName = "John";
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
                userRole.SetTypePermissions<Person>(SecurityOperations.FullAccess, SecuritySystemModifier.Allow);
                userRole.SetTypePermissions<PhoneNumber>(SecurityOperations.FullAccess, SecuritySystemModifier.Allow);
                userRole.SetTypePermissions<Address>(SecurityOperations.FullAccess, SecuritySystemModifier.Allow);
                userRole.SetTypePermissions<Country>(SecurityOperations.FullAccess, SecuritySystemModifier.Allow);
                userRole.AddObjectAccessPermission<SecuritySystemUser>("[Oid] = CurrentUserId()", SecurityOperations.ReadOnlyAccess);
                userRole.HiddenNavigationItems = "AddressGroup, Person_Varied";
            }
            return userRole;
        }
    }
}
