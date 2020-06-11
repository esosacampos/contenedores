using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CEPA.CCO.Linq
{
    public class DocBuqueLINQ
    {
        public static List<DocBuque> ObtenerBuqueDoc(string c_cliente)
        {
            try
            {
                List<DocBuque> lista = new List<DocBuque>();

                EncaBuqueBL _encaBL = new EncaBuqueBL();

                List<DetaDoc> pDocumentos = new List<DetaDoc>();

                pDocumentos = DetaDocDAL.ObtenerDoc(DBComun.Estado.verdadero, c_cliente);

                var query = (from a in _encaBL.ObtenerBuque(DBComun.Estado.verdadero, c_cliente)
                             select new DocBuque
                             {
                                 c_llegada = a.c_llegada,
                                 c_buque = a.c_buque,
                                 c_imo = a.c_imo,
                                 c_cliente = a.c_cliente,
                                 d_buque = a.d_buque,
                                 d_cliente = a.d_cliente,
                                 f_llegada = a.f_llegada,
                                 CantArchivo = (
                                 from b in pDocumentos
                                 where (a.c_llegada == b.c_llegada && a.c_cliente == b.c_naviera && a.c_imo == b.c_imo) && (b.IdDoc != null) && (b.IdTipoMov == 1)
                                 select b
                                 ).Count(),
                                 CantRemo = (
                                 from b in pDocumentos
                                 where (a.c_llegada == b.c_llegada && a.c_cliente == b.c_naviera && a.c_imo == b.c_imo) && (b.IdDoc != null) && (b.IdTipoMov == 2)
                                 select b
                                 ).Count()
                             }).ToList();



                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<DocBuque> ObtenerBuqueDocEx(string c_cliente)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            #region "codigo_p"
            /* var query = (from a in _encaBL.ObtenerBuque(DBComun.Estado.verdadero, c_cliente)
                          join b in DetaDocDAL.ObtenerDoc(DBComun.Estado.verdadero, c_cliente) on new { c_cliente = a.c_cliente, c_llegada = a.c_llegada } equals new { c_cliente = b.c_naviera, c_llegada = b.c_llegada } into joinBuques
                          from b in joinBuques.DefaultIfEmpty()
                          group new { a, b } by new
                          {
                              c_buque = a.c_buque,
                              c_cliente = a.c_cliente,
                              c_llegada = a.c_llegada,
                              c_imo = a.c_imo,
                              d_buque = a.d_buque,
                              d_cliente = a.d_cliente,
                              f_llegada = a.f_llegada
                          } into g
                          select new DocBuque
                          {
                              c_llegada = g.Key.c_llegada,
                              c_buque = g.Key.c_buque,
                              c_imo = g.Key.c_imo,
                              c_cliente = g.Key.c_cliente,
                              d_buque = g.Key.d_buque,
                              d_cliente = g.Key.d_cliente,
                              f_llegada = g.Key.f_llegada,
                              CantArchivo = g.Count(p => ((p.b != null) && (p.b.IdDoc != null)))

                          }).ToList();*/

            /*var query = (from a in _encaBL.ObtenerBuque(DBComun.Estado.verdadero, c_cliente)
                         join b in DetaDocDAL.ObtenerDoc(DBComun.Estado.verdadero, c_cliente) on new { c_cliente = a.c_cliente, c_llegada = a.c_llegada } equals new { c_cliente = b.c_naviera, c_llegada = b.c_llegada }                                                
                         select new DocBuque
                         {
                             c_llegada = b.c_llegada,
                             c_buque = a.c_buque,
                             c_imo = b.c_imo,
                             c_cliente = a.c_cliente,
                             d_buque = a.d_buque,
                             d_cliente = a.d_cliente,
                             f_llegada = a.f_llegada,
                             CantArchivo = b.CantArch
                         }).ToList();*/
            #endregion

            var query = (from a in _encaBL.ObtenerBuque(DBComun.Estado.verdadero, c_cliente)
                         select new DocBuque
                         {
                             c_llegada = a.c_llegada,
                             c_buque = a.c_buque,
                             c_imo = a.c_imo,
                             c_cliente = a.c_cliente,
                             d_buque = a.d_buque,
                             d_cliente = a.d_cliente,
                             f_llegada = a.f_llegada,
                             CantExport = (from b in DetaDocDAL.ObtenerDocEx(DBComun.Estado.verdadero, c_cliente)
                                           where (a.c_llegada == b.c_llegada && a.c_cliente == b.c_naviera && a.c_imo == b.c_imo) && (b.IdDoc != null)
                                           select b).Count()
                         }).ToList();



            return query;
        }

        public static List<DocBuque> ObtenerAduana(DBComun.Estado pTipo)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCabecera(pTipo)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(pTipo) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         select new DocBuque
                         {
                             IdReg = a.IdReg,
                             d_cliente = b.d_cliente,
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             CantArchivo = a.CantArch,
                             f_llegada = a.f_llegada,
                             num_manif = a.num_manif,
                             c_cliente = a.c_naviera,
                             a_manifiesto = a.a_manifiesto,
                             IdDoc = a.IdDoc,
                             b_sidunea = a.b_sidunea
                         }).OrderBy(g => g.c_cliente).ToList();


            return query;
        }

        public static List<DocBuque> ObtenerAduanaEx(DBComun.Estado pTipo)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCabeceraEx(pTipo)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(pTipo) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         select new DocBuque
                         {
                             IdReg = a.IdReg,
                             d_cliente = b.d_cliente,
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             CantArchivo = a.CantArch,
                             f_llegada = a.f_llegada,
                             c_cliente = a.c_naviera,
                             IdDoc = a.IdDoc
                         }).OrderBy(g => g.c_cliente).ToList();


            return query;
        }

        public static List<DocBuque> ObtenerCancel(DBComun.Estado pTipo, string c_naviera)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCancelados(pTipo)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(pTipo) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada, c_imo = a.c_imo } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada, c_imo = b.c_imo }
                         where a.c_naviera == c_naviera
                         select new DocBuque
                         {
                             IdReg = a.IdReg,
                             d_cliente = b.d_cliente,
                             c_imo = b.c_imo,
                             c_llegada = b.c_llegada,
                             d_buque = b.d_buque,
                             CantArchivo = a.CantArch,
                             f_llegada = b.f_llegada,
                             c_cliente = b.c_cliente,
                             c_voyage = a.c_voyage
                         }).OrderBy(g => g.c_cliente).ToList();


            return query;
        }

        public static List<DocBuque> ObtenerCambios(DBComun.Estado pTipo, string c_naviera)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCambiosRD(pTipo)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(pTipo) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada, c_imo = a.c_imo } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada, c_imo = b.c_imo }
                         where a.c_naviera == c_naviera
                         select new DocBuque
                         {
                             IdReg = a.IdReg,
                             d_cliente = b.d_cliente,
                             c_imo = b.c_imo,
                             c_llegada = b.c_llegada,
                             d_buque = b.d_buque,
                             CantArchivo = a.CantArch,
                             f_llegada = b.f_llegada,
                             c_cliente = b.c_cliente,
                             c_voyage = a.c_voyage
                         }).OrderBy(g => g.c_cliente).ToList();


            return query;
        }

        public static List<DocBuque> ObtenerAduanaId(int pId)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCabecera(DBComun.Estado.verdadero)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         where a.IdReg == pId
                         select new DocBuque
                         {
                             IdReg = a.IdReg,
                             d_cliente = b.d_cliente,
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             CantArchivo = a.CantArch,
                             f_llegada = a.f_llegada,
                             num_manif = a.num_manif,
                             c_cliente = a.c_naviera,
                             IdDoc = a.IdDoc,
                             a_manifiesto = a.a_manifiesto
                         }).OrderBy(g => g.c_cliente).ToList();


            return query;
        }

        public static List<DocBuque> ObtenerAduanaIdEx(int pId)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCabeceraEx(DBComun.Estado.verdadero)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         where a.IdReg == pId
                         select new DocBuque
                         {
                             IdReg = a.IdReg,
                             d_cliente = b.d_cliente,
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             CantArchivo = a.CantArch,
                             f_llegada = a.f_llegada,
                             num_manif = a.num_manif,
                             c_cliente = a.c_naviera,
                             IdDoc = a.IdDoc,
                             a_manifiesto = a.a_manifiesto,
                             c_voyage = a.c_voyage
                         }).OrderBy(g => g.c_cliente).ToList();


            return query;
        }

        public static List<DocBuque> ObtenerCancelados(string c_naviera)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCabeceraCancel(DBComun.Estado.verdadero)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         where a.c_naviera == c_naviera
                         select new DocBuque
                         {
                             IdReg = a.IdReg,
                             d_cliente = b.d_cliente,
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             CantArchivo = a.CantArch,
                             f_llegada = a.f_llegada
                         }).OrderBy(g => g.c_cliente).ToList();


            return query;
        }

        public static List<DocBuque> ObtenerAduanaIdCancel(int pId, string c_naviera)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCabeceraCancel(DBComun.Estado.verdadero)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         where a.IdReg == pId
                         select new DocBuque
                         {
                             IdReg = a.IdReg,
                             d_cliente = b.d_cliente,
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             CantArchivo = a.CantArch,
                             f_llegada = b.f_llegada,
                             c_voyage = a.c_voyage,
                             c_prefijo = a.d_naviera_p
                         }).OrderBy(g => g.c_cliente).ToList();


            return query;
        }

        public static List<DocBuque> ObtenerDAN()
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCabeceraDAN(DBComun.Estado.verdadero)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         select new DocBuque
                         {
                             IdReg = a.IdReg,
                             d_cliente = b.d_cliente,
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             CantArchivo = a.CantArch,
                             f_llegada = a.f_llegada
                         }).OrderByDescending(g => g.f_llegada).ToList();


            return query;
        }

        public static List<DocBuque> ObtenerDAN_Trans()
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCabeceraTrans(DBComun.Estado.verdadero)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_imo = a.c_imo, c_llegada = a.c_llegada } equals new { c_imo = b.c_imo, c_llegada = b.c_llegada }
                         select new DocBuque
                         {

                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             f_llegada = a.f_llegada
                         }).OrderByDescending(g => g.f_llegada).ToList();




            var query_modi = (from t in query
                              group t by new { c_imo = t.c_imo, c_llegada = t.c_llegada, d_buque = t.d_buque } into g
                              select new DocBuque
                              {
                                  c_imo = g.Key.c_imo,
                                  c_llegada = g.Key.c_llegada,
                                  d_buque = g.Key.d_buque,
                                  f_llegada = g.Max(y => y.f_llegada)
                              }).ToList();


            return query_modi;
        }


        public static List<DocBuque> ObtenerTrans_Auto()
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCabeceraTransAuto(DBComun.Estado.verdadero)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_imo = a.c_imo, c_llegada = a.c_llegada } equals new { c_imo = b.c_imo, c_llegada = b.c_llegada }
                         select new DocBuque
                         {

                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             f_llegada = a.f_llegada
                         }).OrderByDescending(g => g.f_llegada).ToList();




            var query_modi = (from t in query
                              group t by new { c_imo = t.c_imo, c_llegada = t.c_llegada, d_buque = t.d_buque } into g
                              select new DocBuque
                              {
                                  c_imo = g.Key.c_imo,
                                  c_llegada = g.Key.c_llegada,
                                  d_buque = g.Key.d_buque,
                                  f_llegada = g.Max(y => y.f_llegada)
                              }).ToList();


            return query_modi;
        }

        public static List<DocBuque> getCotecnaHeader()
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCabeceraTransSw(DBComun.Estado.verdadero)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_imo = a.c_imo, c_llegada = a.c_llegada } equals new { c_imo = b.c_imo, c_llegada = b.c_llegada }
                         select new DocBuque
                         {

                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             f_llegada = a.f_llegada
                         }).OrderByDescending(g => g.f_llegada).ToList();




            var query_modi = (from t in query
                              group t by new { c_imo = t.c_imo, c_llegada = t.c_llegada, d_buque = t.d_buque } into g
                              select new DocBuque
                              {
                                  c_imo = g.Key.c_imo,
                                  c_llegada = g.Key.c_llegada,
                                  d_buque = g.Key.d_buque,
                                  f_llegada = g.Max(y => y.f_llegada)
                              }).ToList();


            return query_modi;
        }

        public static List<DocBuque> ObtenerDANL()
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCabeceraDANL(DBComun.Estado.verdadero)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_llegada = a.c_llegada, c_imo = a.c_imo } equals new { c_llegada = b.c_llegada, c_imo = b.c_imo }
                         select new DocBuque
                         {
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             f_llegada = a.f_llegada
                         }).OrderByDescending(g => g.f_llegada).ToList();


            var query_modi = (from t in query
                              group t by new { c_imo = t.c_imo, c_llegada = t.c_llegada, d_buque = t.d_buque } into g
                              select new DocBuque
                              {
                                  c_imo = g.Key.c_imo,
                                  c_llegada = g.Key.c_llegada,
                                  d_buque = g.Key.d_buque,
                                  f_llegada = g.Max(y => y.f_llegada)
                              }).ToList();


            return query_modi;
        }

        public static List<DocBuque> ObtenerDANId(int pId)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCabeceraDANId(DBComun.Estado.verdadero)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         where a.IdReg == pId
                         select new DocBuque
                         {
                             IdReg = a.IdReg,
                             d_cliente = b.d_cliente,
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             CantArchivo = a.CantArch,
                             f_llegada = a.f_llegada,
                             c_voyage = a.c_voyage,
                             num_manif = a.num_manif,
                             c_cliente = a.c_naviera,
                             a_manifiesto = a.a_manifiesto
                         }).OrderBy(g => g.c_cliente).ToList();


            return query;
        }


        public static List<DocBuque> ObtenerDANId(string c_llegada)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCabeceraDANId(DBComun.Estado.verdadero, c_llegada)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_llegada = a.c_llegada, c_imo = a.c_imo } equals new { c_llegada = b.c_llegada, c_imo = b.c_imo }
                         select new DocBuque
                         {

                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             f_llegada = a.f_llegada
                         }).OrderBy(g => g.f_llegada).Take(1).ToList();


            return query;
        }

        public static List<DocBuque> ObtenerDANIdR(int pId)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCabeceraDANIdR(DBComun.Estado.verdadero, pId)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         select new DocBuque
                         {
                             IdReg = a.IdReg,
                             d_cliente = b.d_cliente,
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             CantArchivo = a.CantArch,
                             f_llegada = a.f_llegada,
                             c_voyage = a.c_voyage,
                             num_manif = a.num_manif,
                             c_cliente = a.c_naviera,
                             a_manifiesto = a.a_manifiesto
                         }).OrderBy(g => g.c_cliente).ToList();


            return query;
        }

        public static List<DocBuque> ObtEncDGA(string c_llegada, string c_naviera)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero)
                         where b.c_llegada == c_llegada && b.c_cliente == c_naviera
                         select new DocBuque
                         {                             
                             d_cliente = b.d_cliente,                           
                             d_buque = b.d_buque                            
                         }).OrderBy(g => g.c_cliente).ToList();


            return query;
        }

        public static List<DocBuque> ObtenerEncaOpera(string c_operadora)
        {

            var query = (from a in OperacionesDAL.ObtenerOperaciones(DBComun.Estado.verdadero, c_operadora)
                         join b in DetaDocDAL.ObtenerDocO(DBComun.Estado.verdadero) on a.c_llegada equals b.c_llegada
                         select new DocBuque
                         {
                             c_imo = b.c_imo,
                             c_llegada = b.c_llegada,
                             d_buque = a.d_buque,
                             f_llegada = a.f_arribo
                         }).OrderBy(g => g.f_llegada).ToList();

            return query;
        }

        public static List<DocBuque> ObtenerEncaOpera(DBComun.Estado pEstado)
        {

            var query = (from a in OperacionesDAL.ObtenerOperaciones(pEstado)
                         join b in DetaDocDAL.ObtenerDocO(pEstado) on a.c_llegada equals b.c_llegada
                         select new DocBuque
                         {
                             IdReg = b.IdReg,
                             c_imo = b.c_imo,
                             c_llegada = b.c_llegada,
                             d_buque = a.d_buque,
                             f_llegada = a.f_arribo,
                             c_voyage = b.c_voyage,
                             c_cliente = b.c_naviera
                         }).OrderBy(g => g.f_llegada).ToList();

            return query;
        }

        public static List<Naviera> ObtenerClientesValidos()
        {


            var query = (from a in EncaBuqueDAL.ObtenerNavieras1(DBComun.Estado.verdadero)
                         join b in EncaNavieraDAL.ObtenerValidas() on a.c_cliente equals b.c_naviera
                         select new Naviera
                         {
                             c_cliente = a.c_cliente,
                             d_nombre = a.d_nombre
                         }).OrderBy(g => g.c_cliente).ToList();


            return query;
        }

        public static List<DetaDAN> ObtenerOficioReport(string n_oficio, int a_folio)
        {

            var query = (from a in DetaNavieraDAL.ObtenerOficio(n_oficio, a_folio)
                         join b in EncaBuqueDAL.ObtenerBuquesJoinOf(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         select new DetaDAN
                         {
                             b_escaner = a.b_escaner,
                             d_cliente = b.d_cliente,
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             n_oficio = a.n_oficio,
                             c_viaje = a.c_viaje,
                             n_contenedor = a.n_contenedor,
                             c_pais_origen = a.c_pais_origen,
                             c_correlativo = a.c_correlativo,
                             c_consignatario = a.c_consignatario,
                             f_dan = a.f_dan,
                             jefe_almacen = a.jefe_almacen,
                             sub_inspector = a.sub_inspector,
                             Clave = a.Clave,
                             c_prefijo = a.c_prefijo,
                             c_naviera = a.c_naviera,
                             ClaveP = a.ClaveP.TrimEnd(),
                             Total = a.Total,
                             Cantidad = a.Cantidad
                         }).OrderBy(g => g.c_correlativo).ToList();


            return query;
        }

        public static List<DetaDAN> ObtenerOficioReportUCC(string n_oficio, int a_folio)
        {

            var query = (from a in DetaNavieraDAL.ObtenerOficioUCC(n_oficio, a_folio)
                         join b in EncaBuqueDAL.ObtenerBuquesJoinOf(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         select new DetaDAN
                         {
                             b_escaner = a.b_escaner,
                             d_cliente = b.d_cliente,
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             n_oficio = a.n_oficio,
                             c_viaje = a.c_viaje,
                             n_contenedor = a.n_contenedor,
                             c_pais_origen = a.c_pais_origen,
                             c_correlativo = a.c_correlativo,
                             c_consignatario = a.c_consignatario,
                             f_dan = a.f_dan,
                             jefe_almacen = a.jefe_almacen,
                             sub_inspector = a.sub_inspector,
                             Clave = a.Clave,
                             c_prefijo = a.c_prefijo,
                             c_naviera = a.c_naviera,
                             ClaveP = a.ClaveP.TrimEnd(),
                             Total = a.Total,
                             Cantidad = a.Cantidad
                         }).OrderBy(g => g.c_correlativo).ToList();


            return query;
        }
               

        public static List<DetaDAN> ObtenerOficioReport1(string n_oficio, int a_folio)
        {

            var query = (from a in DetaNavieraDAL.ObtenerOficio(n_oficio, a_folio)
                         join b in EncaBuqueDAL.ObtenerBuquesJoinOf(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         select new DetaDAN
                         {
                             d_cliente = b.d_cliente,
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             n_oficio = a.n_oficio,
                             c_viaje = a.c_viaje,
                             n_contenedor = a.n_contenedor,
                             c_pais_origen = a.c_pais_origen,
                             c_correlativo = a.c_correlativo,
                             c_consignatario = a.c_consignatario,
                             f_dan = a.f_dan,
                             jefe_almacen = a.jefe_almacen,
                             sub_inspector = a.sub_inspector,
                             Clave = a.Clave
                         }).OrderBy(g => g.c_correlativo).ToList();


            return query;
        }


        public static List<TrackingEnca> ObtenerTracking(string n_contenedor, string c_naviera)
        {
            string _valor = "NULO";

            List<TrackingEnca> pEncaT = new List<TrackingEnca>();
            List<EncaBuque> pBuques = new List<EncaBuque>();
            pEncaT = DetaNavieraDAL.GetAllOrdersByCustomer(n_contenedor, c_naviera, DBComun.TipoBD.SqlServer, _valor);

            var lParam = new List<string>();

            foreach (var cLlega in pEncaT)
            {
                //c_llegadas = c_llegadas + ("'" + cLlega.c_llegada + "'");
                lParam.Add(cLlega.c_llegada);
            }

            List<TrackingEnca> pEnca = new List<TrackingEnca>();

            if (lParam.Count > 0)
            {
                pBuques = EncaBuqueDAL.ObtenerBuquesJoinIN(DBComun.Estado.verdadero, "atraque", lParam);

                var query = (from a in pEncaT
                             join b in pBuques on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }                             
                             select new TrackingEnca
                             {
                                 IdDeta = a.IdDeta,
                                 n_contenedor = a.n_contenedor,
                                 d_cliente = b.d_cliente,
                                 c_llegada = a.c_llegada,
                                 d_buque = b.d_buque,
                                 f_llegada = b.f_llegada,
                                 c_tamaño = a.c_tamaño,
                                 TrackingList = a.TrackingList,
                                 c_naviera = a.c_naviera,
                                 b_estado = a.b_estado,
                                 b_trafico = a.b_trafico,
                                 n_manifiesto = a.n_manifiesto,
                                 b_cancelado = a.b_cancelado,
                                 c_tarja = (from c in EncaBuqueDAL.TarjasLlegada(a.c_llegada, a.n_contenedor, "b")
                                            where c.c_contenedor == a.n_contenedor
                                            select new
                                            {
                                               c_tarja = string.IsNullOrEmpty(c.c_tarja) ? "" : c.c_tarja 
                                            }).Max(x => x.c_tarja),
                                 b_requiere = a.b_requiere
                             }).OrderByDescending(g => g.IdDeta).ToList();

                List<Tarjas> encaTarjas = new List<Tarjas>();

                foreach (var item in query)
                {
                    encaTarjas.AddRange(EncaBuqueDAL.TarjasLlegada(item.c_llegada, item.n_contenedor));

                }

                string c_tarjas = null;
                int con_tarjas = 0;
                string s_descripcion = null;
                DateTime f_regTarja = new DateTime();
                string c_llegada = null;
                List<Tarjas> pTarjasDes = new List<Tarjas>();

                var grupoNavi = (from a in query
                                 group a by a.c_llegada into g
                                 select new
                                 {
                                     c_llegada = g.Key
                                 }).ToList();

                DateTime defaDate = new DateTime(1900, 01, 01);

                if (encaTarjas.Count > 0)
                {
                    foreach (var groupLle in grupoNavi)
                    {
                        s_descripcion = null;
                        c_tarjas = null;

                        List<Tarjas> pEncaTemp = new List<Tarjas>();

                        pEncaTemp = encaTarjas.Where(a => a.c_llegada == groupLle.c_llegada).ToList();

                        if (pEncaTemp.Count > 0)
                        {
                            foreach (var trjs in pEncaTemp)
                            {
                                List<Tarjas> detalleTarjas = new List<Tarjas>();
                                detalleTarjas = EncaBuqueDAL.TarjasDetalle(trjs.c_tarja);
                                if (detalleTarjas.Count > 0)
                                {
                                    foreach (var detTarja in detalleTarjas)
                                    {
                                        if (detTarja.s_descripcion.Length > 0)
                                        {
                                            s_descripcion = s_descripcion + (detTarja.s_descripcion + "/ ");
                                            f_regTarja = detTarja.f_tarja;
                                            c_llegada = detTarja.c_llegada;
                                        }
                                    }
                                }
                                c_tarjas = c_tarjas + (trjs.c_tarja + "/");
                                con_tarjas = con_tarjas + 1;
                            }

                            Tarjas _tarj = new Tarjas
                            {
                                c_tarja = c_tarjas.Substring(0, c_tarjas.Length - 1),
                                s_descripcion = s_descripcion.Substring(0, s_descripcion.Length - 1),
                                c_llegada = groupLle.c_llegada,
                                f_tarja = f_regTarja,
                                con_tarjas = con_tarjas
                            };

                            pTarjasDes.Add(_tarj);

                        }
                        else
                        {
                            Tarjas _tarj = new Tarjas
                            {
                                c_tarja = "/",
                                s_descripcion = "",
                                c_llegada = groupLle.c_llegada,
                                f_tarja = defaDate,
                                con_tarjas = con_tarjas
                            };

                            pTarjasDes.Add(_tarj);
                        }


                    }
                }


                

                string jConsult = Newtonsoft.Json.JsonConvert.SerializeObject(query);

                pEnca = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrackingEnca>>(jConsult);


                var result = from a in query
                             from item in a.TrackingList
                             where item.IdDeta == a.IdDeta
                             select item;

                if (pTarjasDes.Count > 0)
                {
                    var consultaDes = (from a in result
                                           //join b in pTarjasDes on a.c_llegada equals b.c_llegada
                                       select new TrackingDatails
                                       {
                                           IdDeta = a.IdDeta,
                                           n_oficio = a.n_oficio,
                                           f_rep_naviera = a.f_rep_naviera,
                                           f_aut_aduana = a.f_aut_aduana,
                                           f_recep_patio = a.f_recep_patio,
                                           f_ret_dan = a.f_ret_dan,
                                           f_tramite_dan = a.f_tramite_dan,
                                           f_liberado_dan = a.f_liberado_dan,
                                           f_salida_carga = a.f_salida_carga,
                                           f_solic_ingreso = a.f_solic_ingreso,
                                           f_auto_patio = a.f_auto_patio,
                                           f_puerta1 = a.f_puerta1,
                                           c_llegada = a.c_llegada,
                                           n_contenedor = a.n_contenedor,
                                           c_naviera = a.c_naviera,
                                           s_comentarios = a.s_comentarios,
                                           f_trans_aduana = a.f_trans_aduana,
                                           s_consignatario = a.s_consignatario,
                                           descripcion = (from c in pTarjasDes
                                                          where c.c_llegada == a.c_llegada
                                                          select new
                                                          {
                                                              s_descripcion = (c.s_descripcion == null ? string.Empty : c.s_descripcion)
                                                          }).Max(t => t.s_descripcion),
                                           f_caseta = a.f_caseta,
                                           f_marchamo_dan = a.f_marchamo_dan,
                                           f_recepA = a.f_recepA,
                                           f_cancelado = a.f_cancelado,
                                           f_cambio = a.f_cambio,
                                           f_ret_ucc = a.f_ret_ucc,
                                           f_tramite_ucc = a.f_tramite_ucc,
                                           f_liberado_ucc = a.f_liberado_ucc,
                                           f_marchamo_ucc = a.f_marchamo_ucc,
                                           f_deta_dan = a.f_deta_dan,
                                           f_deta_ucc = a.f_deta_ucc,
                                           f_retencion_dga = a.f_retencion_dga,
                                           f_lib_dga = a.f_lib_dga
                                       }).ToList();

                    var teamTarjas = from tar in pTarjasDes
                                     group tar by tar.c_llegada into tarjasGroup
                                     select new
                                     {
                                         c_llegada = tarjasGroup.Key,
                                         f_tarja = tarjasGroup.Max(x => x.f_tarja),
                                         s_descripcion = tarjasGroup.Max(x => x.s_descripcion),
                                         con_tarjas = tarjasGroup.Max(x => x.con_tarjas),
                                         c_tarjas = tarjasGroup.Max(x => x.c_tarja)
                                     };

                    //var TeamTarjas = pTarjasDes.Max();

                    //i.c_llegada == pTarjasDes.Max( x => x.c_llegada) &&
                    var quer = (from a in pEnca
                                join b in teamTarjas on a.c_llegada equals b.c_llegada
                                select new
                                {
                                    IdDeta = a.IdDeta,
                                    n_contenedor = a.n_contenedor,
                                    d_cliente = a.d_cliente,
                                    c_llegada = a.c_llegada,
                                    d_buque = a.d_buque,
                                    f_llegada = a.f_llegada,
                                    c_tamaño = a.c_tamaño,
                                    TrackingList = consultaDes.Where(i => i.IdDeta == a.IdDeta),
                                    c_naviera = a.c_naviera,
                                    b_estado = a.b_estado,
                                    b_trafico = a.b_trafico,
                                    n_manifiesto = a.n_manifiesto,
                                    c_tarja = a.c_tarja,
                                    f_tarja = b.f_tarja == null ? defaDate : b.f_tarja,
                                    v_peso = 0.00,
                                    descripcion = b.s_descripcion == null ? string.Empty : b.s_descripcion,
                                    b_cancelado = a.b_cancelado,
                                    con_tarjas = b.con_tarjas == 0 ? 0 : b.con_tarjas,
                                    c_tarjasn = b.c_tarjas == "/" ? "SIN TARJAS" : b.c_tarjas,
                                    b_requiere = a.b_requiere
                                }).OrderByDescending(g => g.IdDeta).ToList();

                    jConsult = Newtonsoft.Json.JsonConvert.SerializeObject(quer);

                }
                else
                {
                    var consultaDes = (from a in result
                                           //join b in pTarjasDes on a.c_llegada equals b.c_llegada
                                       select new TrackingDatails
                                       {
                                           IdDeta = a.IdDeta,
                                           n_oficio = a.n_oficio,
                                           f_rep_naviera = a.f_rep_naviera,
                                           f_aut_aduana = a.f_aut_aduana,
                                           f_recep_patio = a.f_recep_patio,
                                           f_ret_dan = a.f_ret_dan,
                                           f_tramite_dan = a.f_tramite_dan,
                                           f_liberado_dan = a.f_liberado_dan,
                                           f_salida_carga = a.f_salida_carga,
                                           f_solic_ingreso = a.f_solic_ingreso,
                                           f_auto_patio = a.f_auto_patio,
                                           f_puerta1 = a.f_puerta1,
                                           c_llegada = a.c_llegada,
                                           n_contenedor = a.n_contenedor,
                                           c_naviera = a.c_naviera,
                                           s_comentarios = a.s_comentarios,
                                           f_trans_aduana = a.f_trans_aduana,
                                           s_consignatario = a.s_consignatario,
                                           descripcion = a.descripcion,
                                           f_caseta = a.f_caseta,
                                           f_marchamo_dan = a.f_marchamo_dan,
                                           f_recepA = a.f_recepA,
                                           f_cancelado = a.f_cancelado,
                                           f_cambio = a.f_cambio,
                                           f_tramite_ucc = a.f_tramite_ucc,
                                           f_liberado_ucc = a.f_liberado_ucc,
                                           f_marchamo_ucc = a.f_marchamo_ucc,
                                           f_deta_dan = a.f_deta_dan,
                                           f_deta_ucc = a.f_deta_ucc,
                                           f_ret_ucc = a.f_ret_ucc,
                                           f_retencion_dga = a.f_retencion_dga,
                                           f_lib_dga = a.f_lib_dga
                                       }).ToList();



                    //i.c_llegada == pTarjasDes.Max( x => x.c_llegada) &&
                    var quer = (from a in pEnca
                                    //join b in pTarjasDes on a.c_llegada equals b.c_llegada
                                select new
                                {
                                    IdDeta = a.IdDeta,
                                    n_contenedor = a.n_contenedor,
                                    d_cliente = a.d_cliente,
                                    c_llegada = a.c_llegada,
                                    d_buque = a.d_buque,
                                    f_llegada = a.f_llegada,
                                    c_tamaño = a.c_tamaño,
                                    TrackingList = consultaDes.Where(i => i.IdDeta == a.IdDeta),
                                    c_naviera = a.c_naviera,
                                    b_estado = a.b_estado,
                                    b_trafico = a.b_trafico,
                                    n_manifiesto = a.n_manifiesto,
                                    c_tarja = a.c_tarja,
                                    f_tarja = defaDate,
                                    v_peso = 0.00,
                                    descripcion = a.descripcion,
                                    b_cancelado = a.b_cancelado,
                                    c_tarjasn = "SIN TARJAS",
                                    con_tarjas = 0,
                                    b_requiere = a.b_requiere
                                }).OrderByDescending(g => g.IdDeta).ToList();

                    jConsult = Newtonsoft.Json.JsonConvert.SerializeObject(quer);
                }

                pEnca = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrackingEnca>>(jConsult);
            }

            

            return pEnca;
        }

        public static List<TrackingEnca> ObtenerTracking_Cliente(string n_contenedor, string c_naviera, string n_mani, int a_dm, int s_dm, int c_dm)
        {
            string _valor = "NULO";

            List<TrackingEnca> pEncaT = new List<TrackingEnca>();
            List<EncaBuque> pBuques = new List<EncaBuque>();
            pEncaT = DetaNavieraDAL.GetAllOrdersByCustomer(n_contenedor, c_naviera, DBComun.TipoBD.SqlTracking, a_dm, s_dm, c_dm, n_mani);


            var lParam = new List<string>();

            foreach (var cLlega in pEncaT)
            {
                //c_llegadas = c_llegadas + ("'" + cLlega.c_llegada + "'");
                lParam.Add(cLlega.c_llegada);
            }

            pBuques = EncaBuqueDAL.ObtenerBuquesJoinIN(DBComun.Estado.verdadero, "atraque", lParam);

            var query = (from a in pEncaT
                         join b in pBuques on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         select new TrackingEnca
                         {
                             IdDeta = a.IdDeta,
                             n_contenedor = a.n_contenedor,
                             d_cliente = b.d_cliente,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             f_llegada = b.f_llegada,
                             c_tamaño = a.c_tamaño,
                             TrackingList = a.TrackingList,
                             c_naviera = a.c_naviera,
                             b_estado = a.b_estado,
                             b_trafico = a.b_trafico,
                             n_manifiesto = a.n_manifiesto,
                             b_cancelado = a.b_cancelado,
                             c_tarja = (from c in EncaBuqueDAL.TarjasLlegada(a.c_llegada, a.n_contenedor, "b")
                                        where c.c_contenedor == a.n_contenedor
                                        select new
                                        {
                                            c_tarja = (c.c_tarja == null ? string.Empty : c.c_tarja)
                                        }).Max(x=> x.c_tarja),
                             b_requiere = a.b_requiere
                         }).OrderByDescending(g => g.IdDeta).ToList();

            List<Tarjas> encaTarjas = new List<Tarjas>();

            foreach (var item in query)
            {
                encaTarjas.AddRange(EncaBuqueDAL.TarjasLlegada(item.c_llegada, item.n_contenedor));

            }

            string c_tarjas = null;
            int con_tarjas = 0;
            string s_descripcion = null;
            DateTime f_regTarja = new DateTime();
            string c_llegada = null;
            List<Tarjas> pTarjasDes = new List<Tarjas>();

            var grupoNavi = (from a in query
                             group a by a.c_llegada into g
                             select new
                             {
                                 c_llegada = g.Key
                             }).ToList();

            DateTime defaDate = new DateTime(1900, 01, 01);

            if (encaTarjas.Count > 0)
            {
                foreach (var groupLle in grupoNavi)
                {
                    s_descripcion = null;
                    c_tarjas = null;

                    List<Tarjas> pEncaTemp = new List<Tarjas>();

                    pEncaTemp = encaTarjas.Where(a => a.c_llegada == groupLle.c_llegada).ToList();

                    if (pEncaTemp.Count > 0)
                    {
                        foreach (var trjs in pEncaTemp)
                        {
                            List<Tarjas> detalleTarjas = new List<Tarjas>();
                            detalleTarjas = EncaBuqueDAL.TarjasDetalle(trjs.c_tarja);
                            if (detalleTarjas.Count > 0)
                            {
                                foreach (var detTarja in detalleTarjas)
                                {
                                    if (detTarja.s_descripcion.Length > 0)
                                    {
                                        s_descripcion = s_descripcion + (detTarja.s_descripcion + "/ ");
                                        f_regTarja = detTarja.f_tarja;
                                        c_llegada = detTarja.c_llegada;

                                    }


                                }
                            }

                            c_tarjas = c_tarjas + (trjs.c_tarja + "/");
                            con_tarjas = con_tarjas + 1;
                        }

                        Tarjas _tarj = new Tarjas
                        {
                            c_tarja = c_tarjas.Substring(0, c_tarjas.Length - 1),
                            s_descripcion = s_descripcion.Substring(0, s_descripcion.Length - 1),
                            c_llegada = groupLle.c_llegada,
                            f_tarja = f_regTarja,
                            con_tarjas = con_tarjas
                        };

                        pTarjasDes.Add(_tarj);

                    }
                    else
                    {
                        Tarjas _tarj = new Tarjas
                        {
                            c_tarja = "/",
                            s_descripcion = "",
                            c_llegada = groupLle.c_llegada,
                            f_tarja = defaDate,
                            con_tarjas = con_tarjas
                        };

                        pTarjasDes.Add(_tarj);
                    }


                }
            }


            List<TrackingEnca> pEnca = new List<TrackingEnca>();

            string jConsult = Newtonsoft.Json.JsonConvert.SerializeObject(query);

            pEnca = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrackingEnca>>(jConsult);


            var result = from a in query
                         from item in a.TrackingList
                         where item.IdDeta == a.IdDeta
                         select item;

            if (pTarjasDes.Count > 0)
            {
                var consultaDes = (from a in result
                                       //join b in pTarjasDes on a.c_llegada equals b.c_llegada
                                   select new TrackingDatails
                                   {
                                       IdDeta = a.IdDeta,
                                       n_oficio = a.n_oficio,
                                       f_rep_naviera = a.f_rep_naviera,
                                       f_aut_aduana = a.f_aut_aduana,
                                       f_recep_patio = a.f_recep_patio,
                                       f_ret_dan = a.f_ret_dan,
                                       f_tramite_dan = a.f_tramite_dan,
                                       f_liberado_dan = a.f_liberado_dan,
                                       f_salida_carga = a.f_salida_carga,
                                       f_solic_ingreso = a.f_solic_ingreso,
                                       f_auto_patio = a.f_auto_patio,
                                       f_puerta1 = a.f_puerta1,
                                       c_llegada = a.c_llegada,
                                       n_contenedor = a.n_contenedor,
                                       c_naviera = a.c_naviera,
                                       ubicacion = a.ubicacion,
                                       s_comentarios = a.s_comentarios,
                                       f_trans_aduana = a.f_trans_aduana,
                                       s_consignatario = a.s_consignatario,
                                       descripcion = (from c in pTarjasDes
                                                      where c.c_llegada == a.c_llegada
                                                      select new
                                                      {
                                                          s_descripcion = (c.s_descripcion == null ? string.Empty : c.s_descripcion)
                                                      }).Max(t => t.s_descripcion),
                                       f_caseta = a.f_caseta,
                                       f_marchamo_dan = a.f_marchamo_dan,
                                       f_recepA = a.f_recepA,
                                       f_cancelado = a.f_cancelado,
                                       f_cambio = a.f_cambio,
                                       f_reg_aduana = a.f_reg_aduana,
                                       f_reg_selectivo = a.f_reg_selectivo,
                                       f_lib_aduana = a.f_lib_aduana,
                                       f_ret_mag = a.f_ret_mag,
                                       f_lib_mag = a.f_lib_mag,
                                       f_ret_ucc = a.f_ret_ucc,
                                       f_tramite_ucc = a.f_tramite_ucc,
                                       f_liberado_ucc = a.f_liberado_ucc,
                                       f_marchamo_ucc = a.f_marchamo_ucc,
                                       f_deta_dan = a.f_deta_dan,
                                       f_deta_ucc = a.f_deta_ucc,
                                       f_retencion_dga = a.f_retencion_dga,
                                       f_lib_dga = a.f_lib_dga
                                   }).ToList();

                var teamTarjas = from tar in pTarjasDes
                                 group tar by tar.c_llegada into tarjasGroup
                                 select new
                                 {
                                     c_llegada = tarjasGroup.Key,
                                     f_tarja = tarjasGroup.Max(x => x.f_tarja),
                                     s_descripcion = tarjasGroup.Max(x => x.s_descripcion),
                                     con_tarjas = tarjasGroup.Max(x => x.con_tarjas),
                                     c_tarjas = tarjasGroup.Max(x => x.c_tarja)
                                 };
                             
                var quer = (from a in pEnca
                            join b in teamTarjas on a.c_llegada equals b.c_llegada
                            select new
                            {
                                IdDeta = a.IdDeta,
                                n_contenedor = a.n_contenedor,
                                d_cliente = a.d_cliente,
                                c_llegada = a.c_llegada,
                                d_buque = a.d_buque,
                                f_llegada = a.f_llegada,
                                c_tamaño = a.c_tamaño,
                                TrackingList = consultaDes.Where(i => i.IdDeta == a.IdDeta),
                                c_naviera = a.c_naviera,
                                b_estado = a.b_estado,
                                b_trafico = a.b_trafico,
                                n_manifiesto = a.n_manifiesto,
                                c_tarja = a.c_tarja,
                                f_tarja = b.f_tarja == null ? defaDate : b.f_tarja,
                                v_peso = 0.00,
                                descripcion = b.s_descripcion == null ? string.Empty : b.s_descripcion,
                                b_cancelado = a.b_cancelado,
                                con_tarjas = b.con_tarjas == 0 ? 0 : b.con_tarjas,
                                c_tarjasn = b.c_tarjas == "/" ? "SIN TARJAS" : b.c_tarjas,
                                b_requiere = a.b_requiere
                            }).OrderByDescending(g => g.IdDeta).ToList();

                jConsult = Newtonsoft.Json.JsonConvert.SerializeObject(quer);

            }
            else
            {
                var consultaDes = (from a in result
                                       //join b in pTarjasDes on a.c_llegada equals b.c_llegada
                                   select new TrackingDatails
                                   {
                                       IdDeta = a.IdDeta,
                                       n_oficio = a.n_oficio,
                                       f_rep_naviera = a.f_rep_naviera,
                                       f_aut_aduana = a.f_aut_aduana,
                                       f_recep_patio = a.f_recep_patio,
                                       f_ret_dan = a.f_ret_dan,
                                       f_tramite_dan = a.f_tramite_dan,
                                       f_liberado_dan = a.f_liberado_dan,
                                       f_salida_carga = a.f_salida_carga,
                                       f_solic_ingreso = a.f_solic_ingreso,
                                       f_auto_patio = a.f_auto_patio,
                                       f_puerta1 = a.f_puerta1,
                                       c_llegada = a.c_llegada,
                                       n_contenedor = a.n_contenedor,
                                       c_naviera = a.c_naviera,
                                       ubicacion = a.ubicacion,
                                       s_comentarios = a.s_comentarios,
                                       f_trans_aduana = a.f_trans_aduana,
                                       s_consignatario = a.s_consignatario,
                                       descripcion = a.descripcion,
                                       f_caseta = a.f_caseta,
                                       f_marchamo_dan = a.f_marchamo_dan,
                                       f_recepA = a.f_recepA,
                                       f_cancelado = a.f_cancelado,
                                       f_cambio = a.f_cambio,
                                       f_reg_aduana = a.f_reg_aduana,
                                       f_reg_selectivo = a.f_reg_selectivo,
                                       f_lib_aduana = a.f_lib_aduana,
                                       f_ret_mag = a.f_ret_mag,
                                       f_lib_mag = a.f_lib_mag,
                                       f_ret_ucc = a.f_ret_ucc,
                                       f_tramite_ucc = a.f_tramite_ucc,
                                       f_liberado_ucc = a.f_liberado_ucc,
                                       f_marchamo_ucc = a.f_marchamo_ucc,
                                       f_deta_dan = a.f_deta_dan,
                                       f_deta_ucc = a.f_deta_ucc,
                                       f_retencion_dga = a.f_retencion_dga,
                                       f_lib_dga = a.f_lib_dga
                                   }).ToList();



                //i.c_llegada == pTarjasDes.Max( x => x.c_llegada) &&
                var quer = (from a in pEnca
                                //join b in pTarjasDes on a.c_llegada equals b.c_llegada
                            select new
                            {
                                IdDeta = a.IdDeta,
                                n_contenedor = a.n_contenedor,
                                d_cliente = a.d_cliente,
                                c_llegada = a.c_llegada,
                                d_buque = a.d_buque,
                                f_llegada = a.f_llegada,
                                c_tamaño = a.c_tamaño,
                                TrackingList = consultaDes.Where(i => i.IdDeta == a.IdDeta),
                                c_naviera = a.c_naviera,
                                b_estado = a.b_estado,
                                b_trafico = a.b_trafico,
                                n_manifiesto = a.n_manifiesto,
                                c_tarja = a.c_tarja,
                                f_tarja = defaDate,
                                v_peso = 0.00,
                                descripcion = a.descripcion,
                                b_cancelado = a.b_cancelado,
                                c_tarjasn = "SIN TARJAS",
                                con_tarjas = 0
                            }).OrderByDescending(g => g.IdDeta).ToList();

                jConsult = Newtonsoft.Json.JsonConvert.SerializeObject(quer);
            }



            pEnca = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrackingEnca>>(jConsult);

            return pEnca;



        }



        public static List<DetaNaviera> ObtenerDANM(string n_contenedor, string c_naviera)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in DetaNavieraDAL.ConsultarDANM(n_contenedor, c_naviera)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_cliente = a.c_navi } equals new { c_cliente = b.c_cliente }
                         select new DetaNaviera
                         {
                             n_contenedor = a.n_contenedor,
                             c_tamaño = a.c_tamaño,
                             n_folio = a.n_folio,
                             f_recepcion = a.f_recepcion,
                             f_tramite = a.f_tramite,
                             f_liberado = a.f_liberado,
                             b_estadoV = a.b_estadoV,
                             c_correlativo = a.c_correlativo,
                             CalcDiasD = a.CalcDiasD,
                             c_navi = b.d_cliente,
                             s_comodity = a.s_comodity
                         }).ToList();


            return query;
        }

        public static List<DocBuque> ObtenerAutorizados()
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCabeceraAuto(DBComun.Estado.verdadero)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         select new DocBuque
                         {
                             IdReg = a.IdReg,
                             d_cliente = b.d_cliente,
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             CantArchivo = a.CantArch,
                             f_llegada = a.f_llegada
                         }).OrderBy(g => g.c_cliente).ToList();


            return query;
        }

        public static List<DocBuque> ObtenerTransmi(string c_llegada)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerIdTrans(DBComun.Estado.verdadero, c_llegada)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on a.c_llegada equals b.c_llegada
                         select new DocBuque
                         {
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             f_llegada = b.f_llegada,
                             TotalImp = a.Total_Imp,
                             TotalTrans = a.Total_Trans,
                             TotalTransA = a.Total_TransA,
                             TotalPTrans = a.Total_PTrans,
                             TotalPTransA = a.Total_PTransA,
                             TotalCancel = a.Total_Cancel
                         }).OrderBy(g => g.c_llegada).ToList();


            return query;
        }

        public static List<DocBuque> ObtenerTransmiAuto(string c_llegada)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerIdTransAuto(DBComun.Estado.verdadero, c_llegada)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on a.c_llegada equals b.c_llegada
                         select new DocBuque
                         {
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             f_llegada = b.f_llegada                             
                         }).OrderBy(g => g.c_llegada).ToList();


            return query;
        }

        public static List<DetaNaviera> ObtenerTransmiConsul(string c_llegada)
        {
            List<DetaNaviera> lista = new List<DetaNaviera>();

            DateTime f_tarjac = new DateTime(1900, 1, 1);

            EncaBuqueBL _encaBL = new EncaBuqueBL();

           // List<Tarjas> pTarjas = EncaBuqueDAL.TarjasLlegada(c_llegada);

            
            lista = (from a in DetaNavieraDAL.ObtenerDetaTrans(c_llegada)
            orderby a.c_cliente,
                a.c_correlativo
            select new DetaNaviera
            {
                c_correlativo = a.c_correlativo,
                n_contenedor = a.n_contenedor,
                c_tamaño = a.c_tamaño,
                b_estado = a.b_estado,
                c_tarja = (from c in EncaBuqueDAL.TarjasLlegada(c_llegada, a.n_contenedor, "b")                                        
                        select new { c_tarja = c.c_tarja == null ? "ST" : c.c_tarja }).Max(x=> x.c_tarja),
                f_recep = a.f_recep,
                b_trans = a.b_trans,
                f_trans = a.f_trans,
                f_dan = a.f_dan,
                c_cliente = a.c_cliente,
                c_manifiesto = a.c_manifiesto,
                b_recepcion_c = a.b_recepcion_c,
                b_requiere = a.b_requiere
            }).ToList();
            

            return lista;
        }

        public static List<DetaNaviera> ObtenerTransmiConsulAuto(string c_llegada)
        {
            List<DetaNaviera> lista = new List<DetaNaviera>();

         

            // List<Tarjas> pTarjas = EncaBuqueDAL.TarjasLlegada(c_llegada);


            lista = (from a in DetaNavieraDAL.ObtenerDetaTransAuto(c_llegada)
                     orderby a.c_cliente,
                         a.c_correlativo
                     select new DetaNaviera
                     {
                         IdDeta = a.IdDeta,
                         c_correlativo = a.c_correlativo,
                         n_contenedor = a.n_contenedor,
                         c_tamaño = a.c_tamaño,
                         b_estado = a.b_estado,
                         c_cliente = a.c_cliente,
                         c_manifiesto = a.c_manifiesto,
                         b_despacho = a.b_despacho,
                         b_manejo = a.b_manejo,
                         b_transferencia = a.b_transferencia,
                         s_consignatario = a.s_consignatario,
                         v_peso = a.v_peso,
                         c_llegada = a.c_llegada                      


                     }).ToList();


            return lista;
        }

        public static List<DetaNaviera> ObtenerTransmiConsulAutoSrv(string c_llegada)
        {
            List<DetaNaviera> lista = new List<DetaNaviera>();



            // List<Tarjas> pTarjas = EncaBuqueDAL.TarjasLlegada(c_llegada);


            lista = (from a in DetaNavieraDAL.ObtenerDetaTransAutoSrv(c_llegada)
                     orderby a.c_cliente,
                         a.c_correlativo
                     select new DetaNaviera
                     {
                         IdDeta = a.IdDeta,
                         c_correlativo = a.c_correlativo,
                         n_contenedor = a.n_contenedor,
                         c_tamaño = a.c_tamaño,
                         b_estado = a.b_estado,
                         c_cliente = a.c_cliente,
                         c_manifiesto = a.c_manifiesto,
                         b_despacho = a.b_despacho,
                         b_manejo = a.b_manejo,
                         b_transferencia = a.b_transferencia,
                         s_consignatario = a.s_consignatario,
                         v_peso = a.v_peso,
                         c_llegada = a.c_llegada,
                         f_recep = a.f_recep,
                         f_tramite_s = a.f_tramite_s
                     }).ToList();


            return lista;
        }

        public static List<DocBuque> ObtenerCambiosTran(DBComun.Estado pTipo, string c_naviera)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCambiosTransi(pTipo)
                         join b in EncaBuqueDAL.BuquesZarpe(pTipo) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         where a.c_naviera == c_naviera
                         select new DocBuque
                         {
                             IdReg = a.IdReg,
                             d_cliente = b.d_cliente,
                             c_imo = a.c_imo,
                             c_llegada = b.c_llegada,
                             d_buque = b.d_buque,
                             CantArchivo = a.CantArch,
                             f_llegada = b.f_atraque,
                             c_cliente = b.c_cliente,
                             c_voyage = a.c_voyage
                         }).OrderBy(g => g.c_cliente).ToList();


            return query;
        }

        public static List<DocBuque> ObtenerCambiosId(int pId, string c_naviera)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.ObtenerCambiosTransi(DBComun.Estado.verdadero)
                         join b in EncaBuqueDAL.BuquesZarpe(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         where a.IdReg == pId
                         select new DocBuque
                         {
                             IdReg = a.IdReg,
                             d_cliente = b.d_cliente,
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             CantArchivo = a.CantArch,
                             f_llegada = b.f_atraque,
                             c_voyage = a.c_voyage,
                             c_prefijo = a.d_naviera_p
                         }).OrderBy(g => g.c_cliente).ToList();


            return query;
        }

        public static List<CorteCOTECNA> ObtenerDGA()
        {
            List<CorteCOTECNA> lista = new List<CorteCOTECNA>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.obtCabeceraDGA(DBComun.Estado.verdadero)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_cliente = a.c_cliente, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         where b.f_llegada.Year >= 2017
                         select new CorteCOTECNA
                         {
                             c_llegada = a.c_llegada,
                             n_manifiesto = a.n_manifiesto,
                             d_buque = b.d_buque,
                             d_cliente = b.d_cliente + " - " + a.c_prefijo,
                             t_contenedores = a.t_contenedores,
                             f_atraque = b.f_llegada,
                             c_cliente = a.c_cliente,
                             t_dga = a.t_dga,
                             b_solidga = a.b_solidga,
                             c_voyage = a.c_voyage,
                             IdDoc = a.IdDoc
                         }).OrderByDescending(g => Convert.ToInt32(g.n_manifiesto)).ToList();


            return query;
        }

        public static List<DocBuque> detalleDGA(string n_manifiesto, string c_cliente)
        {
            List<DocBuque> lista = new List<DocBuque>();

            EncaBuqueBL _encaBL = new EncaBuqueBL();

            var query = (from a in EncaNavieraDAL.cabeDetaDGA(DBComun.Estado.verdadero, n_manifiesto, c_cliente)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         select new DocBuque
                         {
                             IdReg = a.IdReg,
                             d_cliente = b.d_cliente,
                             c_imo = a.c_imo,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             CantArchivo = a.CantArch,
                             f_llegada = a.f_llegada,
                             c_voyage = a.c_voyage,
                             num_manif = a.num_manif,
                             c_cliente = a.c_naviera,
                             a_manifiesto = a.a_manifiesto,
                             IdDoc = a.IdDoc
                         }).OrderBy(g => g.c_cliente).ToList();


            return query;
        }

    }
}
