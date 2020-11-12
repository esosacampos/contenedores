using System;
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
using System.Web.Services;
using System.Reflection;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Linq;

namespace CEPA.CCO.UI.Web
{
    public partial class wfTipoCliente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    Cargar();
                }
                catch(Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
                }

            }
        }

        private void Cargar()
        {
            List<TipoCliente> pCliente = new List<TipoCliente>();

            pCliente = ClienteDAL.getTipoCliente(DBComun.Estado.verdadero);

            Session["lstCliente"] = pCliente;            
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["lstCliente"] != null)
                {
                    List<TipoCliente> pClientes = new List<TipoCliente>();

                    pClientes = Session["lstCliente"] as List<TipoCliente>;

                    GridView1.DataSource = pClientes.Where(a => a.s_nombre_comercial.Contains(txtCliente.Text.ToUpper()));
                    GridView1.DataBind();

                    if (GridView1.Rows.Count > 0)
                    {
                        GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

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
                else
                {
                    Cargar();
                }

            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
        }
    }
}