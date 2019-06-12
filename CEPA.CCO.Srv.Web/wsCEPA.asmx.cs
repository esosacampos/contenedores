using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;

namespace CEPA.CCO.Srv.Web
{
    /// <summary>
    /// Descripción breve de Service1
    /// </summary>
    [WebService(Namespace = "http://138.219.156.214:6044/", Description="Servicio de Transferencia de Informacion ADUANA - CEPA")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class wsCEPA : System.Web.Services.WebService
    {

        
        [WebMethod(Description="Metodo para registrar los estados de las declaraciones transmitido por ADUANA")]
        public string InsertRegAduana(string xmlDoc)
        {
            string _respuesta = null;

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xmlDoc);

            XmlNodeList listaCntres = doc.SelectNodes("CntsEstados/DECLA");

            XmlNode unContenedor;

            if (listaCntres.Count > 0)
            {
                for (int i = 0; i < listaCntres.Count; i++)
                {
                    unContenedor = listaCntres.Item(i);

                    string[] cadenaADUANA = unContenedor.SelectSingleNode("NUM_DOC").InnerText.Split('-');

                    string a_doc = null;
                    string c_doc = null;
                    string n_doc = null;
                    string b_sidunea = null;
                    int b_sidu = 0;
                    int b_transito = 0;




                    if (cadenaADUANA.Count() == 3)
                    {
                        a_doc = cadenaADUANA[0].ToString();
                        c_doc = cadenaADUANA[1].ToString();
                        n_doc = cadenaADUANA[2].ToString();
                    }
                    else if (cadenaADUANA.Count() == 4)
                    {
                        a_doc = cadenaADUANA[0].ToString();
                        c_doc = cadenaADUANA[1].ToString();
                        n_doc = cadenaADUANA[2].ToString();
                        b_sidunea = cadenaADUANA[3].ToString();
                    }

                    if (n_doc == "T")
                    {
                        b_transito = 1;
                        b_sidu = 1;
                    }

                    if (b_sidunea == "W")
                        b_sidu = 1;



                    string _fecha = unContenedor.SelectSingleNode("FEC_REG_ESTADO").InnerText.ToString();
                    string _resultP = null;

                    if (b_sidu == 0)
                    {
                        EstadosDecla estadoDe = new EstadosDecla
                        {
                            IdRegAduana = -1,
                            c_aduana = unContenedor.SelectSingleNode("KEY_ADUANA").InnerText,
                            n_manifiesto = Convert.ToInt32(unContenedor.SelectSingleNode("NUM_MANI").InnerText),
                            a_manifiesto = Convert.ToInt32(unContenedor.SelectSingleNode("YEAR_MANI").InnerText),
                            n_contenedor = unContenedor.SelectSingleNode("NUM_CONTEN").InnerText.Replace("-", "").ToUpper(),
                            a_decla = Convert.ToInt32(a_doc),
                            s_decla = Convert.ToInt32(c_doc),
                            c_decla = Convert.ToInt32(n_doc),
                            IdEstado = Convert.ToInt32(unContenedor.SelectSingleNode("ESTADO_ADUANA").InnerText),
                            f_reg_aduana = Convert.ToDateTime(unContenedor.SelectSingleNode("FEC_REG_ESTADO").InnerText),
                            IdSelectividad = Convert.ToInt32(unContenedor.SelectSingleNode("SELECTIVIDAD").InnerText),
                            //n_nit = unContenedor.SelectSingleNode("N_NIT").InnerText.Replace("-", "").ToUpper(),
                            b_siduneawd = b_sidu,
                            n_nit = "NULL",
                            s_consignatario = "NULL",
                            s_descripcion = "NULL",
                            n_BL = "NULL"
                        };

                        _resultP = EstadosDeclaDAL.Insertar(estadoDe);
                    }
                    else
                    {
                        if (b_transito == 0)
                        {
                            EstadosDecla estadoDe = new EstadosDecla
                            {
                                IdRegAduana = -1,
                                c_aduana = unContenedor.SelectSingleNode("KEY_ADUANA").InnerText,
                                n_manifiesto = Convert.ToInt32(unContenedor.SelectSingleNode("NUM_MANI").InnerText),
                                a_manifiesto = Convert.ToInt32(unContenedor.SelectSingleNode("YEAR_MANI").InnerText),
                                n_contenedor = unContenedor.SelectSingleNode("NUM_CONTEN").InnerText.Replace("-", "").ToUpper(),
                                a_decla = Convert.ToInt32(a_doc),
                                s_decla = Convert.ToInt32(c_doc),
                                c_decla = Convert.ToInt32(n_doc),
                                IdEstado = Convert.ToInt32(unContenedor.SelectSingleNode("ESTADO_ADUANA").InnerText),
                                f_reg_aduana = Convert.ToDateTime(unContenedor.SelectSingleNode("FEC_REG_ESTADO").InnerText),
                                IdSelectividad = Convert.ToInt32(unContenedor.SelectSingleNode("SELECTIVIDAD").InnerText),
                                n_nit = unContenedor.SelectSingleNode("N_NIT").InnerText.Replace("-", "").ToUpper(),
                                b_siduneawd = b_sidu,
                                s_consignatario = unContenedor.SelectSingleNode("CONSIGNATARIO").InnerText.ToUpper(),
                                n_BL = unContenedor.SelectSingleNode("BL").InnerText.ToUpper(),
                                s_descripcion = unContenedor.SelectSingleNode("DESCRIPCION").InnerText.ToUpper()
                            };

                            _resultP = EstadosDeclaDAL.Insertar(estadoDe);
                        }
                        else
                        {
                            EstadosDecla estadoDe = new EstadosDecla
                            {
                                IdRegAduana = -1,
                                c_aduana = unContenedor.SelectSingleNode("KEY_ADUANA").InnerText,
                                n_manifiesto = Convert.ToInt32(unContenedor.SelectSingleNode("NUM_MANI").InnerText),
                                a_manifiesto = Convert.ToInt32(unContenedor.SelectSingleNode("YEAR_MANI").InnerText),
                                n_contenedor = unContenedor.SelectSingleNode("NUM_CONTEN").InnerText.Replace("-", "").ToUpper(),
                                a_transito = Convert.ToInt32(a_doc),
                                r_transito = Convert.ToString(c_doc),
                                IdEstado = Convert.ToInt32(unContenedor.SelectSingleNode("ESTADO_ADUANA").InnerText),
                                f_reg_aduana = Convert.ToDateTime(unContenedor.SelectSingleNode("FEC_REG_ESTADO").InnerText),
                                IdSelectividad = Convert.ToInt32(unContenedor.SelectSingleNode("SELECTIVIDAD").InnerText),
                                n_nit = unContenedor.SelectSingleNode("N_NIT").InnerText.Replace("-", "").ToUpper(),
                                b_siduneawd = b_sidu,
                                s_consignatario = unContenedor.SelectSingleNode("CONSIGNATARIO").InnerText.ToUpper(),
                                n_BL = unContenedor.SelectSingleNode("BL").InnerText.ToUpper(),
                                s_descripcion = unContenedor.SelectSingleNode("DESCRIPCION").InnerText.ToUpper()
                            };

                            _resultP = EstadosDeclaDAL.Insertar(estadoDe);
                        }

                    }



                    int _result = Convert.ToInt32(_resultP);

                    if (_result == 0)
                        _respuesta = "<MSG>1| Código de ADUANA incorrecto</MSG>";
                    else if (_result == 1)
                        _respuesta = "<MSG>1| Problemas en número de contenedor no cumple con los estandares internacionales</MSG>";
                    else if (_result == 2)
                        _respuesta = "<MSG>1| Año de la declaracion incorrecto</MSG>";
                    else if (_result == 3)
                        _respuesta = "<MSG>0| Registrado satisfactoriamente</MSG>";
                    else if (_result == 4)
                        _respuesta = "<MSG>1| Verificar Estado declaración y selectividad</MSG>";
                    else if (_result == 5)
                        _respuesta = "<MSG>1| El número de NIT no es válido</MSG>";
                }

            }




            return _respuesta;
        }


        [WebMethod(Description = "Metodo para evaluar si un contenedor esta solicitado por la DAN")]
        public string validDAN(string xmlDoc)
        {
            string _respuesta = null;

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xmlDoc);

            XmlNodeList listaCntres = doc.SelectNodes("Cnts/DAN");

            XmlNode unContenedor;

            if (listaCntres.Count > 0)
            {
                for (int i = 0; i < listaCntres.Count; i++)
                {
                    string c_llegada = null, n_contenedor = null;
                    unContenedor = listaCntres.Item(i);

                    n_contenedor = unContenedor.SelectSingleNode("NUM_CONTEN").InnerText.Replace("-", "").ToUpper();
                    c_llegada = unContenedor.SelectSingleNode("C_LLEGADA").InnerText.Replace("-", "").ToUpper();
                                      

                    string _resultP = EstadosDeclaDAL.ConsulDAN(c_llegada, n_contenedor);

                    int _result = Convert.ToInt32(_resultP);

                    if (_result == 0)
                        _respuesta = "<MSG>0| Contenedor no posee retencion</MSG>";
                    else if (_result == 1)
                        _respuesta = "<MSG>1| Contenedor retenido por la DAN</MSG>";                    
                }

            }




            return _respuesta;
        }

    }
}