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
    public partial class wfConsultaTrans : System.Web.UI.Page
    {
        public string c_naviera;
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
                                tot_imp.Text = item.TotalImp.ToString();
                                tot_trans.Text = item.TotalTrans.ToString();
                                total_arco.Text = item.TotalTransA.ToString();
                                lblPO.Text = item.TotalPTransA.ToString();
                                lblPP.Text = item.TotalPTrans.ToString();
                            }

                            Cargar(c_llegada.Text);
                        }
                        else
                        {
                           
                        }

                    }

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }

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

        private void Cargar(string c_llegada)
        {
            GridView1.DataSource = DocBuqueLINQ.ObtenerTransmiConsul(c_llegada);
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
             if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string trans = e.Row.Cells[8].Text;
  
                if (trans.TrimEnd().TrimStart() == "NO")
                {
                    e.Row.BackColor = Color.FromName("#ffeb9c");
                    
                }

                string rt = e.Row.Cells[6].Text;

                if (rt.TrimEnd().TrimStart() == "RT")
                {
                    e.Row.BackColor = Color.FromName("#ffeb9c");

                }

                string tarja = e.Row.Cells[5].Text;

                 if (tarja == "&nbsp;")
                 {
                     e.Row.BackColor = Color.FromName("#EB7A7A");
                     e.Row.ForeColor = Color.White;
                     e.Row.Cells[5].Text = "SIN TARJA";
                 }

                 string b_arco = e.Row.Cells[10].Text;

                 if (b_arco == "NOT")
                 {
                     e.Row.BackColor = Color.FromName("#EB7A7A");
                     e.Row.ForeColor = Color.White;                     
                 }

                 string arco = e.Row.Cells[11].Text;

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

                e.Row.Cells[0].Controls.Add(new LiteralControl("<ul class='pagination pagination-centered hide-if-no-paging'></ul>"));
            }
        }

       
    }
}