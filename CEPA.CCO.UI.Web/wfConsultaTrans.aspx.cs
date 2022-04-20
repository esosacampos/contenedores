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

using Newtonsoft.Json;


namespace CEPA.CCO.UI.Web
{
    public partial class wfConsultaTrans : System.Web.UI.Page
    {
        public string c_naviera;
        int t_lineas = 0, t_import = 0, t_cancel = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                try
                {

                    if (Request.QueryString["c_llegada"] == null)
                    {
                        throw new Exception("Falta código de cabecera");
                    }
                    else
                    {
                        List<DocBuque> _encaList = DocBuqueLINQ.ObtenerTransmi(Request.QueryString["c_llegada"].ToString());
                        //Sacar naviera
                        if (_encaList.Count > 0)
                        {
                            foreach (DocBuque item in _encaList)
                            {
                                c_imo.Text = item.c_imo;
                                d_buque.Text = item.d_buque;
                                f_llegada.Text = item.f_llegada.ToString("dd/MM/yyyy HH:mm:ss");
                                c_llegada.Text = item.c_llegada;
                                t_import = (int)item.TotalImp;
                                t_cancel = (int)item.TotalCancel;
                                tot_trans.Text = item.TotalTrans.ToString();
                                total_arco.Text = item.TotalTransA.ToString();
                                lblPO.Text = item.TotalPTransA.ToString();
                                lblPP.Text = item.TotalPTrans.ToString();
                                hBuque.Value = " " + item.c_llegada + " - " + item.d_buque;
                                hLlegada.Value = item.c_llegada;
                                break;
                            }

                            Cargar(c_llegada.Text);

                            tot_lineasa.Text = t_lineas.ToString();
                            tot_cancel.Text = t_cancel.ToString();
                            tot_imp.Text = t_import.ToString();
                        }
                        else
                        {

                        }

                    }

                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + "Error: Durante la ejecución recomendamos volverlo intentar, o reportar a Informatica" + "');", true);

                }
            }


        }

        private void Cargar(string c_llegada)
        {
            GridView1.DataSource = DocBuqueLINQ.ObtenerTransmiConsul(c_llegada);
            GridView1.DataBind();

            t_lineas = GridView1.Rows.Count;


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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string trans = e.Row.Cells[11].Text;

                if (trans.TrimEnd().TrimStart() == "NO")
                {
                    e.Row.BackColor = Color.FromName("#ffeb9c");

                }

                string rt = e.Row.Cells[7].Text;

                if (rt.TrimEnd().TrimStart() == "RT")
                {
                    e.Row.BackColor = Color.FromName("#ffeb9c");

                }

                string tarja = e.Row.Cells[6].Text;
                string estado = e.Row.Cells[5].Text;

                if (tarja == "&nbsp;" && !estado.Contains("VACIO"))
                {
                    e.Row.BackColor = Color.FromName("#EB7A7A");
                    e.Row.ForeColor = Color.White;
                    e.Row.Cells[6].Text = "SIN TARJA";
                }
                else if (tarja == "&nbsp;" && estado.Contains("VACIO"))
                {
                    e.Row.BackColor = Color.FromName("#F2F2F2");
                    e.Row.ForeColor = Color.FromName("#FA7D00");
                    e.Row.Cells[6].Text = "NO REQ. TARJA";
                }

                string b_arco = e.Row.Cells[8].Text;

                if (b_arco == "NOT")
                {
                    e.Row.BackColor = Color.FromName("#EB7A7A");
                    e.Row.ForeColor = Color.White;
                }

                string arco = e.Row.Cells[9].Text;

                if (arco == "SIN RECEPCION OIRSA")
                {
                    e.Row.BackColor = Color.FromName("#EB7A7A");
                    e.Row.ForeColor = Color.White;
                }
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfBuquesTrans.aspx");
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

        [System.Web.Services.WebMethod]
        public static string getResumen(string c_llegada)
        {
            try
            {
                List<RecepEsta> consulta = new List<RecepEsta>();
                consulta = RecepEstaDAL.getResumenDAL(DBComun.Estado.verdadero, c_llegada);
                var query = (from a in consulta
                             select new RecepEsta
                             {
                                 c_tamaño = a.c_tamaño,
                                 manifestados = a.manifestados,
                                 cancelados = a.cancelados,
                                 total = a.total,
                                 recibidos = a.recibidos,
                                 pendientes = a.pendientes
                             }).ToList();


                var jConsult = Newtonsoft.Json.JsonConvert.SerializeObject(query);

                return jConsult;
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);
            }          
        }

        [System.Web.Services.WebMethod]
        public static string getDetalle(string c_llegada)
        {
            try
            {
                List<RecepEsta> consulta = new List<RecepEsta>();
                consulta = RecepEstaDAL.getDetalleDAL(DBComun.Estado.verdadero, c_llegada);
                var query = (from a in consulta
                             select new RecepEsta
                             {
                                 c_naviera = a.c_naviera,
                                 c_tamaño = a.c_tamaño,
                                 manifestados = a.manifestados,
                                 cancelados = a.cancelados,
                                 total = a.total,
                                 recibidos = a.recibidos,
                                 pendientes = a.pendientes
                             }).ToList();


                var jConsult = Newtonsoft.Json.JsonConvert.SerializeObject(query);

                return jConsult;
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);
            }
        }
    }
}