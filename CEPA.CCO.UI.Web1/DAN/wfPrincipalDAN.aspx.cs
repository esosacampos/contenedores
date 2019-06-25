﻿using System;
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

namespace CEPA.CCO.UI.Web.DAN
{
    public partial class wfPrincipalDAN : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    
                    Cargar();
                    
                }
                catch (Exception Ex)
                {
                    Response.Write("<script>alert('" + Ex.Message + "');</script>");
                }
            }
        }

        private void Cargar()
        {
            EncaBuqueBL _encaBL = new EncaBuqueBL();

            GridView1.DataSource = DocBuqueLINQ.ObtenerDAN();
            GridView1.DataBind();
        }

        public void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Cargar();               
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
                HyperLink hl = (HyperLink)(e.Row.FindControl("Link1"));
                hl.Attributes.Add("onclick", "Javascript:abrirModal('wfAddFolio.aspx?IdReg=" + e.Row.Cells[0].Text + "'," + e.Row.Cells[0].Text + ")");
                
            }
        }
    }
}