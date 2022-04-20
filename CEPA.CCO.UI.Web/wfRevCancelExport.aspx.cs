using CEPA.CCO.BL;
using CEPA.CCO.Entidades;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace CEPA.CCO.UI.Web
{
    public partial class wfRevCancelExport : System.Web.UI.Page
    {
        private Cont_ExpBL BLConte = new Cont_ExpBL();
        public List<Cont_Exp_Rev> cont_rev = new List<Cont_Exp_Rev>();
        private int idDeta = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Limpiar();
            }
        }

        protected void btn_Buscar_Click(object sender, EventArgs e)
        {
            string msgError = "OK";
            if (txt_Contenedor.Value != "" && (txt_Contenedor.Value).Length == 11)
            {
                cont_rev = BLConte.FindContenedor(txt_Contenedor.Value, ref msgError);
                if (msgError == "OK")
                {
                    if (cont_rev[0].b_autorizado)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Contenedor autorizado el " + cont_rev[0].f_autorizado.ToString() + "');", true);
                    }
                    else if (!cont_rev[0].b_cancelado)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Imposible revertir, Contenedor no esta cancelado.');", true);
                    }
                    else
                    {
                        idDeta = cont_rev[0].IdDeta;
                        txt_justificar.Text = "";
                        txt_justificar.Enabled = true;
                        txt_justificar.Focus();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Numero de contenedor no encontrado.');", true);
                    //Response.Write("<script type='text/javascript'>bootbox.alert('Numero de contenedor no encontrado.');</script>");
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Verifique el número de contenedor.');", true);
                //Response.Write("<script type='text/javascript'>windows.alert('Verifique el número de contenedor.');</script>");
            }
        }

        protected void btn_Guardar_Click(object sender, EventArgs e)
        {
            string msgError = "OK";
            if (txt_justificar.Text.Length >= 10)
            {
                msgError = BLConte.Rev_Cont_Exp(idDeta, txt_justificar.Text.Trim());
                if (msgError != "NO")
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Reversión realizada exitosamente.');", true);
                    //Response.Write("<script>window.alert('Reversión realizada exitosamente.');</script>");
                    Limpiar();
                }

            }
            else
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Favor detalle mejor el motivo.');", true);
            //Response.Write("<script>window.alert('Favor detalle mejor el motivo.');</script>");
        }

        protected void btn_Cancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            txt_justificar.Text = "";
            txt_justificar.Enabled = false;
            txt_Contenedor.Value = "";
            txt_Contenedor.Focus();
        }
    }
}