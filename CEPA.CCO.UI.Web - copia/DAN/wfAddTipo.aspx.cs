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
using System.Text;
using System.DirectoryServices;
using System.Web.Configuration;

namespace CEPA.CCO.UI.Web.DAN
{
    public partial class wfAddTipo : System.Web.UI.Page
    {
        public static int bEsta;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["even"] != null)
                    {
                        CargarEstado();

                        if (Request.QueryString["even"].ToString() == "insertar")
                        {
                            DropDownList1.Enabled = false;
                        }
                        else
                        {
                            List<TipoRevision> jLista = TipoSolicitudDAL.ObtenerRevision(Convert.ToInt32(Request.QueryString["cod"]));

                            if (jLista.Count > 0)
                            {
                                foreach (TipoRevision item in jLista)
                                {
                                    //DropDownList2.SelectedValue = pUsuarios.Where(a => a.c_id_usuario == item.IdUser).ToString();
                                    txtClave.Text = item.Clave;
                                    txtTipo.Text = item.Tipo;                                  

                                    if (item.Habilitado == "Activo")
                                    {
                                        DropDownList1.SelectedIndex = 1;
                                        bEsta = 1;
                                    }
                                    else
                                    {
                                        DropDownList1.SelectedIndex = 2;
                                        bEsta = 0;
                                    }                                    

                                }
                            }                          
                        }

                        
                    }

                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script LANGUAGE='javascript'>alert('CEPA-Contenedores Error: {0}');</script>", ex.Message));
            }
        }

        private void CargarEstado()
        {
            DropDownList1.DataSource = MenuDAL.ObtenerEstados();
            DropDownList1.DataTextField = "Descripcion";
            DropDownList1.DataValueField = "IdProceso";
            DropDownList1.DataBind();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList1.SelectedIndex == 1)
                bEsta = 1;
            else if (DropDownList1.SelectedIndex == 2)
                bEsta = 0;
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                List<PerfilUsuario> pUsers = new List<PerfilUsuario>();
                if (Request.QueryString["even"].ToString() == "insertar")
                {
                    if (txtClave.Text != string.Empty && txtTipo.Text != string.Empty)
                    {

                        Almacenar(true);
                    }
                }
                else
                {                  

                        Almacenar(false);
                        //MenuDAL.ActualizarUsuario(bEsta, txtUser.Text);
                    
                }

                StringBuilder sb = new StringBuilder("<script language='javascript' type='text/javascript'>");
                sb.Append("CerrarConEvento();");
                sb.Append("</script>");

                ClientScript.RegisterStartupScript(typeof(Page), "Cerrar", sb.ToString());
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script LANGUAGE='javascript'>alert('CEPA-Contenedores Error: {0}');</script>", ex.Message));
            }
        }

        private void Almacenar(bool pAlmacenar)
        {       

            if (pAlmacenar == true)
            {
               
                    TipoRevision _tipoRevision = new TipoRevision
                    {
                        Clave = txtClave.Text,
                        Tipo = txtTipo.Text 
                    };

                    string resul = TipoSolicitudDAL.InsertarUsuario(_tipoRevision);                
            }
            else
            {
                TipoRevision _tipoRevision = new TipoRevision
                {
                    IdRevision = Convert.ToInt32(Request.QueryString["cod"]),
                    Clave = txtClave.Text,
                    Tipo = txtTipo.Text,
                    Hab = bEsta
                };

                string resul = TipoSolicitudDAL.Actualizar(_tipoRevision);                
            }
        }

     
    }
}