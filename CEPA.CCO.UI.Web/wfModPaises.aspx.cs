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
using System.Text;
using System.Xml;
using System.IO;
using System.Web.Services;
using System.Drawing;

namespace CEPA.CCO.UI.Web
{
    public partial class wfModPaises : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    Cargar();


                }
                catch (Exception ex)
                {

                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Este módulo presento problemas, intentarlo de nuevo o consultar a Informatica');", true);
                }
            }
        }

        private void Cargar()
        {

           
            GridView1.DataSource = PaisDAL.getPaises(DBComun.TipoBD.SqlServer);
            GridView1.DataBind();

            
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

            GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
            GridView1.FooterRow.TableSection = TableRowSection.TableFooter;

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
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/default.aspx");
        }
        protected void btnCargar_Click(object sender, EventArgs e)
        {
            //Thread.Sleep(4000);

            //ClientScript.RegisterStartupScript(GetType(), "proceso", "almacenando();", true);

            foreach (GridViewRow row in GridView1.Rows)
            {
                //Get the HobbyId from the DataKey property.
                string CountryCode = Convert.ToString(GridView1.DataKeys[row.RowIndex].Values[0]);

                //Get the checked value of the CheckBox.
                bool b_oirsa = (row.FindControl("CheckBox1") as CheckBox).Checked;


                PaisDAL.saveCountry(CountryCode, b_oirsa);

            }
            ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Registrado Correctamente!!!');", true);

            Cargar();
            

            //string mensaje = "prueba";

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //change it according your cell number or find element
                if (e.Row.Cells[1].Text == "Verdadero")
                    e.Row.BackColor = Color.FromArgb(250, 247, 106);                
            }
        }
    }
}