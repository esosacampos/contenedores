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
using System.Web.Services;


namespace CEPA.CCO.UI.Web.DAN
{
    public partial class wfTipoRevision : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Cargar();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
        }
       


        [WebMethod]
        public static List<ProcesoRelacionado> GetEstados()
        {
            var query = from item in MenuDAL.ObtenerEstados()                        
                        select new ProcesoRelacionado
                        {                          
                            IdProceso = item.IdProceso,
                            Descripcion = item.Descripcion
                        };

            return query.ToList<ProcesoRelacionado>();
        }

        [WebMethod]
        public static List<TipoRevision> GetRevision(int Id)
        {
            var query = from item in TipoSolicitudDAL.ObtenerRevision(Id)
                        select new TipoRevision
                        {
                            Clave = item.Clave,
                            Tipo = item.Tipo,
                            Habilitado = item.Habilitado
                        };

            return query.ToList<TipoRevision>();
                                  
        }

        [WebMethod]
        public static string SaveRevision(string pClave, string pTipo)
        {
            TipoRevision _tipoRevision = new TipoRevision
            {
                Clave = pClave,
                Tipo = pTipo
            };

            string resul = TipoSolicitudDAL.InsertarUsuario(_tipoRevision);

            return "Registrado Correctamente";

        }


        private void Cargar()
        {
            GridView2.DataSource = TipoSolicitudDAL.ObtenerRevisionTo();            
            GridView2.DataBind();

            GridView2.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

            // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
              GridView2.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
              GridView2.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";             

            //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

            GridView2.HeaderRow.TableSection = TableRowSection.TableHeader;

            GridView2.FooterRow.Cells[0].Attributes["text-align"] = "center";
            GridView2.FooterRow.TableSection = TableRowSection.TableFooter;
            //  ViewState["EmployeeList"] = GridView1.DataSource;
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton button = (LinkButton)e.Row.FindControl("lkButton");
                string desc = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "IdRevision"));

                button.OnClientClick = string.Format("Javascript:CargarModi({0})", desc);

            }
        }
    }
}