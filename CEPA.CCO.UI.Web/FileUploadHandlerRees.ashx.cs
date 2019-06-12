using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CEPA.Tareas.UI.ASP.Controls;
using CEPA.CCO.Linq;
using System.Globalization;
using System.Web.UI;
using System.Threading;

namespace CEPA.CCO.UI.Web
{
    /// <summary>
    /// Summary description for FileUploadHandler
    /// </summary>
    public class FileUploadHandlerRees : IHttpHandler, System.Web.SessionState.IRequiresSessionState 
    {

        public void ProcessRequest(HttpContext context)
        {
            MultiFileUpload pControl = new MultiFileUpload();
            wfCargar pCargar = new wfCargar();
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
                                

                                CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
                                
                                _cargar.c_usuario = context.Session["c_usuario"].ToString();



                                _cargar.AlmacenarBooking();
                               
                                context.Response.ContentType = "text/plain";
                                context.Response.Write("Carga Exitosa!");

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
                HttpContext.Current.Session["listaBooking"] = null;
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