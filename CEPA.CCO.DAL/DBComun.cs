using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.Sql;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web.Configuration;
using Sybase.Data.AseClient;
using AMS.Profile;
using System.Web;
using System.IO;
using System.Windows;
using System.Windows.Forms;





namespace CEPA.CCO.DAL
{
    public class DBComun
    {
        public enum TipoBD
        {
            SqlServer,
            SyBase,
            Excel,
            SqlContratistas,
            VFP,
            SqlLink,
            SqlTracking,
            SyBaseNET
        }

        public enum TipoCorreo
        {
            CEPA,
            Gmail
        }

        public enum Estado
        {
            falso,
            verdadero
        }
        IDbTransaction Transaccion;
        SqlTransaction TransaSQL;
        public IDbTransaction GetTransacction()
        {
            return Transaccion;
        }

        public SqlTransaction GetTransacctionSQL()
        {
            return TransaSQL;
        }


        public static string sRuta { get; set; }

        //Generar tabla para conexiones de bases de datos
        public static string GenerarCadenaSQL(TipoBD pTipo, Estado pEstado)
        {
            string aplicacion = Application.StartupPath + "\\DATA\\xmlConfig.xml";
            AMS.Profile.Xml archXML = new AMS.Profile.Xml();

            if (pEstado == Estado.verdadero)
                archXML = new AMS.Profile.Xml(HttpContext.Current.Server.MapPath("\\DATA\\xmlConfig.xml"));
            else
                archXML = new AMS.Profile.Xml(Application.StartupPath + "\\DATA\\xmlConfig.xml");

            string ConnectionString = null;
            switch (pTipo)
            {
                case TipoBD.SqlServer :
                                        ConnectionString = "Data Source=" + archXML.GetValue("CONFIGSQL", "Server", ".\\");
                                        ConnectionString += ";Initial Catalog=" + archXML.GetValue("CONFIGSQL", "Database", "CEPA_TAREAS");
                                        ConnectionString += ";Network Library=" + archXML.GetValue("CONFIGSQL", "Library", "dbmssocn (TCP/IP)").Substring(0, 8) + ";";
                                        ConnectionString += "Persist Security Info=" + archXML.GetValue("CONFIGSQL", "Persist", "True") + ";";
                                        ConnectionString += "User ID=" + archXML.GetValue("CONFIGSQL", "Username", "") + ";";
                                        ConnectionString += "Password=" + archXML.GetValue("CONFIGSQL", "Password", "") + ";";
                                        break;
                case TipoBD.SyBase :
                                        ConnectionString = "Provider=" + archXML.GetValue("CONFIGSYBASE", "Provider", ".\\");
                                        ConnectionString += ";Server Name=" + archXML.GetValue("CONFIGSYBASE", "ServerName", "10.1.4.30");
                                        ConnectionString += ";Server Port Address=" + archXML.GetValue("CONFIGSYBASE", "ServerPortAddress", "5000");
                                        ConnectionString += ";Initial Catalog=" + archXML.GetValue("CONFIGSYBASE", "Database", "CEPA_SADFI");
                                        ConnectionString += ";User ID=" + archXML.GetValue("CONFIGSYBASE", "Username", "");
                                        ConnectionString += ";Password=" + archXML.GetValue("CONFIGSYBASE", "Password", "") + ";";
                                        break;
                case TipoBD.VFP:
                                        ConnectionString = "Provider=" + archXML.GetValue("CONFIGVFP", "Provider", ".\\");
                                        ConnectionString += ";Data Source=" + archXML.GetValue("CONFIGVFP", "Data Source", "10.1.4.30");                                        
                                        break;
                case TipoBD.Excel :
                                        ConnectionString = "Provider=" + archXML.GetValue("CONFIGEXCEL", "Provider", ".\\");
                                        ConnectionString += ";Data Source=\"" + sRuta;
                                        ConnectionString += "\";Extended Properties=\"" + archXML.GetValue("CONFIGEXCEL", "Extended", "12.0");                                     
                                        ConnectionString += ";HDR=" + archXML.GetValue("CONFIGEXCEL", "HDR", "") + "\";";    
                                        break;
                case TipoBD.SqlContratistas:
                                        ConnectionString = "Data Source=" + archXML.GetValue("CONFIGSQL1", "Server", ".\\");
                                        ConnectionString += ";Initial Catalog=" + archXML.GetValue("CONFIGSQL1", "Database", "CEPA_TAREAS");
                                        ConnectionString += ";Network Library=" + archXML.GetValue("CONFIGSQL1", "Library", "dbmssocn (TCP/IP)").Substring(0, 8) + ";";
                                        ConnectionString += "Persist Security Info=" + archXML.GetValue("CONFIGSQL1", "Persist", "True") + ";";
                                        ConnectionString += "User ID=" + archXML.GetValue("CONFIGSQL1", "Username", "") + ";";
                                        ConnectionString += "Password=" + archXML.GetValue("CONFIGSQL1", "Password", "") + ";";
                                        break;
                case TipoBD.SqlLink:
                                        ConnectionString = "Data Source=" + archXML.GetValue("CONFIGLINKED", "Server", ".\\");
                                        ConnectionString += ";Initial Catalog=" + archXML.GetValue("CONFIGLINKED", "Database", "CEPA_CONTENEDORES");
                                        ConnectionString += ";Network Library=" + archXML.GetValue("CONFIGLINKED", "Library", "dbmssocn (TCP/IP)").Substring(0, 8) + ";";
                                        ConnectionString += "Persist Security Info=" + archXML.GetValue("CONFIGLINKED", "Persist", "True") + ";";
                                        ConnectionString += "User ID=" + archXML.GetValue("CONFIGLINKED", "Username", "") + ";";
                                        ConnectionString += "Password=" + archXML.GetValue("CONFIGLINKED", "Password", "") + ";";
                                        break;
                case TipoBD.SqlTracking:
                                        ConnectionString = "Data Source=" + archXML.GetValue("CONFIGSQL2", "Server", ".\\");
                                        ConnectionString += ";Initial Catalog=" + archXML.GetValue("CONFIGSQL2", "Database", "CEPA_TAREAS");
                                        ConnectionString += ";Network Library=" + archXML.GetValue("CONFIGSQL2", "Library", "dbmssocn (TCP/IP)").Substring(0, 8) + ";";
                                        ConnectionString += "Persist Security Info=" + archXML.GetValue("CONFIGSQL2", "Persist", "True") + ";";
                                        ConnectionString += "User ID=" + archXML.GetValue("CONFIGSQL2", "Username", "") + ";";
                                        ConnectionString += "Password=" + archXML.GetValue("CONFIGSQL2", "Password", "") + ";";
                                        break;
                case TipoBD.SyBaseNET:                                       
                                        ConnectionString = "Data Source=" + archXML.GetValue("CONFIGSYBASE", "ServerName", "10.1.4.30");
                                        ConnectionString += ";Port=" + archXML.GetValue("CONFIGSYBASE", "ServerPortAddress", "5000");
                                        ConnectionString += ";Database=" + archXML.GetValue("CONFIGSYBASE", "Database", "CEPA_SADFI");
                                        ConnectionString += ";Uid=" + archXML.GetValue("CONFIGSYBASE", "Username", "");
                                        ConnectionString += ";Pwd=" + archXML.GetValue("CONFIGSYBASE", "Password", "") + ";";
                                        break;
            }          

            return ConnectionString;
        }



