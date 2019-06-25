﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;

namespace CEPA.CCO.UI.Web.Consulta
{
   

    public partial class wfConsultaDAN : System.Web.UI.Page
    {
        private static readonly DateTime FIRST_GOOD_DATE = new DateTime(1900, 01, 01);

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            List<DetaNaviera> pLista = new List<DetaNaviera>();
            List<DetaNaviera> pLista1 = new List<DetaNaviera>();
            try
            {
                if (txtBuscar.Text.Length == 11)
                {
                    pLista = DocBuqueLINQ.ObtenerDANM(txtBuscar.Text.ToUpper(), Session["c_naviera"].ToString());

                    if (pLista.Count > 0)
                    {
                        if (pLista.Count == 1)
                        {
                            foreach (var item in pLista)
                            {
                                if (item.b_estadoV == "1")
                                {
                                    DetailsView1.DataSource = pLista;
                                    DetailsView1.DataBind();
                                }
                                else
                                {
                                    DetailsView1.DataSource = null;
                                    DetailsView1.DataBind();
                                    Label lblEmptyMessage = DetailsView1.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                                    lblEmptyMessage.Text = "Este contenedor no se encuentra retenido: " + txtBuscar.Text.ToUpper();
                                }
                            }
                        }
                        else if (pLista.Count > 1)
                        {
                            for (int i = 0; i < pLista.Count; i++)
                            {
                                if (pLista[i].b_estadoV == "1")
                                {
                                    pLista1.Add(pLista[i]);
                                }
                            }

                            if (pLista1.Count > 0)
                            {
                                DetailsView1.DataSource = pLista1;
                                DetailsView1.DataBind();
                            }
                            else
                            {
                                DetailsView1.DataSource = null;
                                DetailsView1.DataBind();
                                Label lblEmptyMessage = DetailsView1.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                                lblEmptyMessage.Text = "Este contenedor no se encuentra retenido: " + txtBuscar.Text.ToUpper();
                            }
                        }
                    }
                    else
                    {
                        DetailsView1.DataSource = null;
                        DetailsView1.DataBind();
                        Label lblEmptyMessage = DetailsView1.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                        lblEmptyMessage.Text = "No se poseen registros de este contenedor: " + txtBuscar.Text.ToUpper();
                    }


                }
                else
                {
                    DetailsView1.DataSource = null;
                    DetailsView1.DataBind();
                    Label lblEmptyMessage = DetailsView1.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                    lblEmptyMessage.Text = "Este número no posee los 11 caracteres " + txtBuscar.Text.ToUpper();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime f_liberados;
                DateTime f_tramite;
                f_liberados = Convert.ToDateTime(e.Row.Cells[5].Text);
                f_tramite = Convert.ToDateTime(e.Row.Cells[4].Text);

                if (f_liberados > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[5].Text = "";
                }

                if (f_tramite > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[4].Text = "";
                }

            }
        }

        protected void DetailsView1_DataBound(object sender, EventArgs e)
        {

            if (DetailsView1.DataSource != null)
            {
                if (Convert.ToDateTime(DetailsView1.Rows[6].Cells[1].Text) > FIRST_GOOD_DATE)
                { }
                else
                {
                    DetailsView1.Rows[6].Cells[1].Text = "";
                }

                if (Convert.ToDateTime(DetailsView1.Rows[7].Cells[1].Text) > FIRST_GOOD_DATE)
                { }
                else
                {
                    DetailsView1.Rows[7].Cells[1].Text = "";
                }
            }
        }
    }
}