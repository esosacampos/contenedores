using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CEPA.CCO.Linq;
using System.Globalization;
using System.Web.UI;
using System.Threading;
using CEPA.Tareas.UI.ASP.Controls;


namespace CEPA.CCO.UI.Web.Bodega
{
    /// <summary>
    /// Summary description for FileUploadHandler
    /// </summary>
    public class FileUploadHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState 
    {

        public void ProcessRequest(HttpContext context)
        {
            MultiFileUpload pControl = new MultiFileUpload();
            wfSolicitud pCargar = new wfSolicitud();
            pControl.FileExtensionsEnabled = ".pdf|.jpg|.jpeg|.png";
            
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
                                string fname = context.Server.MapPath("~/Solicitudes/" + file.FileName);
                                context.Session["archivo"] = file.FileName;
                                file.SaveAs(fname);
                                context.Session["ruta"] = fname;

                                context.Response.ContentType = "text/plain";
                                context.Response.Write("Carga Exitosa!");

                            }
                        }
                    }
                }
                else
                {
                    context.Response.Write("No cargar más de un archivo");

                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);

            }
            finally
            {
                
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