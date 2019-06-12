﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CEPA.CCO.Linq;
using CEPA.Tareas.UI.ASP.Controls;
using System.Globalization;
using System.Web.UI;
using System.Threading;

namespace CEPA.CCO.UI.Web
{
    /// <summary>
    /// Descripción breve de FileExport
    /// </summary>
    public class FileExport : IHttpHandler, System.Web.SessionState.IRequiresSessionState 
    {

        public void ProcessRequest(HttpContext context)
        {
            MultiFileUpload pControl = new MultiFileUpload();

            pControl.FileExtensionsEnabled = ".xls|.xlsx";

            try
            {
                //System.Threading.Thread.Sleep(3000);
                if (context.Request.Files.Count > 0)
                {
                    HttpFileCollection files = context.Request.Files;

                    if (files.Count > 0)
                    {
                        if (files.Count == 1)
                        {
                            if (pControl.ValidarExtensiones(files))
                            {
                                HttpPostedFile file = files[0];
                                string fname = context.Server.MapPath("~/Archivos/" + file.FileName);
                                context.Session["archivo"] = file.FileName;
                                file.SaveAs(fname);
                                context.Session["ruta"] = fname;


                                CargarExportacion _cargar = new CargarExportacion();
                                int aduana = Convert.ToInt32(context.Request.QueryString["aduana"].ToString());

                                if (aduana == 0)
                                {
                                    _cargar.d_buque = context.Request.QueryString["d_buque"].ToString();
                                    _cargar.c_usuario = context.Session["c_usuario"].ToString();
                                    _cargar.c_imo = context.Request.QueryString["c_imo"].ToString();
                                    _cargar.c_llegada = context.Request.QueryString["c_llegada"].ToString();
                                    _cargar.f_llegada = Convert.ToDateTime(context.Request.QueryString["f_llegada"].ToString(), CultureInfo.CreateSpecificCulture("es-SV"));
                                    _cargar.sustitucion = Convert.ToInt32(context.Request.QueryString["susti"].ToString());

                                    if (_cargar.sustitucion == 1)
                                        _cargar.arch_susti = context.Request.QueryString["arch_susti"].ToString();

                                    _cargar.AlmacenaCargaExport();
                                }
                                else if (aduana == 1)
                                {
                                    _cargar.d_buque = context.Request.QueryString["d_buque"].ToString();
                                    _cargar.c_usuario = context.Session["c_usuario"].ToString();
                                    _cargar.n_manifiesto = Convert.ToInt32(context.Request.QueryString["n_manif"].ToString());
                                    _cargar.IdRegC = Convert.ToInt32(context.Request.QueryString["IdRegC"].ToString());
                                    _cargar.IdDocC = Convert.ToInt32(context.Request.QueryString["IddocC"].ToString());
                                    _cargar.a_manip = context.Request.QueryString["a_maniC"].ToString();


                                    //_cargar.AlmacenarAduana();
                                }
                                //context.Response.ContentType = "text/plain";
                                //context.Response.Write("Carga Exitosa!");

                            }
                        }


                    }
                }
                else
                {
                    context.Response.Write("Cargar solo un listado");

                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }
            finally
            {
                HttpContext.Current.Session["listaValid"] = null;
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}