using System;
using DevExpress.ExpressApp.Security;

namespace HideNavigationItemsExample.Module.Security {
    public class NavigationItemPermission : IOperationPermission {
        public NavigationItemPermission(string hiddenNavigationItem) {
            this._HiddenNavigationItem = hiddenNavigationItem;
        }
        public string Operation {
            get { return "NavigateToItem"; }
        }
        private string _HiddenNavigationItem;
        public string HiddenNavigationItem {
            get { return _HiddenNavigationItem; }
            set {
                _HiddenNavigationItem = value;
            }
        }
    }
    public class NavigationItemPermissionRequest : IPermissionRequest {
        public NavigationItemPermissionRequest(string navigationItem) {
            this._NavigationItem = navigationItem;
        }
        private string _NavigationItem;
        public string NavigationItem {
            get { return _NavigationItem; }
            set {
                _NavigationItem = value;
            }
        }
        public object GetHashObject() {
            return String.Format("{0} - {1}", this.GetType().FullName, NavigationItem);
        }
    }
    public class NavigationItemPermissionRequestProcessor :
    PermissionRequestProcessorBase<NavigationItemPermissionRequest> {
        private IPermissionDictionary permissions;
        public NavigationItemPermissionRequestProcessor(IPermissionDictionary permissions) {
            this.permissions = permissions;
        }
        public override bool IsGranted(NavigationItemPermissionRequest permissionRequest) {
            foreach (NavigationItemPermission permission in permissions.GetPermissions<NavigationItemPermission>()) {
                if (permission.HiddenNavigationItem == permissionRequest.NavigationItem) {
                    return false;
                }
            }
            return true;
        }
    }
}
