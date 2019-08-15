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
using System.Globalization;
using System.Drawing;



namespace CEPA.CCO.UI.Web
{
    public partial class wfConsulBL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBuscar.Text))
                    throw new Exception("Debe indicar el # de BL a buscar");
                else
                {
                    GridView1.DataSource = BlDAL.getBL(txtBuscar.Text, DBComun.Estado.verdadero);
                    GridView1.DataBind();

                    GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "phone";

                    // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                    GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
                    GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
                    GridView1.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";

                    //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

                    GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

                    GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
                    GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
                    //  ViewState["EmployeeList"] = GridView1.DataSource;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + "Error: Durante la ejecución recomendamos volverlo intentar, o reportar a Informatica " + ex.Message + "');", true);
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
       
    }
}