        public static IDbConnection ObtenerConexion(TipoBD pTipo, Estado pEstado)
        {
            if (pTipo == TipoBD.SqlServer)
                return new SqlConnection(GenerarCadenaSQL(TipoBD.SqlServer, pEstado));
            else if (pTipo == TipoBD.SyBase)
                return new OleDbConnection(GenerarCadenaSQL(TipoBD.SyBase, pEstado));
            else if (pTipo == TipoBD.Excel)
                return new OleDbConnection(GenerarCadenaSQL(TipoBD.Excel, pEstado));
            else if (pTipo == TipoBD.SqlContratistas)
                return new SqlConnection(GenerarCadenaSQL(TipoBD.SqlContratistas, pEstado));
            else if (pTipo == TipoBD.VFP)
                return new OleDbConnection(GenerarCadenaSQL(TipoBD.VFP, pEstado));
            if (pTipo == TipoBD.SqlLink)
                return new SqlConnection(GenerarCadenaSQL(TipoBD.SqlLink, pEstado));
            if (pTipo == TipoBD.SqlTracking)
                return new SqlConnection(GenerarCadenaSQL(TipoBD.SqlTracking, pEstado));
            else if (pTipo == TipoBD.SyBaseNET)
                return new AseConnection(GenerarCadenaSQL(TipoBD.SyBaseNET, pEstado));
            return null;
            //return new AseConnection(ConnectionSyBASE);
          
        }

        #region "Propiedades Correo"
        public static string smptServer(TipoCorreo pTipo, Estado pEstado)
        {
            AMS.Profile.Xml archXML1 = new AMS.Profile.Xml();

            if (pEstado == DBComun.Estado.verdadero)
                archXML1 = new AMS.Profile.Xml(HttpContext.Current.Server.MapPath("\\DATA\\xmlConfig.xml"));
            else
                archXML1 = new AMS.Profile.Xml(Application.StartupPath + "\\DATA\\xmlConfig.xml");

            if (pTipo == TipoCorreo.CEPA)
            {
                return archXML1.GetValue("MAIL", "smtpServer", "");
            }
            else
            {
                return archXML1.GetValue("MAIL2", "smtpServer", "");
            }
        }

