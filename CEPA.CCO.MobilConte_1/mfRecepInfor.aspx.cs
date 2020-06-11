using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;

namespace CEPA.CCO.MobilConte
{
    public partial class mfRecepInfor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

                var query = (from a in ValidaTarjaDAL.getDataRecepcion()
                             join b in EncaBuqueDAL.showShipping(DBComun.Estado.verdadero) on a.c_llegada equals b.c_llegada
                             select new InfOperaciones
                             {                                 
                                 n_contenedor = a.n_contenedor,
                                 f_recepcion = a.f_recepcion,
                                 c_marcacion = a.c_marcacion
                             }).ToList();

                GridView1.DataSource = query;
                GridView1.DataBind();

                GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

                // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";

                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

                GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
                GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
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