using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace CEPA.CCO.UI.Web
{
    public partial class wfLstValidFact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

            }
        }
        public static List<ValiadaTarja> getValidaciones(string f_corta)
        {
            List<ValiadaTarja> _contenedores = new List<ValiadaTarja>();
            string apiUrl = WebConfigurationManager.AppSettings["apiFox"].ToString();
            Procedure proceso = new Procedure
            {
                NBase = "CONTENEDORES",
                Procedimiento = "ListaValTarja", // "contenedor_exp"; //"Sqlentllenos"; //contenedor_exp('NYKU3806160') //"lstsalidascarga";// ('NYKU3806160')
                Parametros = new List<Parametros>()
            };
            proceso.Parametros.Add(new Parametros { nombre = "f_corta", valor = f_corta });
            
            string inputJson = JsonConvert.SerializeObject(proceso);
            apiUrl = apiUrl + inputJson;
            _contenedores = Conectar(_contenedores, apiUrl);
            return _contenedores;
        }

        private static List<ValiadaTarja> Conectar(List<ValiadaTarja> _contenedores, string apiUrl)
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
                        _contenedores = tabla.ToList<ValiadaTarja>();
                    }
                }
            }
            return _contenedores;
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string f_corta = txtDOB.Text.Replace("/", "-");

            if (string.IsNullOrEmpty(f_corta))
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Debe seleccionar una fecha a consultar');", true);
                return;
            }
            try
            {
                List<ValiadaTarja> pList = new List<ValiadaTarja>();
                pList = getValidaciones(f_corta);
                grvValidaciones.DataSource = pList;
                grvValidaciones.DataBind();

                if (grvValidaciones.Rows.Count > 0)
                {
                    TableCellCollection cells = grvValidaciones.HeaderRow.Cells;
                    cells[0].Attributes.Add("data-class", "expand");
                    cells[2].Attributes.Add("data-hide", "phone");
                    cells[3].Attributes.Add("data-hide", "phone");
                    cells[4].Attributes.Add("data-hide", "phone,tablet");


                    grvValidaciones.HeaderRow.TableSection = TableRowSection.TableHeader;

                    grvValidaciones.FooterRow.Cells[0].Attributes["text-align"] = "center";
                    grvValidaciones.FooterRow.TableSection = TableRowSection.TableFooter;
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('La fecha seleccionada no devolvió resultados, internar con una nueva fecha');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
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

                e.Row.Cells[0].Controls.Add(new LiteralControl("<ul class='pagination pagination-centered hide-if-no-paging'></ul><div class='divider'  style='margin-bottom: 15px;'></div></div><span class='label label-default pie' style='background-color: #dff0d8;border-radius: 25px;font-family: sans-serif;font-size: 18px;color: #468847;border-color: #d6e9c6;margin-top: 18px;'></span>"));
            }
        }

        protected void grvValidaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (grvValidaciones.HeaderRow.Cells[i].Text == "JUSTIFICACI&#211;N")
                    {
                        e.Row.Cells[i].Text = e.Row.Cells[i].Text.ToUpper();
                    }

                    if (grvValidaciones.HeaderRow.Cells[i].Text == "USUARIO")
                    {
                        e.Row.Cells[i].Text = e.Row.Cells[i].Text.Substring(0, e.Row.Cells[i].Text.Length - 6);
                    }
                }

            }
        }
    }
}