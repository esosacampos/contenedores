using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace CEPA.CCO.UI.Web.CLIENTES
{
    public class Global : System.Web.HttpApplication
    {
        private static int totalNumberOfUsers = 0;
        private static int currentNumberOfUsers = 0;

        protected void Application_Start(object sender, EventArgs e)
        {
           
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            totalNumberOfUsers += 1;
            currentNumberOfUsers += 1;
        }

        protected void Session_End(Object sender, EventArgs e)
        {
            currentNumberOfUsers -= 1;
        }

        protected void Application_End(Object sender, EventArgs e)
        {

        }

        public static int TotalNumberOfUsers
        {
            get
            {
                return totalNumberOfUsers;
            }
        }

        public static int CurrentNumberOfUsers
        {
            get
            {
                return currentNumberOfUsers;
            }
        } 

    }
}