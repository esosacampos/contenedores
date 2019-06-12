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

namespace CEPA.CCO.UI.Web.Aduana
{
    public partial class wfConsultaBuques : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.Params["__EVENTTARGET"] == "ActManifiesto")
            {
                string mani = Page.Request.Params["__EVENTARGUMENT"].ToString();
                this.Cargar();
            }      
       
            if (!IsPostBack)
            {
                try
                {
                    Cargar();
                    if (GridView1.Rows.Count <= 0)                    
                        btnCargar.Visible = false;
                    
                    else
                        btnCargar.Visible = true;
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
                }
            }

            if (GridView1.Rows.Count > 0)
            {
                GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

                // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[6].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[7].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[9].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[10].Attributes["data-hide"] = "phone";
                //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        
            
            //  ViewState["EmployeeList"] = GridView1.DataSource;
        }

        private void Cargar()
        {
            EncaBuqueBL _encaBL = new EncaBuqueBL();

            GridView1.DataSource = DocBuqueLINQ.ObtenerAduana(DBComun.Estado.verdadero);
            GridView1.DataBind();

            if (GridView1.Rows.Count > 0)
            {
                GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";
                GridView1.HeaderRow.Cells[0].Attributes["id"] = "num_manif";

                // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[6].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[7].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[9].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[10].Attributes["data-hide"] = "phone";
                //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ////int CantArch = 0;

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
               

            //}
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            
            int cantidad = GridView1.Rows.Count;
            int cont = 0;
            
            NotiAutorizados _notiAutorizados = new NotiAutorizados();
            CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
            

            try
            {             
                //GenerarAutorizacion(ref valores, cantidad, ref Correlativo, ref naviera, ref cont, _notiAutorizados);                        
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Cantidad de manifiestos autorizados " + cont + " de "+ cantidad +"');", true);                
            }
            catch (Exception ex)
            {
                //_cargar.Imprimir();
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);                
            }
            finally
            {
                HttpContext.Current.Session["SeleTodos"] = null;
            }
        }

        private void GenerarAutorizacion()
        {

            //GenerarListado();
        }

        private static string GenerarListado(int IdReg, int cant)
        {
            int valores = 0;
            int cantidad = cant;
            int Correlativo = 0;
            string naviera = null;
          
            int cont = 0;
            
            NotiAutorizados _notiAutorizados = new NotiAutorizados();
            CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
      

            //SeleccionManager.Seleccion((GridView)GridView1);
            //List<EncaNaviera> _lit = HttpContext.Current.Session["SeleTodos"] as List<EncaNaviera>;

            //Manifiesto

            List<DocBuque> _lit1 = DocBuqueLINQ.ObtenerAduanaId(IdReg);

            List<EncaNaviera> _lit = new List<EncaNaviera>();


            if (_lit1.Count > 0)
            {
                foreach (var itemCo in _lit1)
                {
                    EncaNaviera _encaLit = new EncaNaviera
                    {
                        IdReg = (int)itemCo.IdReg,                        
                        c_imo = itemCo.c_imo,
                        c_llegada = itemCo.c_llegada,
                        d_buque = itemCo.d_buque,                       
                        f_llegada = itemCo.f_llegada,
                        num_manif = itemCo.num_manif,
                        IdDoc = itemCo.IdDoc,
                        c_naviera = itemCo.c_cliente
                    };

                    _lit.Add(_encaLit);
                }
            }




            int IdDocW = 0;
            


            if (_lit == null)
                _lit = new List<EncaNaviera>();

            
            if (_lit.Count > 0)
            {

                foreach (EncaNaviera item in _lit)
                {

                    int validR = Convert.ToInt32(DetaDocDAL.VerificarValid(item.num_manif, IdReg, DBComun.Estado.verdadero, item.IdDoc));

                    if (validR == 0 || validR == 2)
                    {
                        throw new Exception("Debe verificar la validación del manifiesto electrónico");
                    }
                    else
                    {
                        List<DetaNaviera> _lista = DetaNavieraDAL.ObtenerDetalle(item.IdReg, item.num_manif);

                        foreach (var ir in _lista)
                        {
                            IdDocW = ir.IdDoc;

                            break;
                        }

                        HttpContext.Current.Session["IdDoc"] = IdDocW;

                        int _resultado = Convert.ToInt32(DetaNavieraDAL.ActualizarDeta(DBComun.Estado.verdadero, item.IdReg, IdDocW));

                        if (_resultado > 0)
                        {
                            valores = _resultado;
                            cont = cont + 1;
                        }

                        if (valores > 0)
                        {

                            string c_naviera = EncaNavieraDAL.ObtenerNaviera(DBComun.Estado.verdadero, item.IdReg);

                            List<EncaBuque> d_naviera = EncaBuqueDAL.ObtenerNaviera(DBComun.Estado.verdadero, c_naviera);
                            foreach (EncaBuque item5 in d_naviera)
                            {
                                naviera = item5.d_cliente;
                                break;
                            }


                            List<DetaNaviera> _listAutorizados = DetaNavieraDAL.ObtenerAutorizados(item.IdReg);
                            Correlativo = Convert.ToInt32(DetaNavieraDAL.ObtenerCorrelativo(item.c_imo, item.c_llegada, c_naviera, DBComun.Estado.verdadero)) + 1;

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
                            List<ArchivoAduana> _listaAOrdenar = DetaNavieraDAL.ObtenerResultado(item.IdReg, _rango, Correlativo - 1, DBComun.Estado.verdadero);


                            //    _notiAutorizados.GenerarExcelDAN(_listaAOrdenar, c_naviera, naviera, item.c_llegada, item.IdReg,
                            //    item.d_buque, item.f_llegada, valores, valores, null);




                            //foreach (var itemDS in _listaAOrdenar)
                            //{
                            //    DetaNaviera pDeta = new DetaNaviera
                            //    {
                            //        n_contenedor = itemDS.n_contenedor,
                            //        b_manejo = itemDS.b_manejo.Trim().TrimEnd().TrimStart(),
                            //        b_transferencia = itemDS.b_transferencia.Trim().TrimEnd().TrimStart(),
                            //        b_despacho = itemDS.b_despacho.Trim().TrimEnd().TrimStart(),
                            //        c_cliente = item.c_naviera,
                            //        c_llegada = item.c_llegada,
                            //        c_tamaño = itemDS.c_tamaño,
                            //        b_estado = itemDS.b_estado,
                            //        c_condicion = itemDS.c_condicion,
                            //        v_peso = itemDS.v_peso,
                            //        s_consignatario = itemDS.s_consignatario
                            //    };

                            //    string resultado = DetaNavieraDAL.AlmacenarSADFI(pDeta, DBComun.Estado.verdadero);

                            //}


                            //foreach (var itemDSS in _listaAOrdenar)
                            //{
                            //    DetaNaviera pDeta = new DetaNaviera
                            //    {
                            //        n_contenedor = itemDSS.n_contenedor,
                            //        b_manejo = itemDSS.b_manejo.Trim().TrimEnd().TrimStart(),
                            //        b_transferencia = itemDSS.b_transferencia.Trim().TrimEnd().TrimStart(),
                            //        b_despacho = itemDSS.b_despacho.Trim().TrimEnd().TrimStart(),
                            //        c_cliente = item.c_naviera,
                            //        c_llegada = item.c_llegada,
                            //        c_tamaño = itemDSS.c_tamaño,
                            //        b_estado = itemDSS.b_estado,
                            //        c_condicion = itemDSS.c_condicion,
                            //        v_peso = itemDSS.v_peso,
                            //        s_consignatario = itemDSS.s_consignatario,
                            //        c_correlativo = itemDSS.c_correlativo,
                            //        v_tara = itemDSS.v_tara,
                            //        s_comodity = itemDSS.s_comodity,
                            //        c_pais_origen = itemDSS.c_pais_origen,
                            //        c_pais_destino = itemDSS.c_pais_destino
                            //    };

                            //    string resultado = DetaNavieraDAL.AlmacenarSADFI_AMP(pDeta, DBComun.Estado.verdadero);

                            //}

                            _notiAutorizados.GenerarAplicacionCX(_listaAOrdenar, c_naviera, naviera, item.c_llegada, item.IdReg,
                             item.d_buque, item.f_llegada, valores, valores, null);

                            
                        }
                    }

                    break;

                }

                return string.Format("Cantidad de manifiestos autorizados {0} de {1} ", cont, cantidad);

            }
            else
            {
                return string.Format("No existen contenedores pendientes de autorización", valores, cantidad);
            }
        }

        public void Timer1_Tick(object sender, EventArgs e)
        {
            try         
            {
                Cargar();
                if (GridView1.Rows.Count <= 0)                
                    btnCargar.Visible = false;                
                else
                    btnCargar.Visible = true;
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }


        [WebMethod]
        public static int RevisarManif(int manif, int IdReg, int IdDoc)
        {
            int validR = Convert.ToInt32(DetaDocDAL.VerificarValid(manif, IdReg, DBComun.Estado.verdadero, IdDoc));
            return validR;

        }

        [WebMethod]
        public static string OmitirValid(int manif, int IdReg, string Comentarios)
        {
            if (Comentarios != string.Empty && manif > 0)
            {
                string validR = DetaDocDAL.ActualizarOmitir(Comentarios, manif, HttpContext.Current.Session["c_usuario"].ToString(), IdReg);
                validR = "Almacenado Correctamante!!";
                return validR;
            }
            else
                throw new Exception("Debe seleccionar manifiesto a validar e incluir comentarios");


        }

        [WebMethod]
        public static string GenerarListadoExcel(int IdReg, int cant)
        {
            
            string resultado = GenerarListado(IdReg, cant);
            return resultado;

        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    foreach (GridViewRow row in GridView1.Rows)
            //    {

            //        CheckBox chk = row.FindControl("CheckBox1") as CheckBox;
            //        if (chk.Checked)
            //        {
            //            int nmanif = Convert.ToInt32(row.Cells[1].Text);
            //            int validR = Convert.ToInt32(DetaDocDAL.VerificarValid(nmanif));

            //            if (validR == 0)
            //            {
            //                ScriptManager.RegisterClientScriptBlock(chk, typeof(Button), "Popup", "javascript:Confirm(" + nmanif.ToString() + ");", true);
            //            }
            //            else if (validR == 2)
            //            {
            //                ScriptManager.RegisterClientScriptBlock(chk, typeof(Button), "Popup", "javascript:Confirm1(" + nmanif.ToString() + ");", true);                           
            //            }

            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
               
            //    CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
            //    string ClientScript = string.Format("<script>alert('" + ex.Message + "');</script>");
            //    //this.ClientScript.RegisterStartupScript(this.GetType(), "WOpen", ClientScript, true);
            //    ScriptManager.RegisterClientScriptBlock(btnCargar, typeof(Button), "WOpen", ClientScript, true);
            //    _cargar.Clear("wfConsultaBuques.aspx", ex.Message);
            //}

        }

        public void OnConfirm(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
            }
        }
            


    }
}