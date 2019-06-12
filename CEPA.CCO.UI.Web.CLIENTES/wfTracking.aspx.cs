using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;
using Recaptcha;
using System.Net;
using System.IO;
using System.Xml;
using System.Text;


using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Drawing;
using System.Configuration;

namespace CEPA.CCO.UI.Web.CLIENTES
{
    public partial class wfTracking : System.Web.UI.Page
    {
        private static readonly DateTime FIRST_GOOD_DATE = new DateTime(1900, 01, 01);
        private static readonly DateTime FIRST_GOOD_DATE1 = new DateTime(2016, 01, 01, 00, 00, 00);
        public int b_sidunea = 0;

        protected void Page_Load(object sender, EventArgs e)
        {



            if (!IsPostBack)
            {

            }
        }
        
    }
}