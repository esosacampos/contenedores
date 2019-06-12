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

namespace CEPA.CCO.UI.Web.UCC
{
    public partial class wfGenerarOficioUCC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Datepicker.Text = DateTime.Now.Year.ToString();
            }
        }

        protected void brnReporte_Click(object sender, EventArgs e)
        {
            int a_folio = 0;
            try
            {
                if (Datepicker.Text.Trim().TrimEnd().TrimStart() == "" || Datepicker.Text.Trim().TrimEnd().TrimStart() == string.Empty)
                {
                    a_folio = 2018;
                }

                if (Datepicker.Text.Trim().TrimEnd().TrimStart().Length == 4)
                {
                    a_folio = Convert.ToInt32(Datepicker.Text.Trim().TrimEnd().TrimStart());
                }
                else
                {
                    throw new Exception("Indicar año de oficio en formato 4 digitos Ej. 2018");
                }

                //ReportViewer1.Visible is set to false in design mode
                ReportViewer1.Visible = true;


                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/UCC/rptOficioUCC.rdlc");

                ReportDataSource datasource = new
                  ReportDataSource("OficioSet",
                  DocBuqueLINQ.ObtenerOficioReportUCC(TextBox1.Text, a_folio));

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                if (DocBuqueLINQ.ObtenerOficioReportUCC(TextBox1.Text, a_folio).Count == 0)
                {
                    throw new Exception("No produjo resultados ese número de oficio");                      
                }
               
                ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
                ReportViewer1.Visible = false;
            }
            
        }

        
    }
}