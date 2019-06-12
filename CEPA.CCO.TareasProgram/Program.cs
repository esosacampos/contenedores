using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;
using System.IO;
using System.Globalization;
using MoreLinq;


namespace CEPA.CCO.TareasProgram
{
    public class Program
    {
        public static string Archivo = Application.StartupPath + "\\CEPA_CCO_DAN.TXT";
  

        static void Main(string[] args)
        {
            DateTime _fecha;
            string Html = "";
            EnvioCorreo _correo = new EnvioCorreo();
            TextWriter tw = new StreamWriter(Archivo, true);
            try
            {
                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ===>> INICIO DE ALERTA <<===");

                var query = (from a in AlertaDANDAL.ObtenerAlerta(DBComun.Estado.falso, "")
                             join b in EncaBuqueDAL.ObtenerBuquesJoinA(DBComun.Estado.falso) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                             select new AlertaDAN
                             {
                                 c_numeral = a.c_numeral,
                                 c_naviera = a.c_naviera,
                                 d_naviera = b.d_cliente,
                                 n_contenedor = a.n_contenedor,
                                 f_liberacion = a.f_liberacion,
                                 ClaveP = a.ClaveP,
                                 ClaveQ = a.ClaveQ,
                                 c_transporte = a.c_transporte
                             }).ToList();

                Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                Html += "<b><u> LISTADO DE CONTENEDORES REVISADOS Y LIBERADOS POR LA DAN   </b></u><br />";
                Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;\">";
                Html += "<tr>";
                _fecha = DetaNavieraLINQ.FechaBDS();
                Html += "<td style=\"text-align: left;\"><font size=2>Fecha/Hora&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp&nbsp;" + _fecha.ToString() + "</font></td>";
                Html += "</tr>";
                Html += "<tr>";
                Html += "<td style=\"text-align: left;\" ><font size = 2>Usuario&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + "alerta.contenedores" + "</font></td>";
                Html += "</tr>";
                Html += "</table>";
                Html += "<br />";


                Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                Html += "<tr>";
                Html += "<center>";
                Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</font></th>";
                Html += "<td width=\"40px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                Html += "<td width=\"50px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>FECHA/HORA LIBERACION</font></th>";
                Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>P</font></th>";
                Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>Q</font></th>";
                Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TRANSPORTE</font></th>";
                Html += "<td width=\"40px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>AGENCIA NAVIERA</font></th>";
                Html += "</center>";
                Html += "</tr>";
                
                foreach (var itemC in query)
                {
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.c_numeral + "</font></td>";
                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.n_contenedor + "</font></td>";
                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.f_liberacion + "</font></td>";
                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.ClaveP + "</font></td>";
                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.ClaveQ + "</font></td>";
                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.c_transporte + "</font></td>";  
                    Html += "<td height=\"25\"><font size=2 color=blue>" + itemC.d_naviera + "</font></td>";
                    Html += "</center>";
                    Html += "</tr>";
                    Html += "</font>";
                }

                Html += "</table>";

                Html += "<br /><br /><br />";
                Html += "<font color='blue' size=4><b>NOTA:</b><br/><br/><i>Favor gestionar con quien corresponda, la movilización inmediata de los contenedores listados, con el propósito de liberar espacios para el registro de otros contenedores, CEPA se reserva el derecho de movilizarlos con sus equipos y facturar el arrendamiento de estos.-</i></font><br />";
                Html += "<br />";

                _correo.Subject = string.Format("LISTADOS DE CONTENEDORES LIBERADOS POR LA DAN {0}", _fecha.ToString("dd/MM/yyyy HH:mm"));

                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificacionesCCAlert("b_noti_alerta", DBComun.Estado.falso);               

                _correo.Asunto = Html;
                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso);

                var _copia = query.ToList();


                foreach (var naviera in _copia.DistinctBy(p=> p.c_naviera))
                {
                    Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                    Html += "<b><u> LISTADO DE CONTENEDORES REVISADOS Y LIBERADOS POR LA DAN  </b></u><br />";
                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;\">";
                    Html += "<tr>";
                    _fecha = DetaNavieraLINQ.FechaBDS();
                    Html += "<td style=\"text-align: left;\"><font size=2>Fecha/Hora&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                    Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp&nbsp;" + _fecha.ToString() + "</font></td>";
                    Html += "</tr>";
                    Html += "<tr>";
                    Html += "<td style=\"text-align: left;\" ><font size = 2>Usuario&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                    Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + "alerta.contenedores" + "</font></td>";
                    Html += "</tr>";
                    Html += "</table>";
                    Html += "<br />";


                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</font></th>";
                    Html += "<td width=\"40px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "<td width=\"50px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>FECHA/HORA LIBERACION</font></th>";
                    Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>P</font></th>";
                    Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>Q</font></th>";
                    Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TRANSPORTE</font></th>";
                    Html += "<td width=\"40px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>AGENCIA NAVIERA</font></th>";
                    Html += "</center>";
                    Html += "</tr>";

                    foreach (var itemC in query.Where(c => c.c_naviera.Equals(naviera.c_naviera)))
                    {
                        Html += "<tr>";
                        Html += "<center>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.c_numeral + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.n_contenedor + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.f_liberacion + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.ClaveP + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.ClaveQ + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.c_transporte + "</font></td>";
                        Html += "<td height=\"25\"><font size=2 color=blue>" + itemC.d_naviera + "</font></td>";
                        Html += "</center>";
                        Html += "</tr>";
                        Html += "</font>";
                    }

                    Html += "</table>";

                    Html += "<br /><br /><br />";
                    Html += "<font color='blue' size=4><b>NOTA:</b><br/><br/><i>Favor gestionar con quien corresponda, la movilización inmediata de los contenedores listados, con el propósito de liberar espacios para el registro de otros contenedores, CEPA se reserva el derecho de movilizarlos con sus equipos y facturar el arrendamiento de estos.-</i></font><br />";
                    Html += "<br />";

                    _correo.Subject = string.Format("LISTADOS DE CONTENEDORES LIBERADOS POR LA DAN {0}", _fecha.ToString("dd/MM/yyyy HH:mm"));

                    _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_alerta", DBComun.Estado.falso, naviera.c_naviera);

                    _correo.ListaCC = NotificacionesDAL.ObtenerNotificacionesRestrAlert(DBComun.Estado.falso);

                    _correo.Asunto = Html;

                    _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso);
                }




                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ===>> ENVIO DE ALERTA EXITOSO <<===");
                tw.Flush();
                tw.Dispose();
                tw.Close();
                
           
                Application.Exit();
            }
            catch (Exception ex)
            {
                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: NO SE PUEDO INICIAR LA ALERTA (" + ex.Message + ")");
                tw.Flush();
                tw.Dispose();
                tw.Close();                
            }
        }
    }
}
