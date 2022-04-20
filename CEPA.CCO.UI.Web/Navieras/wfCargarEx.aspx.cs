using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using CEPA.CCO.Linq;

namespace CEPA.CCO.UI.Web.Navieras
{
    public partial class wfCargarEx : System.Web.UI.Page
    {
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Navieras/wfPrinciExport.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["c_buque"] == null || Request.QueryString["c_llegada"] == null || Request.QueryString["c_cliente"] == null)
                    {
                        throw new Exception("Falta código de buque");
                    }
                    else
                    {
                        EncaBuqueBL _encaBL = new EncaBuqueBL();
                        List<EncaBuque> _encaList = new List<EncaBuque>();
                        _encaList = _encaBL.ObtenerBuqueID(DBComun.Estado.verdadero, Request.QueryString["c_cliente"].ToString(), Request.QueryString["c_buque"].ToString(), Request.QueryString["c_llegada"].ToString());

                        if (_encaList.Count > 0)
                        {
                            foreach (EncaBuque item in _encaList)
                            {
                                foreach (var itemNav in EncaNavieraDAL.GetNavieras(DBComun.Estado.verdadero).Where(a => a.c_naviera == Request.QueryString["c_cliente"].ToString()))
                                {
                                    d_agencia.Text = itemNav.d_naviera_p;
                                    hIsoNavi.Value = itemNav.c_iso_navi;
                                    break;
                                }                                                    
                                c_imo.Text = item.c_imo;
                                d_buque_c.Text = item.d_buque;
                                f_llegada.Text = item.f_llegada.ToString("dd/MM/yyyy hh:mm tt");
                                c_llegada.Text = item.c_llegada;
                                hNaviera.Value = Request.QueryString["c_cliente"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
        }
    }
}