        public static string smtpPort(TipoCorreo pTipo, Estado pEstado)
        {
            AMS.Profile.Xml archXML1 = new AMS.Profile.Xml();

            if (pEstado == DBComun.Estado.verdadero)
                archXML1 = new AMS.Profile.Xml(HttpContext.Current.Server.MapPath("\\DATA\\xmlConfig.xml"));
            else
                archXML1 = new AMS.Profile.Xml(Application.StartupPath + "\\DATA\\xmlConfig.xml");

            if (pTipo == TipoCorreo.CEPA)
            {
                return archXML1.GetValue("MAIL", "smtpPort", "25");
            }
            else
            {
                return archXML1.GetValue("MAIL2", "smtpPort", "25");
            }
        }

        public static bool smtpSSL(TipoCorreo pTipo, Estado pEstado)
        {
            AMS.Profile.Xml archXML1 = new AMS.Profile.Xml();

            if (pEstado == DBComun.Estado.verdadero)
                archXML1 = new AMS.Profile.Xml(HttpContext.Current.Server.MapPath("\\DATA\\xmlConfig.xml"));
            else
                archXML1 = new AMS.Profile.Xml(Application.StartupPath + "\\DATA\\xmlConfig.xml");

            if (pTipo == TipoCorreo.CEPA)
            {
                if (archXML1.GetValue("MAIL", "smtpSSL", "False") == "False")
                    return false;
                else
                    return true;
            }
            else
            {
                if (archXML1.GetValue("MAIL2", "smtpSSL", "True") == "True")
                    return true;
                else
                    return false;
            }
        }

        public static string smtpUserName(TipoCorreo pTipo, Estado pEstado)
        {
            AMS.Profile.Xml archXML1 = new AMS.Profile.Xml();

            if (pEstado == DBComun.Estado.verdadero)
                archXML1 = new AMS.Profile.Xml(HttpContext.Current.Server.MapPath("\\DATA\\xmlConfig.xml"));
            else
                archXML1 = new AMS.Profile.Xml(Application.StartupPath + "\\DATA\\xmlConfig.xml");

            if (pTipo == TipoCorreo.CEPA)
            {
                return archXML1.GetValue("MAIL", "smtpUser", "");
            }
            else
            {
                return archXML1.GetValue("MAIL2", "smtpUser", "");
            }
        }

        public static string smtpPassword(TipoCorreo pTipo, Estado pEstado)
        {
            AMS.Profile.Xml archXML1 = new AMS.Profile.Xml();

            if (pEstado == DBComun.Estado.verdadero)
                archXML1 = new AMS.Profile.Xml(HttpContext.Current.Server.MapPath("\\DATA\\xmlConfig.xml"));
            else
                archXML1 = new AMS.Profile.Xml(Application.StartupPath + "\\DATA\\xmlConfig.xml");

            if (pTipo == TipoCorreo.CEPA)
            {
                return archXML1.GetValue("MAIL", "smtpPwd", "");
            }
            else
            {
                return archXML1.GetValue("MAIL2", "smtpPwd", "");
            }
        }

        public static string fromMail(TipoCorreo pTipo, Estado pEstado)
        {
            AMS.Profile.Xml archXML1 = new AMS.Profile.Xml();

            if (pEstado == DBComun.Estado.verdadero)
                archXML1 = new AMS.Profile.Xml(HttpContext.Current.Server.MapPath("\\DATA\\xmlConfig.xml"));
            else
                archXML1 = new AMS.Profile.Xml(Application.StartupPath + "\\DATA\\xmlConfig.xml");

            if (pTipo == TipoCorreo.CEPA)
            {
                return archXML1.GetValue("MAIL", "fromMail", "");
            }
            else
            {
                return archXML1.GetValue("MAIL2", "fromMail", "");
            }
        }

        public static string fromName(TipoCorreo pTipo, Estado pEstado)
        {
            AMS.Profile.Xml archXML1 = new AMS.Profile.Xml();

            if (pEstado == DBComun.Estado.verdadero)
                archXML1 = new AMS.Profile.Xml(HttpContext.Current.Server.MapPath("\\DATA\\xmlConfig.xml"));
            else
                archXML1 = new AMS.Profile.Xml(Application.StartupPath + "\\DATA\\xmlConfig.xml");

            if (pTipo == TipoCorreo.CEPA)
            {
                return archXML1.GetValue("MAIL", "nameMail", "");
            }
            else
            {
                return archXML1.GetValue("MAIL2", "nameMail", "");
            }
        }
        #endregion
    }
}
