using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;

namespace HideNavigationItemsExample.Module.Security {
    [DefaultClassOptions, ImageName("BO_Role")]
    [Appearance("HideHiddenNavigationItemsForAdministrators", AppearanceItemType="LayoutItem", TargetItems = "HiddenNavigationItems", Visibility = ViewItemVisibility.Hide, Criteria = "IsAdministrative")]
    public class CustomSecurityRole : SecuritySystemRole {
        public CustomSecurityRole(Session session) : base(session) { }
        private string _HiddenNavigationItems;
        public string HiddenNavigationItems {
            get { return _HiddenNavigationItems; }
            set { SetPropertyValue("HiddenNavigationItems", ref _HiddenNavigationItems, value); }
        }
        protected override IEnumerable<IOperationPermission> GetPermissionsCore() {
            List<IOperationPermission> result = new List<IOperationPermission>();
            result.AddRange(base.GetPermissionsCore());
            if (!String.IsNullOrEmpty(HiddenNavigationItems)) {
                foreach (string hiddenNavigationItem in HiddenNavigationItems.Split(';', ',')) {
                    result.Add(new NavigationItemPermission(hiddenNavigationItem.Trim()));
                }
            }
            return result;
        }
    }
}
