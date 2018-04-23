using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp;
using HideNavigationItemsExample.Module.Security;

namespace HideNavigationItemsExample.Module.Controllers {
    public class CustomShowNavigationItemController : ShowNavigationItemController {
        protected override void SynchItemWithSecurity(DevExpress.ExpressApp.Actions.ChoiceActionItem item) {
            if (!SecuritySystem.IsGranted(new NavigationItemPermissionRequest(item.Id))) {
                item.Active[SecurityVisibleKey] = false;
            } else {
                base.SynchItemWithSecurity(item);
            }
        }
    }
}
