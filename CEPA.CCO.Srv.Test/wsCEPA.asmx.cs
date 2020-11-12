using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;
using System.Xml;

namespace CEPA.CCO.Srv.Test
{
    /// <summary>
    /// Descripción breve de Service1
    /// </summary>
    [WebService(Namespace = "http://190.86.214.193:6046/", Description = "Servicio de Transferencia de Informacion ADUANA - CEPA")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class wsCEPA : System.Web.Services.WebService
    {


        [WebMethod(Description = "Metodo para registrar los estados de las declaraciones transmitido por ADUANA")]
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

                        _resultP = EstadosDeclaDAL.InsertarTest(estadoDe);
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

                            _resultP = EstadosDeclaDAL.InsertarTest(estadoDe);
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

                            _resultP = EstadosDeclaDAL.InsertarTest(estadoDe);
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

        [WebMethod(Description = "Metodo para registrar las deconsolidaciones realizadas a un contenedor")]
        public string InsertarBLs(string xmlDoc)
        {
            string _respuesta = null;
            int _resulDeta = 0;

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xmlDoc);

            XmlNodeList encaMani = doc.SelectNodes("MdsParts/MDS4");

            XmlNode unMani;

            List<DetaDoc> lstDoc = new List<DetaDoc>();
            DetaDoc pDoc = new DetaDoc();

            if (encaMani.Count > 0)
            {
                for (int i = 0; i < encaMani.Count; i++)
                {

                    unMani = encaMani.Item(i);

                    pDoc.a_manif = unMani.SelectSingleNode("CAR_REG_YEAR").InnerText.Trim();
                    pDoc.num_manif = Convert.ToInt32(unMani.SelectSingleNode("CAR_REG_NBER").InnerText.Trim());

                    break;

                }
            }



            XmlNodeList listaCntres = doc.SelectNodes("MdsParts/MDS4/MDS5");

            XmlNode unContenedor;

            List<ArchivoAduanaValid> pGuarda = new List<ArchivoAduanaValid>();

            if (listaCntres.Count > 0)
            {
                for (int i = 0; i < listaCntres.Count; i++)
                {
                    unContenedor = listaCntres.Item(i);

                    string _contenedor = unContenedor.SelectSingleNode("CAR_CTN_IDENT").InnerText;

                    ArchivoAduanaValid validAduana = new ArchivoAduanaValid
                    {
                        IdValid = -1,
                        n_contenedor = unContenedor.SelectSingleNode("CAR_CTN_IDENT").InnerText.Replace(" -", "").Replace(" ", ""),
                        n_manifiesto = pDoc.num_manif,
                        n_BL = unContenedor.SelectSingleNode("KEY_BOL_REF").InnerText.Trim(),
                        n_BL_master = unContenedor.SelectSingleNode("KEY_BOL_REF_MST").InnerText.Trim(),
                        a_mani = pDoc.a_manif,
                        c_tipo_bl = unContenedor.SelectSingleNode("CARBOL_TYP_COD").InnerText.Trim(),
                        b_sidunea = 1,
                        c_tamaño = unContenedor.SelectSingleNode("CAR_CTN_TYP").InnerText.Trim(),
                        s_agencia = unContenedor.SelectSingleNode("CAR_CAR_NAM").InnerText.Trim(),
                        v_peso = Convert.ToDouble(unContenedor.SelectSingleNode("CAR_CTN_GWG").InnerText.Trim()),
                        c_paquete = Convert.ToInt32(unContenedor.SelectSingleNode("CAR_CTN_NBR").InnerText.Trim()),
                        c_embalaje = unContenedor.SelectSingleNode("CARBOL_PCK_COD").InnerText.Trim(),
                        d_embalaje = unContenedor.SelectSingleNode("CARBOL_PCK_NAM").InnerText.Trim(),
                        c_pais_origen = unContenedor.SelectSingleNode("CARBOL_DEP_COD").InnerText.Substring(0, 2).Trim(),
                        d_puerto_origen = unContenedor.SelectSingleNode("CARBOL_DEP_COD").InnerText.Substring(2, 3).Trim(),
                        c_pais_destino = unContenedor.SelectSingleNode("CARBOL_DEST_COD").InnerText.Substring(0, 2).Trim(),
                        d_puerto_destino = unContenedor.SelectSingleNode("CARBOL_DEST_COD").InnerText.Substring(2, 3).Trim(),
                        s_nit = unContenedor.SelectSingleNode("CARBOL_CON_COD") != null ? unContenedor.SelectSingleNode("CARBOL_CON_COD").InnerText.Trim() : "",
                        s_consignatario = unContenedor.SelectSingleNode("CARBOL_CON_NAM") != null ? unContenedor.SelectSingleNode("CARBOL_CON_NAM").InnerText.Trim() : ""
                    };

                    //Almacenar manifiesto devuelto por aduana
                    _resulDeta = Convert.ToInt32(DetaNavieraDAL.AlmacenarValidMst(validAduana, DBComun.Estado.verdadero));

                    if (_resulDeta == 0)
                        _respuesta = "<MSG>1| Desconsolidación NO registrada</MSG>";
                    else if (_resulDeta == 1)
                        _respuesta = "<MSG>0| Registrado satisfactoriamente</MSG>";
                    else if (_resulDeta == 2)
                        _respuesta = "<MSG>1| Contenedor no pasa estándar de validación</MSG>";


                }               


                
            }

            return _respuesta;
        }
    }
}
