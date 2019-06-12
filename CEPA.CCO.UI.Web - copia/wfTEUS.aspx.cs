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
    public partial class wfTEUS : System.Web.UI.Page
    {
        private int year, total_te1 = 0, total_te2 = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
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

                if (year < 2014)
                    throw new Exception("Año minimo de consulta 2014");



                GridView1.DataSource = TEUSAnaDAL.ObtenerTEUS(DBComun.Estado.verdadero, year);
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

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal te1, te2;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Text = "TEUS " + (year - 1).ToString();
                e.Row.Cells[2].Text = "% " + (year - 1).ToString();
                e.Row.Cells[3].Text = "TEUS " + year.ToString();
                e.Row.Cells[4].Text = "% " + year.ToString();
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                total_te2 += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "teu2"));
                total_te1 += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "teu1"));

                System.Web.UI.WebControls.Image imgFile = e.Row.FindControl("Image1") as System.Web.UI.WebControls.Image;

                te1 = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "t2"));
                te2 = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "t"));

                if(te2 > te1)
                    imgFile.ImageUrl = ResolveUrl("~/CSS/Img/f_verde.gif");
                else
                    imgFile.ImageUrl = ResolveUrl("~/CSS/Img/f_rojo.gif");

                //if (Session["c_naviera"].ToString() != "11" && Session["c_naviera"].ToString() != "289")
                //{
                //    string c_agencia = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "c_agencia"));

                //    if (c_agencia != Session["c_naviera"].ToString())
                //        e.Row.Visible = false;
                //}

                e.Row.Cells[6].Visible = false;                
                GridView1.HeaderRow.Cells[6].Visible = false;                         


            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTALES ....";
                e.Row.Cells[1].Text = total_te2.ToString();
                e.Row.Cells[3].Text = total_te1.ToString();
                e.Row.Cells[6].Visible = false;
            }
        }
    }
}