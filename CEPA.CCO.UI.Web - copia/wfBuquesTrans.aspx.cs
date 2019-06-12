using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using System.Data.SqlClient;
using CEPA.CCO.Linq;
using System.Threading;

namespace CEPA.CCO.UI.Web
{
    public partial class wfBuquesTrans : System.Web.UI.Page
    {
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
                    Response.Write("<script>alert('" + Ex.Message + "');</script>");
                }
            }
            //GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

            //// GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
            //GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
            //GridView1.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
            //GridView1.HeaderRow.Cells[6].Attributes["data-hide"] = "phone";
            //GridView1.HeaderRow.Cells[7].Attributes["data-hide"] = "phone";            
            ////GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

            //GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

            //GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
            //GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
            ////  ViewState["EmployeeList"] = GridView1.DataSource;
        }

        private void Cargar()
        {
            EncaBuqueBL _encaBL = new EncaBuqueBL();

            GridView1.DataSource = DocBuqueLINQ.ObtenerDAN_Trans();
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
                Response.Write("<script>alert('" + ex.Message + "');</script>");
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

                e.Row.Cells[0].Controls.Add(new LiteralControl("<ul class='pagination pagination-centered hide-if-no-paging'></ul>"));
            }
        }

        
    }
}