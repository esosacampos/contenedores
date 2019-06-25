﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;
using CEPA.CCO.BL;

namespace CEPA.CCO.UI.Web.Navieras
{
    public partial class wfExportacion : System.Web.UI.Page
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
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
                }
            }
        }
        private void Cargar()
        {
            EncaBuqueBL _encaBL = new EncaBuqueBL();
            GridView1.DataSource = DocBuqueLINQ.ObtenerBuqueDocEx(Session["c_naviera"].ToString());
            GridView1.DataBind();

            GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

            GridView1.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";


            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
            GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Cargar();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            List<DocBuque> _lista = DocBuqueLINQ.ObtenerBuqueDocEx(Session["c_naviera"].ToString());
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // int CantArch;

            string c_llegada;
            string c_imo;
         

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //HyperLink link = e.Row.FindControl("Link") as HyperLink;
                //HyperLink link1 = e.Row.FindControl("Link1") as HyperLink;

                c_imo = e.Row.Cells[0].Text;
                c_llegada = e.Row.Cells[1].Text;

                //b_noti = Convert.ToInt32(EncaNavieraDAL.ObtenerNotiS(c_imo, c_llegada, Session["c_naviera"].ToString()));


                // CantArch = Convert.ToInt32(e.Row.Cells[4].Text);

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