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
using System.Web.Services;


namespace CEPA.CCO.UI.Web
{
    public partial class wfConsulDecla : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, typeof(string), "getYearD", "getYear();", true);

            if (!IsPostBack)
            {
                try
                {

                    txtYear.Text = DateTime.Now.ToString("yyyy");
                    Cargar();
                    
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
                }
            }
        }

        private void Cargar()
        {
            GridView1.DataSource = DetaNavieraDAL.getDeclaraciones(DBComun.Estado.verdadero);
            GridView1.DataBind();

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

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetConte(string prefix, int year)
        {
            List<string> customers = new List<string>();

            //customers = ValidaTarjaDAL.GetContenedor(prefix);


            var query = (from a in ValidaTarjaDAL.GetContenedorDecla(prefix, year)                         
                         select new InfOperaciones
                         {
                             n_contenedor = a.n_contenedor,
                             IdDeta = a.IdDeta
                         }).ToList();



            foreach (var item in query)
            {
                customers.Add(string.Format("{0}-{1}", item.n_contenedor, item.IdDeta));
            }


            return customers.ToArray();
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetDecla(string prefix, int year)
        {
            List<string> customers = new List<string>();

            //customers = ValidaTarjaDAL.GetContenedor(prefix);


            var query = (from a in ValidaTarjaDAL.GetNumDecla(prefix, year)
                         select new InfOperaciones
                         {
                             n_contenedor = a.n_contenedor,
                             IdDeta = a.IdDeta
                         }).ToList();



            foreach (var item in query)
            {
                customers.Add(string.Format("{0}/{1}", item.n_contenedor, item.IdDeta));
            }


            return customers.ToArray();
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

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(txtYear.Text) >= 2016)
                {
                    if (Request.Form["txtConte"].Length == 0 && Request.Form["txtDecla"].Length == 0)
                    {
                        throw new Exception("Debe indicar contenedor o # declaración a buscar");
                    }
                    else
                    {
                        GridView1.DataSource = DetaNavieraDAL.getDeclaracionesFilter(DBComun.Estado.verdadero, Request.Form["txtConte"], Request.Form["txtDecla"], Convert.ToInt32(txtYear.Text));
                        GridView1.DataBind();

                        GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

                        // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                        GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
                        GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
                        GridView1.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";

                        //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

                        GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

                        GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
                        GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
                    }
                }
                else
                {
                    throw new Exception("El año debe poseer 4 dígitos y no puede ser menor que 2016 YYYY (Ej. 2016)");
                }

            }
            catch (Exception ex)
            {
                Cargar();
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
            
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string trans = e.Row.Cells[4].Text;

                if (trans.TrimEnd().TrimStart() == "ROJO")
                {
                    e.Row.BackColor = Color.FromName("#f8d7da");
                    e.Row.ForeColor = Color.FromName("#721c24");
                }
                else if (trans.TrimEnd().TrimStart() == "VERDE")
                {
                    e.Row.BackColor = Color.FromName("#d4edda");
                    e.Row.ForeColor = Color.FromName("#155724");
                }
                else if(trans.TrimEnd().TrimStart() == "AMARILLO")
                {
                    e.Row.BackColor = Color.FromName("#fff3cd");
                    e.Row.ForeColor = Color.FromName("#856404");
                }
            }
        }
    }
}