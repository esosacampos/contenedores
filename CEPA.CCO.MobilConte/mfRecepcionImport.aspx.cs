﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;

namespace CEPA.CCO.MobilConte
{
    public partial class mfRecepcionImport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hMarcacion.Value = Session["c_marcacion"].ToString();

        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetConte(string prefix)
        {
            List<string> customers = new List<string>();

            //customers = ValidaTarjaDAL.GetContenedor(prefix);


            var query = (from a in ValidaTarjaDAL.GetContenedor(prefix)                         
                         join b in EncaBuqueDAL.showShipping(DBComun.Estado.verdadero) on a.c_llegada equals b.c_llegada
                         select new InfOperaciones
                         {
                            n_contenedor = a.n_contenedor,
                            IdDeta = a.IdDeta
                         }).ToList();

          

            foreach (var item in query)
	        {
		        customers.Add(string.Format("{0}-{1}", item.n_contenedor, item.IdDeta));
	        }
            

            return customers.ToArray();
        }
        
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string GetConteInfo(string n_contenedor)
        {
            var query = (from a in ValidaTarjaDAL.GetContenedorInfor(n_contenedor)
                         join b in EncaBuqueDAL.ObtenerBuquesJoinC(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         select new InfOperaciones
                         {
                             c_tamaño = a.c_tamaño,
                             v_tara = a.v_tara,
                             c_trafico = a.c_trafico,
                             c_llegada = a.c_llegada,
                             c_estado = a.c_estado,
                             c_buque = b.d_buque,
                             c_cliente = a.c_cliente,
                             IdDeta = a.IdDeta,
                             b_detenido = a.b_detenido,
                             b_style = a.b_style,
                             b_aduana = a.b_aduana,
                             b_staduana = a.b_staduana
                         }).ToList();



            return Newtonsoft.Json.JsonConvert.SerializeObject(query);
        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string SaveConfir(int IdDeta,string s_observaciones, string c_marcacion, string b_directo)
        {


            string _valor = null;
            string _resultado = ValidaTarjaDAL.SaveConfirmacion(DBComun.Estado.verdadero, IdDeta, s_observaciones, c_marcacion, b_directo);

            if(Convert.ToInt32(_resultado) > 0)
                _valor = "Registro de recepcion exitoso";
            else
                _valor = "Verificar información o reportar problemas en confirmación";


            return Newtonsoft.Json.JsonConvert.SerializeObject(_valor);
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string showSummary()
        {
            var query = (from a in ValidaTarjaDAL.shoSummaryBuque()
                         join b in EncaBuqueDAL.showShipping(DBComun.Estado.verdadero) on a.c_llegada equals b.c_llegada
                         select new InfOperaciones
                         {                            
                             c_llegada = a.c_llegada,                             
                             c_buque = b.d_buque,
                             Total = a.Total,
                             OIRSA = a.OIRSA,
                             PO = a.PO,
                             PATIO = a.PATIO,
                             PP = a.PP
                         }).ToList();



            return Newtonsoft.Json.JsonConvert.SerializeObject(query);
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string getDataRecep()
        {
            var query = (from a in ValidaTarjaDAL.getDataRecepcion()                         
                         select new InfOperaciones
                         {
                            n_contenedor = a.n_contenedor,
                            f_recepcion = a.f_recepcion,
                            c_marcacion = a.c_marcacion
                         }).ToList();



            return Newtonsoft.Json.JsonConvert.SerializeObject(query);
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