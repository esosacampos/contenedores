using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using System.Data.SqlClient;
using CEPA.CCO.Linq;
using System.Threading;

namespace CEPA.CCO.UI.Web.DAN
{
    public partial class wfGenerarOficio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void brnReporte_Click(object sender, EventArgs e)
        {
            try
            {
                //ReportViewer1.Visible is set to false in design mode
                ReportViewer1.Visible = true;


                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/DAN/rptOficio.rdlc");

                ReportDataSource datasource = new
                  ReportDataSource("OficioSet",
                  DocBuqueLINQ.ObtenerOficioReport(TextBox1.Text));

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                if (DocBuqueLINQ.ObtenerOficioReport(TextBox1.Text).Count == 0)
                {
                    lblMessage.Text = "No produjo resultados ese número de oficio";
                    ReportViewer1.Visible = false;
                }
               
                ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
            
        }

        
    }
}