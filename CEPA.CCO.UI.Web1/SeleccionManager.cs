using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using CEPA.CCO.Linq;
using System.Globalization;
using System.Data.Linq.SqlClient;

namespace CEPA.CCO.UI.Web
{
    public static class SeleccionManager
    {
        public static void KeepSelection(GridView grid)
        {

            List<DetaNaviera> _listaContenedores = new List<DetaNaviera>();
            //
            // se obtienen los id de producto checkeados de la pagina actual
            //
            List<int> checkedProd = (from item in grid.Rows.Cast<GridViewRow>()
                                        let check = (CheckBox)item.FindControl("CheckBox1")
                                        where check.Checked
                                        select  Convert.ToInt32(grid.DataKeys[item.RowIndex].Value)).ToList();
          
        
            HttpContext.Current.Session["ConSelection"] = checkedProd;


        }

        public static void RestoreSelection(GridView grid)
        {
            List<DetaNaviera> _listEmp = new List<DetaNaviera>();

            List<int> contenedoresSel = HttpContext.Current.Session["ConSelection"] as List<int>;

            if (contenedoresSel == null)
                return;

            //
            // se comparan los registros de la pagina del grid con los recuperados de la Session
            // los coincidentes se devuelven para ser seleccionados
            //
            List<GridViewRow> result = (from item in grid.Rows.Cast<GridViewRow>()
                                        join item2 in contenedoresSel
                                        on Convert.ToInt32(grid.DataKeys[item.RowIndex].Value) equals item2 into g
                                        where g.Any()
                                        select item).ToList();

            //
            // se recorre cada item para marcarlo
            //
            result.ForEach(x => ((CheckBox)x.FindControl("CheckBox1")).Checked = true);

            //EmpleadoBL _empBL = new EmpleadoBL();
            //_listEmp = _empBL.ObtenerEmpleados();

            //HttpContext.Current.Session["ListaEmpleados"] = _listEmp;

        }

        public static void Cancelados(GridView grid)
        {

            List<DetaNaviera> _listaContenedores = new List<DetaNaviera>();
            //
            // se obtienen los id de producto checkeados de la pagina actual
            //
            List<DetaNaviera> uncheckedProd = (from item in grid.Rows.Cast<GridViewRow>()
                                             let check = (CheckBox)item.FindControl("CheckBox1")
                                             let contenedor = Convert.ToString(item.Cells[1].Text ?? string.Empty)
                                             where !check.Checked
                                             select new DetaNaviera
                                             {
                                                 IdDeta = Convert.ToInt32(grid.DataKeys[item.RowIndex].Value),
                                                 n_contenedor = contenedor
                                             }).ToList();



            HttpContext.Current.Session["Cancelados"] = uncheckedProd;

        }

        public static void Seleccion(GridView grid)
        {

            List<DetaNaviera> _listaContenedores = new List<DetaNaviera>();
            //
            // se obtienen los id de producto checkeados de la pagina actual
            //
            List<EncaNaviera> checkedProd = (from item in grid.Rows.Cast<GridViewRow>()
                                             let check = (CheckBox)item.FindControl("CheckBox1")
                                             let manifiesto = Convert.ToString(item.Cells[1].Text ?? string.Empty)
                                             let imo = Convert.ToString(item.Cells[3].Text ?? string.Empty)
                                             let llegada = Convert.ToString(item.Cells[4].Text ?? string.Empty)
                                             let buque = Convert.ToString(item.Cells[5].Text ?? string.Empty)
                                             let fllegada = Convert.ToString(item.Cells[6].Text)                                            
                                             where check.Checked
                                             select new EncaNaviera
                                             {
                                                 IdReg = Convert.ToInt32(grid.DataKeys[item.RowIndex].Value),
                                                 c_imo = imo,
                                                 c_llegada = llegada,
                                                 f_llegada = Convert.ToDateTime(fllegada, CultureInfo.CreateSpecificCulture("es-SV")),
                                                 d_buque = buque,
                                                 num_manif = Convert.ToInt32(manifiesto) 
                                             }).ToList();


            HttpContext.Current.Session["SeleTodos"] = checkedProd;


        }

