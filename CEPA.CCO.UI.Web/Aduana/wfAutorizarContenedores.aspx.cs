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
using System.Globalization;

namespace CEPA.CCO.UI.Web.Aduana
{
    public partial class wfAutorizarContenedores : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Request.QueryString["IdReg"] == null || Request.QueryString["n_manif"] == null)
                    {
                        throw new Exception("Falta código de cabecera");
                    }
                    else
                    {
                        List<DocBuque> _encaList = DocBuqueLINQ.ObtenerAduanaId(Convert.ToInt32(Request.QueryString["IdReg"]));
                        //Sacar naviera
                        if (_encaList.Count > 0)
                        {
                            foreach (DocBuque item in _encaList)
                            {
                                c_imo.Text = item.c_imo;
                                d_buque.Text = item.d_buque;
                                f_llegada.Text = item.f_llegada.ToString("dd/MM/yyyy HH:mm:ss");
                                c_llegada.Text = item.c_llegada;
                                n_manifiesto.Text = Request.QueryString["n_manif"].ToString();
                            }

                            Cargar();

                            
                        }
                        else
                        {
                            btnCargar.Enabled = false;
                        }

                    }
                    
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        private void Cargar()
        {
            GridView1.DataSource = DetaNavieraDAL.ObtenerDetalle(Convert.ToInt32(Request.QueryString["IdReg"])).OrderBy(y => y.n_contenedor).ToList();
            GridView1.DataBind();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfConsultaBuques.aspx");
        }        

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
            try
            {
                string[] pAutorizado = null;
                int valores = 0;
                int cantidad = GridView1.Rows.Count;
                int Correlativo = 0;
                string naviera = null;
                NotiAutorizados _notiAutorizados = new NotiAutorizados();

                SeleccionManager.KeepSelection((GridView)GridView1);
                SeleccionManager.Cancelados((GridView)GridView1);


                List<int> _lit = HttpContext.Current.Session["ConSelection"] as List<int>;
                List<DetaNaviera> _litCancel = HttpContext.Current.Session["Cancelados"] as List<DetaNaviera>;

                if (_lit == null)
                    _lit = new List<int>();

                if (_lit.Count > 0)
                {
                    foreach (int item in _lit)
                    {
                        int _resultado = Convert.ToInt32(DetaNavieraDAL.ActualizarDetaId(DBComun.Estado.verdadero, item));

                        if (_resultado > 0)
                        {
                            valores = valores + 1;
                        }
                    }


                    if (valores > 0)
                    {
                        int _cancelados = Convert.ToInt32(DetaNavieraDAL.ActualizarCancelar(DBComun.Estado.verdadero, Convert.ToInt32(Request.QueryString["IdReg"])));

                        string c_naviera = EncaNavieraDAL.ObtenerNaviera(DBComun.Estado.verdadero, Convert.ToInt32(Request.QueryString["IdReg"]));

                        List<EncaBuque> d_naviera = EncaBuqueDAL.ObtenerNaviera(DBComun.Estado.verdadero, c_naviera);
                        foreach (EncaBuque item5 in d_naviera)
                        {
                            naviera = item5.d_cliente;
                        }

                        List<DetaNaviera> _listAutorizados = DetaNavieraDAL.ObtenerAutorizados(Convert.ToInt32(Request.QueryString["IdReg"])).OrderBy(p => p.n_contenedor).ToList();
                        Correlativo = Convert.ToInt32(DetaNavieraDAL.ObtenerCorrelativo(c_imo.Text, c_llegada.Text, c_naviera, DBComun.Estado.verdadero)) + 1;



                        List<DetaNaviera> _list = new List<DetaNaviera>();

                        for (int i = 1; i < 7; i++)
                        {

                            var consulta = DetaNavieraLINQ.AlmacenarArchivo(_listAutorizados, DBComun.Estado.verdadero, i);

                            if (consulta.Count > 0)
                                _list.AddRange(consulta);

                            foreach (var item_C in consulta)
                            {
                                _listAutorizados.RemoveAll(rt => rt.n_contenedor.ToUpper() == item_C.n_contenedor.ToUpper());
                            }

                            
                        }
                        string arrLista;
                        if (_listAutorizados.Count > 0)
                        {
                            
                            for (int l = 0; l < _listAutorizados.Count; l++)
                            {
                                pAutorizado[l] = _listAutorizados[l].n_contenedor;
                            }

                            arrLista = string.Join(", \n", pAutorizado);
                            throw new Exception("Los siguientes contenedores no entran en ninguna clasificacion de ordenamiento de contenedores : \n" + arrLista);
                        }

                        if (_list.Count > 0)
                        {
                            foreach (DetaNaviera deta in _list)
                            {
                                int _actCorre = Convert.ToInt32(DetaNavieraDAL.ActualizarCorrelativo(DBComun.Estado.verdadero, Correlativo, deta.IdDeta));
                                Correlativo = Correlativo + 1;
                            }

                            // Envio de correos con archivos adjuntos.
                        }

                        int _rango = ((Correlativo - 1) - valores) + 1;

                        List<ArchivoAduana> _listaAOrdenar = DetaNavieraDAL.ObtenerResultado(Convert.ToInt32(Request.QueryString["IdReg"]), _rango, Correlativo - 1, DBComun.Estado.verdadero);

                        HttpContext.Current.Session["IdDoc"] = DetaDocDAL.ObtenerIdDoc(Convert.ToInt32(Request.QueryString["n_manif"]));
                        
                        _cargar.Imprimir();
                        Response.Write(string.Format("<script>alert('Cantidad de contenedores autorizados {0} de {1}');</script>", valores, cantidad));
                        _notiAutorizados.GenerarExcelDAN(_listaAOrdenar, c_naviera, naviera, c_llegada.Text, Convert.ToInt32(Request.QueryString["IdReg"]),
                           d_buque.Text, Convert.ToDateTime(f_llegada.Text, CultureInfo.CreateSpecificCulture("es-SV")), valores, cantidad, _litCancel);
                        Thread.Sleep(500);
                        _notiAutorizados.GenerarAplicacion(_listaAOrdenar, c_naviera, naviera, c_llegada.Text, Convert.ToInt32(Request.QueryString["IdReg"]),
                            d_buque.Text, Convert.ToDateTime(f_llegada.Text, CultureInfo.CreateSpecificCulture("es-SV")), valores, cantidad, _litCancel);
                        Thread.Sleep(500);
                        _cargar.Clear("wfConsultaBuques.aspx");
                    }
                }
                else
                {
                    _cargar.Imprimir();
                    Response.Write(string.Format("<script>alert('No existen contenedores pendientes de autorización');</script>", valores, cantidad));
                    Thread.Sleep(4000);
                   _cargar.Clear("wfConsultaBuques.aspx");
                }
            }    
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
            finally
            {
                HttpContext.Current.Session["ConSelection"] = null;
                HttpContext.Current.Session["Cancelados"] = null;
            }
        }
    }
}