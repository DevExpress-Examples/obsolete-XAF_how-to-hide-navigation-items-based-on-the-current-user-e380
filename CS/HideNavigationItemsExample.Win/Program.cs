using System;
using System.Configuration;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using HideNavigationItemsExample.Module.Security;
using System.Collections.Generic;

namespace HideNavigationItemsExample.Win {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
#if EASYTEST
            DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            HideNavigationItemsExampleWindowsFormsApplication winApplication = new HideNavigationItemsExampleWindowsFormsApplication();
            // Refer to the http://documentation.devexpress.com/#Xaf/CustomDocument2680 help article for more details on how to provide a custom splash form.
            //winApplication.SplashScreen = new DevExpress.ExpressApp.Win.Utils.DXSplashScreen("YourSplashImage.png");
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
            try {
                ((SecurityStrategy)winApplication.Security).CustomizeRequestProcessors +=
                    delegate(object sender, CustomizeRequestProcessorsEventArgs e) {
                        List<IOperationPermission> result = new List<IOperationPermission>();
                        SecurityStrategyComplex security = sender as SecurityStrategyComplex;
                        if (security != null) {
                            ISecurityUserWithRoles user = security.User as ISecurityUserWithRoles;
                            if (user != null) {
                                foreach (ISecurityRole role in user.Roles) {
                                    if (role is CustomSecurityRole) {
                                        result.AddRange(((CustomSecurityRole)role).GetPermissions());
                                    }
                                }
                            }
                        }
                        IPermissionDictionary permissionDictionary = new PermissionDictionary((IEnumerable<IOperationPermission>)result);
                        e.Processors.Add(typeof(NavigationItemPermissionRequest), new NavigationItemPermissionRequestProcessor(permissionDictionary));
                    };
                winApplication.Setup();
                winApplication.Start();
            } catch (Exception e) {
                winApplication.HandleException(e);
            }
        }
    }
}