        public static void CanceladosID(GridView grid)
        {

            List<DetaNaviera> _listaContenedores = new List<DetaNaviera>();
            //
            // se obtienen los id de producto checkeados de la pagina actual
            //
            List<DetaNaviera> uncheckedProd = (from item in grid.Rows.Cast<GridViewRow>()
                                               let check = (CheckBox)item.FindControl("CheckBox1")
                                               let contenedor = Convert.ToString(item.Cells[1].Text ?? string.Empty)                                              
                                               let observacion = (TextBox)item.FindControl("txtObservaciones")
                                               where check.Checked
                                               select new DetaNaviera
                                               {
                                                   IdDeta = Convert.ToInt32(grid.DataKeys[item.RowIndex].Value),
                                                   n_contenedor = contenedor,
                                                   s_observaciones = observacion.Text
                                               }).ToList();



            HttpContext.Current.Session["Cancelados"] = uncheckedProd;

        }

        public static void Detenidos(GridView grid)
        {

            List<DetaNaviera> _listaContenedores = new List<DetaNaviera>();
            //
            // se obtienen los id de producto checkeados de la pagina actual
            //
            List<DetaNaviera> uncheckedProd = (from item in grid.Rows.Cast<GridViewRow>()
                                               let check = (CheckBox)item.FindControl("CheckBox1")
                                               let check1 = (CheckBox)item.FindControl("CheckBox2")
                                               let contenedor = Convert.ToString(item.Cells[2].Text ?? string.Empty)
                                               let ClaveRe = (DropDownList)item.FindControl("ddlRevision")
                                               let nBL = Convert.ToString(item.Cells[9].Text ?? string.Empty)
                                               let s_consignatario = Convert.ToString(item.Cells[10].Text ?? string.Empty)
                                               let size = Convert.ToString(item.Cells[3].Text ?? string.Empty)
                                               where check.Checked
                                               select new DetaNaviera
                                               {
                                                   IdDeta = Convert.ToInt32(grid.DataKeys[item.RowIndex].Value),
                                                   n_contenedor = contenedor,
                                                   ClaveRe = ClaveRe.SelectedValue,
                                                   b_escaner = check1.Checked == true ? 1 : 0,
                                                   s_consignatario = s_consignatario,
                                                   n_BL = nBL,
                                                   c_tamaño = size
                                               }).ToList();



            HttpContext.Current.Session["Detenidos"] = uncheckedProd;

        }

        public static void Liberados(GridView grid)
        {

            List<DetaNaviera> _listaContenedores = new List<DetaNaviera>();
            //
            // se obtienen los id de producto checkeados de la pagina actual
            //

            


            List<DetaNaviera> uncheckedProd = (from item in grid.Rows.Cast<GridViewRow>()
                                               let check = (CheckBox)item.FindControl("CheckBox1")
                                               let Id = Convert.ToInt32(item.Cells[0].Text ?? string.Empty)
                                               let oficio = Convert.ToString(item.Cells[1].Text ?? string.Empty)
                                               let contenedor = Convert.ToString(item.Cells[2].Text ?? string.Empty)
                                               let f_tramite = (TextBox)item.FindControl("txtDOB")
                                               let size = Convert.ToString(item.Cells[3].Text ?? string.Empty)                                              
                                               where check.Checked 
                                               select new DetaNaviera
                                               {
                                                   IdDeta = Id,
                                                   n_contenedor = contenedor,
                                                   n_folio = oficio,
                                                   c_tamaño = size,
                                                   f_recepcion = Convert.ToDateTime(grid.DataKeys[item.RowIndex].Value),
                                                   f_dan = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                                                   f_liberado = Convert.ToDateTime(f_tramite.Text),
                                                   CalcDias = SqlMethods.DateDiffHour(Convert.ToDateTime(f_tramite.Text), DateTime.Now)
                                               }).ToList();
            
            HttpContext.Current.Session["Liberados"] = uncheckedProd;

        }

    }
        
}