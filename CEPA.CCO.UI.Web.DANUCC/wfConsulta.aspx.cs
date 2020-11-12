using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CEPA.CCO.UI.Web.DANUCC
{
    public partial class wfConsulta : System.Web.UI.Page
    {
        private static readonly DateTime FIRST_GOOD_DATE = new DateTime(1900, 01, 01);
        private static readonly DateTime SECOND_GOOD_DATE = new DateTime(2016, 01, 01);

        protected void Page_Load(object sender, EventArgs e)
        {

        }



        protected void grvRetenciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime f_liberados;
                DateTime f_tramite;
                DateTime f_rpatio;
                DateTime f_recepcion;
                DateTime f_ini_dan;
                DateTime f_retenido;
                DateTime f_cancelado;
                string TipoRevisa = null;
                f_recepcion = Convert.ToDateTime(e.Row.Cells[4].Text);
                f_rpatio = Convert.ToDateTime(e.Row.Cells[5].Text);
                f_retenido = Convert.ToDateTime(e.Row.Cells[6].Text);
                f_cancelado = Convert.ToDateTime(e.Row.Cells[7].Text);
                f_tramite = Convert.ToDateTime(e.Row.Cells[8].Text);
                f_ini_dan = Convert.ToDateTime(e.Row.Cells[9].Text);
                f_liberados = Convert.ToDateTime(e.Row.Cells[11].Text);
                TipoRevisa = e.Row.Cells[1].Text;

                HiddenField hEstado = (HiddenField)e.Row.FindControl("hEstado") as HiddenField;

                if (hEstado.Value == "CANCELADO")
                {
                    e.Row.BackColor = Color.FromName("#f8d7da");
                    e.Row.ForeColor = Color.FromName("#721c24");
                }

                if (TipoRevisa == "NAD")
                {
                    e.Row.Cells[1].Text = "";
                }

                if (f_liberados > FIRST_GOOD_DATE)
                {
                    e.Row.BackColor = Color.FromName("#dff0d8");
                    e.Row.ForeColor = Color.FromName("#155724");
                }
                else
                {
                    e.Row.Cells[11].Text = "";
                    e.Row.BackColor = Color.FromName("#fcf8e3");
                    e.Row.ForeColor = Color.FromName("#8a6d3b");
                }

                if (f_tramite > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[8].Text = "";
                }
                if (f_rpatio > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[5].Text = "";
                }
                if (f_recepcion > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[4].Text = "";
                }
                if (f_ini_dan > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[9].Text = "";
                }
                if (f_retenido > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[6].Text = "";
                }
                if (f_recepcion > SECOND_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[4].Text = "";
                }
                if (f_rpatio > SECOND_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[5].Text = "";
                }
                if (f_cancelado > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[7].Text = "";
                }

            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            List<DetaNaviera> pLista = new List<DetaNaviera>();
            List<DetaNaviera> pLista1 = new List<DetaNaviera>();
            try
            {
                string b_valido = null;
                string b_cancel = null;
                string mensaje = null;
                string label = null;
                string valida = null;

                string v_mensaje = null;
                string v_label = null;

                b_retenido.Text = "";
                grvRetenciones.DataSource = null;
                grvRetenciones.DataBind();

                if(txtContenedor.Text.Length == 0)
                {
                    b_retenido.Text = "El # de Contenedor no posee un valor ";
                    throw new Exception("El # de Contenedor no posee un valor ") ;
                }
                string b_shipper = DetaNavieraDAL.validShipper(DBComun.Estado.verdadero, txtContenedor.Text.Trim().TrimEnd().TrimStart(), DBComun.TipoBD.SqlServer);

                if (b_shipper != "VALIDO")
                {
                    if (txtContenedor.Text.Trim().TrimEnd().TrimStart().Length != 11)
                    {
                        b_retenido.Text = "El # de Contenedor debe poseer 11 caracteres: " + txtContenedor.Text;
                        throw new Exception("El # de Contenedor debe poseer 11 caracteres: " + txtContenedor.Text);
                    }

                    valida = DetaNavieraDAL.ValidaContenedor(DBComun.Estado.verdadero, txtContenedor.Text.Trim().TrimEnd().TrimStart(), DBComun.TipoBD.SqlServer);

                    if (valida != "VALIDO")
                    {
                        b_retenido.Text = "El # de Contenedor no es válido: " + txtContenedor.Text;
                        throw new Exception("El # de Contenedor no es válido: " + txtContenedor.Text);
                    }
                }

                if (Datepicker.Text.Length != 4)
                {
                    b_retenido.Text = "El año de manifiesto posee 4 dígitos: " + Datepicker.Text;
                    throw new Exception("El año de manifiesto posee 4 dígitos: " + Datepicker.Text);
                }
                if (ArchivoBookingDAL.isNumeric(txtMani.Text) == false)
                {
                    b_retenido.Text = "El # de Manifiesto solo debe poseer digitos: " + txtMani.Text;
                    throw new Exception("El # de Manifiesto solo debe poseer digitos: " + txtMani.Text);
                }
                if (ArchivoBookingDAL.isNumeric(Datepicker.Text) == false)
                {
                    b_retenido.Text = "El Año del Manifiesto solo debe poseer digitos: " + Datepicker.Text;
                    throw new Exception("El Año del Manifiesto solo debe poseer digitos: " + Datepicker.Text);
                }


                if (Datepicker.Text.Length == 4 && ArchivoBookingDAL.isNumeric(txtMani.Text))
                {
                    pLista = DetaNavieraDAL.ConsultarDAN_Web(txtContenedor.Text.Trim().TrimEnd().TrimStart(), Datepicker.Text.Trim().TrimEnd().TrimStart(), Convert.ToInt32(txtMani.Text.Trim().TrimEnd().TrimStart())).ToList();

                    if (pLista.Count > 0)
                    {
                        foreach (var item in pLista)
                        {
                            b_valido = item.b_retenido;
                            b_cancel = item.b_estado;
                            if (b_cancel == "CANCELADO")
                            {
                                mensaje = "El contenedor " + txtContenedor.Text + " se encuentra cancelado";
                                label = "El contenedor " + txtContenedor.Text.ToUpper() + " se encuentra " + " <strong><u><font color='red'>CANCELADO" + "</font></u></strong>";

                                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + mensaje + "');", true);
                                grvRetenciones.DataSource = pLista;
                                grvRetenciones.DataBind();
                            }
                            else
                            {
                                if (item.b_estadoV == "1" && (item.TipoRe == "DAN" || item.TipoRe == "UCC" || item.TipoRe == "DGA"))
                                {
                                    if (b_valido == "0")
                                    {
                                        mensaje = "El contenedor " + txtContenedor.Text + " se encuentra liberado por " + item.TipoRe + "/ ";
                                        label = "El contenedor " + txtContenedor.Text.ToUpper() + " se encuentra " + " <strong><u><font color='green'>LIBERADO POR " + item.TipoRe + "/ " + "</font></u></strong>" ;
                                    }
                                    else
                                    {
                                        mensaje = mensaje + "El contenedor " + txtContenedor.Text + " se encuentra retenido por " + item.TipoRe + "/ ";
                                        label = label + ("El contenedor " + txtContenedor.Text.ToUpper() + " se encuentra " + " <strong><u><font color='red'>RETENIDO POR " + item.TipoRe +  "/ " + "</font></u></strong>");
                                    }                                    
                                }
                                else
                                {
                                    if (item.TipoRe == "NAD")
                                    {
                                        mensaje = "El contenedor " + txtContenedor.Text + " no se encuentra retenido";
                                        label = "El contenedor " + txtContenedor.Text.ToUpper() + " no se encuentra retenido";
                                    }

                                    //ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + mensaje + "');", true);
                                    //grvRetenciones.DataSource = pLista;
                                    //grvRetenciones.DataBind();
                                    // Label lblEmptyMessage = GridView1.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                                    //throw new Exception("Este contenedor no se encuentra retenido: " + txtBuscar.Text);
                                }                                                                              
                            }

                            if (mensaje.Contains("retenido"))
                            {
                                v_mensaje = mensaje;
                                v_label = label;
                            }
                            else
                            {
                                v_mensaje = "El contenedor " + txtContenedor.Text + " se encuentra liberado";
                                v_label = "El contenedor " + txtContenedor.Text.ToUpper() + " se encuentra " + " <strong><u><font color='green'>LIBERADO "  + "</font></u></strong>";
                            }
                        }
                    }
                    else
                    {
                        grvRetenciones.DataSource = null;
                        grvRetenciones.DataBind();
                        // Label lblEmptyMessage = GridView1.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                        label = "No se poseen registros de información para el contenedor: " + txtContenedor.Text.ToUpper() + " Se recomienda confirmar el número de manifiesto.";
                        ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + label + "');", true);
                    }

                    //string[] cadena = v_mensaje.Split('/');


                    //if(cadena != null && cadena.Length > 0)
                    //{
                    //    cadena = cadena.Where(x => !x.Contains("retenido")).ToArray();
                    //}

                    //if(cadena.Length == 0)
                    //{
                    //    mensaje = "El contenedor " + txtContenedor.Text + " se encuentra liberado";
                    //    label = "El contenedor " + txtContenedor.Text.ToUpper() + " se encuentra " + " <strong><u><font color='green'>LIBERADO " + "</font></u></strong>";
                    //}
                    //else if(cadena.Length >= 1)
                    //{
                    //    for (int i = 0; i < cadena.Length; i++)
                    //    {
                    //        mensaje += cadena[i];
                    //        label = v_label;
                    //    }
                    //}

                    if (v_mensaje.Length > 0)
                    {
                        //if (v_mensaje.Contains("retenido"))
                        //{
                        mensaje = v_mensaje.Remove(v_mensaje.Length - 1);
                        label = v_label.Remove(v_label.Length - 1);
                        //}
                    }

                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + mensaje + "');", true);
                    grvRetenciones.DataSource = pLista;
                    grvRetenciones.DataBind();

                    b_retenido.Text = label;
                }
                else
                {
                    grvRetenciones.DataSource = null;
                    grvRetenciones.DataBind();
                    //  Label lblEmptyMessage = GridView1.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                    label = "El contenedor: " + txtContenedor.Text.ToUpper() + " no posee los 11 caracteres requeridos";
                    b_retenido.Text = label;
                    grvRetenciones.DataSource = null;
                    grvRetenciones.DataBind();
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + label + "');", true);
                }


                grvRetenciones.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

                // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                grvRetenciones.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
                grvRetenciones.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
                grvRetenciones.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
                grvRetenciones.HeaderRow.Cells[6].Attributes["data-hide"] = "phone";
                grvRetenciones.HeaderRow.Cells[7].Attributes["data-hide"] = "phone";

                //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

                grvRetenciones.HeaderRow.TableSection = TableRowSection.TableHeader;

                grvRetenciones.FooterRow.Cells[0].Attributes["text-align"] = "center";
                grvRetenciones.FooterRow.TableSection = TableRowSection.TableFooter;
                //  ViewState["EmployeeList"] = GridView1.DataSource;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfConsulta.aspx");
        }
    }
}