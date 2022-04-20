using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using CEPA.CCO.Entidades;
using Newtonsoft.Json;
using System.Reflection;

namespace CEPA.CCO.DAL
{
    public class Cont_ExpDAL
    {
        public static List<Cont_Exp> GetContenedor(string c_contenedor)
        {
            List<Cont_Exp> _contenedores = new List<Cont_Exp>();
            string apiUrl = "http://10.1.4.20:83/api/Ejecutar/?Consulta=";
            Procedure proceso = new Procedure();
            proceso.NBase = "CONTENEDORES";
            proceso.Procedimiento = "contenedor_exp"; // "contenedor_exp"; //"Sqlentllenos"; //contenedor_exp('NYKU3806160') //"lstsalidascarga";// ('NYKU3806160')
            proceso.Parametros = new List<Parametros>();
            proceso.Parametros.Add(new Parametros { nombre = "_contenedor", valor = c_contenedor });
            string inputJson = JsonConvert.SerializeObject(proceso);
            apiUrl = apiUrl + inputJson;
            _contenedores = Conectar(_contenedores, apiUrl);
            return _contenedores;
        }

        public static List<Cont_Exp_Rev> FindContenedor(string c_contenedor, ref string msgError)
        {
            List<Cont_Exp_Rev> _lista = new List<Cont_Exp_Rev>();
            DataTable tabla = new DataTable();
            try
            {
                using (var _cone = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
                {
                    _cone.Open();
                    var _proce = DBComun.CrearProcedimiento(_cone, "pa_Consul_Conte_Exp");
                    DBComun.CreateParametro(ref _proce, "@n_contenedor", c_contenedor);
                    using (var _reader = DBComun.EjecutarSQL(_proce))
                    {
                        tabla.Load(_reader);
                        if (tabla.Rows.Count > 0)
                        {
                            _lista = tabla.ToList<Cont_Exp_Rev>();
                        }
                        else
                        {
                            msgError = "Ningun resultado";
                            _lista = null;
                        }

                    }
                    _cone.Close();
                }
            }
            catch (Exception ex)
            {
                msgError = ex.ToString();
                _lista = null;
            }
            return _lista;
        }


        private static List<Cont_Exp> Conectar(List<Cont_Exp> _contenedores, string apiUrl)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json; charset=utf-8";
            string file = string.Empty;
            var response = (HttpWebResponse)httpWebRequest.GetResponse();
            //string idx = "{ "DBase":"CONTENEDORES","Servidor":null,"Procedimiento":"Sqlentllenos","Consulta":true,"Parametros":[{"nombre":"_dia","valor":"15-05-2019"}]}";
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                file = sr.ReadToEnd();
                DataTable tabla = JsonConvert.DeserializeObject<DataTable>(file) as DataTable;
                if (tabla.Rows.Count > 0)
                {
                    if (!tabla.Rows[0][0].ToString().StartsWith("ERROR"))
                    {
                        _contenedores = tabla.ToList<Cont_Exp>();
                    }
                }
            }
            return _contenedores;
        }

        //public static List<T> ConvertToList<T>(DataTable dt)
        //{
        //    var columnNames = dt.Columns.Cast<DataColumn>()
        //     .Select(c => c.ColumnName)
        //     .ToList();
        //    var properties = typeof(T).GetProperties();
        //    return dt.AsEnumerable().Select(row =>
        //    {
        //        var objT = Activator.CreateInstance<T>();
        //        foreach (var pro in properties)
        //        {
        //            if (columnNames.Contains(pro.Name))
        //                pro.SetValue(objT, row[pro.Name]);
        //        }
        //        return objT;
        //    }).ToList();
        //}

        public static string Rev_Cont_Exp(int iddeta, string justifica)
        {
            using (var _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                var _procedure = DBComun.CrearProcedimiento(_conn, "pa_Rev_Cancel_Exp");
                DBComun.CreateParametro(ref _procedure, "@iddeta", iddeta);
                DBComun.CreateParametro(ref _procedure, "@justifica", justifica);
                int _reader = -1;
                try
                {
                    _reader = DBComun.EjecutarAct(_procedure);
                    return "OK";
                }
                catch (Exception ex)
                {
                    return "NO";
                }
            }
        }
    }
}
