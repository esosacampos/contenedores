using CEPA.CCO.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

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
                                     select Convert.ToInt32(grid.DataKeys[item.RowIndex].Value)).ToList();


            HttpContext.Current.Session["ConSelection"] = checkedProd;


        }

        public static void RestoreSelection(GridView grid)
        {
            List<DetaNaviera> _listEmp = new List<DetaNaviera>();

            List<int> contenedoresSel = HttpContext.Current.Session["ConSelection"] as List<int>;

            if (contenedoresSel == null)
            {
                return;
            }

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
                                               let retenido = Convert.ToString(item.Cells[3].Text ?? string.Empty)
                                               let correlativo = (HiddenField)item.FindControl("hCorre")
                                               let c_size = Convert.ToString(item.Cells[2].Text ?? string.Empty)
                                               where check.Checked
                                               select new DetaNaviera
                                               {
                                                   IdDeta = Convert.ToInt32(grid.DataKeys[item.RowIndex].Value),
                                                   n_contenedor = contenedor,
                                                   s_observaciones = observacion.Text,
                                                   b_retenido = retenido,
                                                   c_tamaño = c_size,
                                                   c_correlativo = Convert.ToInt32(correlativo.Value)
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
                                               let nBL = Convert.ToString(item.Cells[10].Text ?? string.Empty)
                                               let s_consignatario = Convert.ToString(item.Cells[11].Text ?? string.Empty)
                                               let size = Convert.ToString(item.Cells[3].Text ?? string.Empty)
                                               let btd = Convert.ToString(item.Cells[4].Text ?? string.Empty)
                                               where check.Checked
                                               select new DetaNaviera
                                               {
                                                   IdDeta = Convert.ToInt32(grid.DataKeys[item.RowIndex].Value),
                                                   n_contenedor = contenedor,
                                                   ClaveRe = ClaveRe.SelectedValue,
                                                   b_escaner = check1.Checked == true ? 1 : 0,
                                                   s_consignatario = s_consignatario,
                                                   n_BL = nBL,
                                                   c_tamaño = size,
                                                   b_rdt = btd
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
                                               let Id = (HiddenField)item.FindControl("hId")
                                               let oficio = Convert.ToString(item.Cells[0].Text ?? string.Empty)
                                               let c_cliente = Convert.ToString(item.Cells[1].Text ?? string.Empty)
                                               let c_llegada = (HiddenField)item.FindControl("hLlegada")
                                               let d_buque = Convert.ToString(item.Cells[2].Text ?? string.Empty)
                                               let contenedor = Convert.ToString(item.Cells[3].Text ?? string.Empty)             
                                               let f_revision = (TextBox)item.FindControl("txtDOB1")
                                               let t_revision = (HiddenField)item.FindControl("hRevision")
                                               let t_detalle = (HiddenField)item.FindControl("hDetalle")
                                               let size = Convert.ToString(item.Cells[4].Text ?? string.Empty)
                                               let f_tramite_s = Convert.ToString(item.Cells[6].Text ?? string.Empty)
                                               where check.Checked
                                               select new DetaNaviera
                                               {
                                                   IdDeta = Convert.ToInt32(Id.Value),
                                                   n_contenedor = contenedor,
                                                   n_folio = oficio,
                                                   c_tamaño = size,
                                                   c_cliente = c_cliente,
                                                   c_llegada = Convert.ToString(c_llegada.Value),
                                                   d_buque = d_buque,
                                                   c_manifiesto = grid.DataKeys[item.RowIndex].Values[4].ToString(),
                                                   f_retenido = Convert.ToDateTime(grid.DataKeys[item.RowIndex].Values[0].ToString()),
                                                   f_recep_patio = Convert.ToDateTime(grid.DataKeys[item.RowIndex].Values[1].ToString()),
                                                   f_dan = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                                                   //f_tramite = Convert.ToDateTime(f_tramite.Text),
                                                   f_revision = Convert.ToDateTime(f_revision.Text),
                                                   t_revision = Convert.ToInt32(t_revision.Value),
                                                   CalcDias = SqlMethods.DateDiffHour(Convert.ToDateTime(f_revision.Text), DateTime.Now),
                                                   c_navi = grid.DataKeys[item.RowIndex].Values[2].ToString(),
                                                   c_tipo_doc = grid.DataKeys[item.RowIndex].Values[3].ToString(),
                                                   t_detalle = Convert.ToInt32(t_detalle.Value),
                                                   f_tramite_s = f_tramite_s
                                               }).ToList();

            HttpContext.Current.Session["Liberados"] = uncheckedProd;

        }

        public static void AutoExport(GridView grid)
        {

            List<DetaNaviera> _listaContenedores = new List<DetaNaviera>();
            //
            // se obtienen los id de producto checkeados de la pagina actual
            //
            List<DetaNaviera> uncheckedProd = (from item in grid.Rows.Cast<GridViewRow>()
                                               let check = (CheckBox)item.FindControl("CheckBox1")
                                               let contenedor = Convert.ToString(item.Cells[0].Text ?? string.Empty)
                                               let iddoc = (HiddenField)item.FindControl("Hidden2")
                                               let idreg = (HiddenField)item.FindControl("Hidden1")
                                               let iddeta = (HiddenField)item.FindControl("Hidden3")
                                               let v_peso = Convert.ToDouble(item.Cells[2].Text ?? string.Empty)
                                               let c_tamaño = Convert.ToString(item.Cells[1].Text ?? string.Empty)
                                               let c_pais_destino = Convert.ToString(item.Cells[3].Text ?? string.Empty)
                                               let c_detalle_pais = Convert.ToString(item.Cells[4].Text ?? string.Empty)
                                               where check.Checked
                                               select new DetaNaviera
                                               {
                                                   IdDeta = Convert.ToInt32(iddeta.Value),
                                                   n_contenedor = contenedor,
                                                   IdDoc = Convert.ToInt32(iddoc.Value),
                                                   IdReg = Convert.ToInt32(idreg.Value),
                                                   v_peso = v_peso,
                                                   c_tamaño = c_tamaño,
                                                   c_pais_destino = c_pais_destino,
                                                   c_detalle_pais = c_detalle_pais
                                               }).ToList();



            HttpContext.Current.Session["Exportados"] = uncheckedProd;

        }

        public static void CambiosID(GridView grid)
        {

            List<DetaNaviera> _listaContenedores = new List<DetaNaviera>();
            //
            // se obtienen los id de producto checkeados de la pagina actual
            //
            List<DetaNaviera> uncheckedProd = (from item in grid.Rows.Cast<GridViewRow>()
                                               let check = (CheckBox)item.FindControl("CheckBox1")
                                               let contenedor = Convert.ToString(item.Cells[1].Text ?? string.Empty)
                                               let observacion = (TextBox)item.FindControl("txtObservaciones")
                                               let idDeta = (HiddenField)item.FindControl("hIdDeta")
                                               let c_size = Convert.ToString(item.Cells[2].Text ?? string.Empty)
                                               let c_estado = Convert.ToString(item.Cells[4].Text ?? string.Empty)
                                               let retenido = Convert.ToString(item.Cells[3].Text ?? string.Empty)
                                               let checkREF = (CheckBox)item.FindControl("CheckBox2")
                                               where check.Checked
                                               select new DetaNaviera
                                               {
                                                   IdDeta = Convert.ToInt32(idDeta.Value),
                                                   n_contenedor = contenedor,
                                                   s_observaciones = observacion.Text,
                                                   b_retenido = retenido,
                                                   c_tamaño = c_size,
                                                   c_correlativo = Convert.ToInt32(grid.DataKeys[item.RowIndex].Value),
                                                   b_estado = c_estado,
                                                   b_reef = checkREF.Checked == true ? "Y" : "N"
                                               }).OrderByDescending(a => a.s_observaciones).ToList();



            HttpContext.Current.Session["Cambios"] = uncheckedProd;

        }


        public static void Solicitados(GridView grid)
        {

            List<DetaNaviera> _listaContenedores = new List<DetaNaviera>();
            //
            // se obtienen los id de producto checkeados de la pagina actual
            //
            List<DetaNaviera> uncheckedProd = (from item in grid.Rows.Cast<GridViewRow>()
                                               let check = (CheckBox)item.FindControl("CheckBox1")
                                               let contenedor = Convert.ToString(item.Cells[0].Text ?? string.Empty)
                                               let nBL = Convert.ToString(item.Cells[7].Text ?? string.Empty)
                                               let s_consignatario = Convert.ToString(item.Cells[8].Text ?? string.Empty)
                                               let size = Convert.ToString(item.Cells[1].Text ?? string.Empty)
                                               let b_rtd = Convert.ToString(item.Cells[2].Text ?? string.Empty)
                                               where check.Checked
                                               select new DetaNaviera
                                               {
                                                   IdDeta = Convert.ToInt32(grid.DataKeys[item.RowIndex].Value),
                                                   n_contenedor = contenedor,
                                                   s_consignatario = s_consignatario,
                                                   n_BL = nBL,
                                                   c_tamaño = size,
                                                   b_rdt = b_rtd
                                               }).ToList();



            HttpContext.Current.Session["Solicitados"] = uncheckedProd;

        }

    }

}