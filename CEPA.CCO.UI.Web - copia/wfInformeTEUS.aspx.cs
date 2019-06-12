using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;

namespace CEPA.CCO.UI.Web
{
    public partial class wfInformeTEUS : System.Web.UI.Page
    {
        private int year, ti_lleno = 0, ti_vacios = 0, t_impor = 0, te_llenos = 0, te_vacios = 0, t_expor = 0, t_uni = 0, t_Te = 0;
        private int tei_lleno = 0, tei_vacio = 0, tee_lleno = 0, tee_vacio = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string s_buque = null, f_desatraque = null;
            try
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                if (txtBuscar.Text == "" || txtBuscar.Text.Length == 0)
                {
                    year = DetaNavieraLINQ.YearBD();
                }
                else
                {
                    if (ArchivoBookingDAL.isNumeric(txtBuscar.Text))
                    {
                        if (txtBuscar.Text.Length == 4)
                        {
                            year = int.Parse(txtBuscar.Text);
                        }
                        else
                            throw new Exception("El año de consulta debe poseer 4 digitos");
                    }
                    else
                    {
                        throw new Exception("Debe indicar el año a consultar en números");
                    }
                }

                if (year < 2013)
                    throw new Exception("Año minimo de consulta 2013");


                List<TEUSResu> pLista = new List<TEUSResu>();

                pLista = TEUSAnaDAL.ObtenerResumen(DBComun.Estado.verdadero, year);

                if (pLista.Count > 0)
                {

                    foreach (var item in pLista)
                    {
                        s_buque = item.s_nombre_buque;
                        f_desatraque = item.f_desatraque.ToString("dd/MM/yyyy HH:mm");
                        break;
                    }

                    GridView1.DataSource = pLista;
                    GridView1.DataBind();

                    if (GridView1.Rows.Count > 0)
                    {
                        GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

                        GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
                        GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
                        GridView1.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";

                        GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
                        GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
                        GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
                    }

                    lblMensaje.Text = "Última fecha de actualización " + f_desatraque + " - (Buque : " + s_buque + ")" ;
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ti_lleno += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "vi_luni"));
                ti_vacios += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "vi_vuni"));
                t_impor += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "t_import"));
                te_llenos += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ve_luni"));
                te_vacios += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ve_vuni"));
                t_expor += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "t_export"));
                t_uni += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "t_uni"));
                t_Te += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "t_teus"));

                tei_lleno += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "vi_lteus"));
                tei_vacio += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "vi_vteus"));
                tee_lleno += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ve_lteus"));
                tee_vacio += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ve_vteus"));

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTALES ....";
                e.Row.Cells[1].Text = ti_lleno.ToString("N0");
                e.Row.Cells[2].Text = tei_lleno.ToString("N0");
                e.Row.Cells[3].Text = ti_vacios.ToString("N0");
                e.Row.Cells[4].Text = tei_vacio.ToString("N0");
                e.Row.Cells[5].Text = t_impor.ToString("N0");

                e.Row.Cells[6].Text = te_llenos.ToString("N0");
                e.Row.Cells[7].Text = tee_lleno.ToString("N0");
                e.Row.Cells[8].Text = te_vacios.ToString("N0");
                e.Row.Cells[9].Text = tee_vacio.ToString("N0");
                e.Row.Cells[10].Text = t_expor.ToString("N0");

                e.Row.Cells[11].Text = t_uni.ToString("N0");
                e.Row.Cells[12].Text = t_Te.ToString("N0");
               
            }
        }
    }
}