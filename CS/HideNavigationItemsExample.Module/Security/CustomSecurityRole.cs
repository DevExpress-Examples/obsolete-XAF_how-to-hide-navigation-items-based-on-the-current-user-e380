using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;

namespace HideNavigationItemsExample.Module.Security {
    [DefaultClassOptions, ImageName("BO_Role")]
    [Appearance("HideHiddenNavigationItemsForAdministrators", AppearanceItemType="LayoutItem", TargetItems = "HiddenNavigationItems", Visibility = ViewItemVisibility.Hide, Criteria = "IsAdministrative")]
    public class CustomSecurityRole : PermissionPolicyRole {
        public CustomSecurityRole(Session session) : base(session) { }
        private string _HiddenNavigationItems;
        public string HiddenNavigationItems {
            get { return _HiddenNavigationItems; }
            set { SetPropertyValue("HiddenNavigationItems", ref _HiddenNavigationItems, value); }
        }
        public IEnumerable<IOperationPermission> GetPermissions() {
            List<IOperationPermission> result = new List<IOperationPermission>();
            if (!String.IsNullOrEmpty(HiddenNavigationItems)) {
                foreach (string hiddenNavigationItem in HiddenNavigationItems.Split(';', ',')) {
                    result.Add(new NavigationItemPermission(hiddenNavigationItem.Trim()));
                }
            }
            return result;
        }
    }
}
