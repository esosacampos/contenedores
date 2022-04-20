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
using System.Web.Services;

namespace CEPA.CCO.UI.Web
{
    public partial class wfPrinciExport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Cargar();
                   
                    if (GridView1.Rows.Count > 0)
                    {
                        btnCargar.Visible = false;
                    }
                    //btnCargar.Visible = true;

                    else
                    {
                        btnCargar.Visible = false;
                        throw new Exception("No hay listados pendientes de autorización");
                    }
                }

                if (GridView1.Rows.Count > 0)
                {
                    GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";
                    GridView1.HeaderRow.Cells[0].Attributes["id"] = "num_manif";

                    // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                    GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
                    GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
                    GridView1.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
                    GridView1.HeaderRow.Cells[6].Attributes["data-hide"] = "phone";
                
                    //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

                    GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
        }

        private void Cargar()
        {
            EncaBuqueBL _encaBL = new EncaBuqueBL();

            GridView1.DataSource = DocBuqueLINQ.ObtenerAduanaExAuto(DBComun.Estado.verdadero);
            GridView1.DataBind();

           

        }
    }
}