using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using CEPA.CCO.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cxExcel = ClosedXML.Excel;
using Font = iTextSharp.text.Font;

namespace CEPA.CCO.UI.Web
{
    public partial class wfTransmisionAutoP : System.Web.UI.Page
    {

        public string SanitizeXmlString(string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            StringBuilder buffer = new StringBuilder(xml.Length);

            foreach (char c in xml)
            {
                if (IsLegalXmlChar(c))
                {
                    buffer.Append(c);
                }
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Whether a given character is allowed by XML 1.0.
        /// </summary>
        public bool IsLegalXmlChar(int character)
        {
            return
            (
                 character == 0x9 /* == '\t' == 9   */          ||
                 character == 0xA /* == '\n' == 10  */          ||
                 character == 0xD /* == '\r' == 13  */          ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    Cargar();

                }
                catch (Exception Ex)
                {                    
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + "Error: " + Ex.Message + "');", true);
                }
            }

            RegisterPostBackControl();
        }

        private void Cargar()
        {
            EncaBuqueBL _encaBL = new EncaBuqueBL();

            GridView1.DataSource = DocBuqueLINQ.ObtenerTrans_Auto();
            GridView1.DataBind();

            GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand"; 

            // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
            //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

            GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
            GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
            //  ViewState["EmployeeList"] = GridView1.DataSource;
        }


        public void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Cargar();
            }
            catch (Exception ex)
            {
                Response.Write("<script>bootbox.alert('" + ex.Message + "');</script>");
            }
        }

        protected void onRowCreate(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int colSpan = e.Row.Cells.Count;

                for (int i = (e.Row.Cells.Count - 1); i >= 1; i -= 1)
                {
                    e.Row.Cells.RemoveAt(i);
                    e.Row.Cells[0].ColumnSpan = colSpan;
                }

                e.Row.Cells[0].Controls.Add(new LiteralControl("<ul class='pagination pagination-centered hide-if-no-paging'></ul><div class='divider'  style='margin-bottom: 15px;'></div></div><span class='label label-default pie' style='background-color: #dff0d8;border-radius: 25px;font-family: sans-serif;font-size: 18px;color: #468847;border-color: #d6e9c6;margin-top: 18px;'></span>"));
            }
        }

        public void RegisterPostBackControl()
        {
            foreach (GridViewRow row in GridView1.Rows)
            {                
                ScriptManager current = ScriptManager.GetCurrent(Page);
                if (current != null)
                {
                    /*urrent.RegisterPostBackControl(lnkFull);*/
                }
            }
        }

    }
}





