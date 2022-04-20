using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;

namespace CEPA.CCO.UI.Web
{
    public partial class wfDANEstadistica : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ddlClaves.DataSource = TipoRevisionesDAL.getClaves();
                ddlClaves.DataTextField = "Tipo";
                ddlClaves.DataValueField = "Clave";
                ddlClaves.DataBind();

                ddlClaves.Items.Insert(0, new ListItem("-- Seleccionar Tipo Retencion -- "));
                ddlClaves.Items.Insert(1, new ListItem("Todos"));
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                int a_valor = Convert.ToInt32(txtBuscar.Text);
                string clave_val = ddlClaves.SelectedValue.ToString();

                if (a_valor >= 2014)
                {
                    GridView1.DataSource = DanEstadisticaDAL.ObtenerEstadisticaCL(txtBuscar.Text, "11", clave_val);
                    GridView1.DataBind();

                    if (GridView1.Rows.Count > 0)
                    {
                        GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

                        GridView1.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";
                        GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";


                        GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }

                   
                }
                else
                    throw new Exception("Debe indicar el año a consultar pero no puede ser menor de 2014");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
        }

        private int t_retenidos = 0, t_liberados = 0, t_pendientes = 0, t_liberadosmes = 0;
           
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                t_retenidos += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "retenidos"));
                t_liberados += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "liberados"));
                t_pendientes += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "pendientes"));
                t_liberadosmes += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "libmes"));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTALES:";
                e.Row.Cells[1].Text = t_retenidos.ToString();
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[2].Text = t_liberados.ToString();
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[3].Text = t_pendientes.ToString();
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[4].Text = t_liberadosmes.ToString();
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;

            }

        }
    }
}