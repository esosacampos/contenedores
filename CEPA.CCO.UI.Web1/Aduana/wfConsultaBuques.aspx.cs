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
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        private void Cargar()
        {
            EncaBuqueBL _encaBL = new EncaBuqueBL();

            GridView1.DataSource = DocBuqueLINQ.ObtenerAduana();
            GridView1.DataBind();
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
            int valores = 0;
            int cantidad = GridView1.Rows.Count;
            int Correlativo = 0;
            string naviera = null;
            string d_buque = null;
            int cont = 0;
            DateTime f_llegada;
            NotiAutorizados _notiAutorizados = new NotiAutorizados();
            CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
            int validC = 0;

            try
            {
               // validC = Convert.ToInt32(DetaDocDAL.VerificarValid(n_manifiesto));
    
               _cargar.Imprimir();
                GenerarAutorizacion(ref valores, cantidad, ref Correlativo, ref naviera, ref cont, _notiAutorizados);
                Thread.Sleep(1000);
                string ClientScript = string.Format("<script>alert('Cantidad de barcos autorizados {0} de {1}');</script>", cont, cantidad);
                this.ClientScript.RegisterStartupScript(this.GetType(), "WOpen", ClientScript, true);                
                ScriptManager.RegisterClientScriptBlock(btnCargar, typeof(Button), "WOpen", ClientScript, true);
                _cargar.Clear("wfConsultaBuques.aspx", string.Format("Cantidad de barcos autorizados {0} de {1}", cont, cantidad));
            }
            catch (Exception ex)
            {
                //_cargar.Imprimir();
                string ClientScript = string.Format("<script>alert('" + ex.Message + "');</script>");
                //this.ClientScript.RegisterStartupScript(this.GetType(), "WOpen", ClientScript, true);
                ScriptManager.RegisterClientScriptBlock(btnCargar, typeof(Button), "WOpen", ClientScript, true);               
                _cargar.Clear("wfConsultaBuques.aspx", ex.Message);
            }
            finally
            {
                HttpContext.Current.Session["SeleTodos"] = null;
            }
        }

        private void GenerarAutorizacion(ref int valores, int cantidad, ref int Correlativo, ref string naviera, ref int cont, NotiAutorizados _notiAutorizados)
        {
            SeleccionManager.Seleccion((GridView)GridView1);
            List<EncaNaviera> _lit = HttpContext.Current.Session["SeleTodos"] as List<EncaNaviera>;
            int IdDocW = 0;


            

            if (_lit == null)
                _lit = new List<EncaNaviera>();





            if (_lit.Count > 0)
            {
             
                foreach (EncaNaviera item in _lit)
                {
                    
                    int validR = Convert.ToInt32(DetaDocDAL.VerificarValid(item.num_manif));

                    if (validR == 0)
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


                            List<DetaNaviera> _listAutorizados = DetaNavieraDAL.ObtenerAutorizados(item.IdReg, IdDocW);
                            Correlativo = Convert.ToInt32(DetaNavieraDAL.ObtenerCorrelativo(item.c_imo, item.c_llegada, c_naviera)) + 1;

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
                            List<ArchivoAduana> _listaAOrdenar = DetaNavieraDAL.ObtenerResultado(item.IdReg, _rango, Correlativo - 1);


                            //    _notiAutorizados.GenerarExcelDAN(_listaAOrdenar, c_naviera, naviera, item.c_llegada, item.IdReg,
                            //    item.d_buque, item.f_llegada, valores, valores, null);

                            _notiAutorizados.GenerarAplicacionCX(_listaAOrdenar, c_naviera, naviera, item.c_llegada, item.IdReg,
                             item.d_buque, item.f_llegada, valores, valores, null);

                            HttpContext.Current.Session["SeleTodos"] = null;
                        }
                    }
                
                }

            }
            else
            {
                Response.Write(string.Format("<script>alert('No existen contenedores pendientes de autorización');</script>", valores, cantidad));
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

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in GridView1.Rows)
                {

                    CheckBox chk = row.FindControl("CheckBox1") as CheckBox;
                    if (chk.Checked)
                    {
                        int nmanif = Convert.ToInt32(row.Cells[1].Text);
                        int validR = Convert.ToInt32(DetaDocDAL.VerificarValid(nmanif));

                        if (validR == 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(chk, typeof(Button), "Popup", "javascript:Confirm(" + nmanif.ToString() + ");", true);
                        }
                        else if (validR == 2)
                        {
                            ScriptManager.RegisterClientScriptBlock(chk, typeof(Button), "Popup", "javascript:Confirm1(" + nmanif.ToString() + ");", true);                           
                        }

                    }
                }
            }
            catch (Exception ex)
            {
               
                CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
                string ClientScript = string.Format("<script>alert('" + ex.Message + "');</script>");
                //this.ClientScript.RegisterStartupScript(this.GetType(), "WOpen", ClientScript, true);
                ScriptManager.RegisterClientScriptBlock(btnCargar, typeof(Button), "WOpen", ClientScript, true);
                _cargar.Clear("wfConsultaBuques.aspx", ex.Message);
            }

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

        //protected void CheckBox1_PreRender(object sender, EventArgs e)
        //{
        //    CheckBox startCheckBox = (CheckBox)sender;
        //    const string ReturnConfirm = "if (!confirm(‘Are you sure you want to update this entry?’)) return false;";
        //    startCheckBox.Attributes.Add("OnClick", ReturnConfirm);            
        //}



      


    }
}