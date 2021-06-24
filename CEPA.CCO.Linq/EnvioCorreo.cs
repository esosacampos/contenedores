using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CEPA.CCO.DAL;
using System.Net.Mail;
using CEPA.CCO.Entidades;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace CEPA.CCO.Linq
{
    public class EnvioCorreo
    {
        #region "Propiedades"
        private List<string> m_listArch = new List<string>();
        private List<string> m_Responsables = new List<string>();
        public bool Error { get; set; }

        public string Para { get; set; }
        public string Registra { get; set; }
        public string FromName { get; set; }
        public string FromMail { get; set; }
        //public string IdTarea { get; set; }
        public string Subject { get; set; }
        
        public List<string> Responsables
        {
            get { return m_Responsables; }
            set { m_Responsables = value; }
        }
        
        public string m_Asunto;

        public string Asunto
        {
            get { return m_Asunto; }
            set { m_Asunto = value; }
        }

        public List<string> ListArch
        {
            get { return m_listArch; }
            set { m_listArch = value; }
        }

        private List<Notificaciones> m_ListaNoti;

        public List<Notificaciones> ListaNoti
        {
            get { return m_ListaNoti; }
            set { m_ListaNoti = value; }
        }

        private List<Notificaciones> m_ListaCC;

        public List<Notificaciones> ListaCC
        {
            get { return m_ListaCC; }
            set { m_ListaCC = value; }
        }

        #endregion

        public void EnviarCorreo(DBComun.TipoCorreo pTipo, DBComun.Estado pEstadoC)
        {
            SmtpClient _cliente = new SmtpClient();
            MailMessage _mensaje = new MailMessage();

            _cliente.Host = DBComun.smptServer(pTipo, pEstadoC);
            _cliente.Port = Convert.ToInt32(DBComun.smtpPort(pTipo, pEstadoC));
            _cliente.EnableSsl = DBComun.smtpSSL(pTipo, pEstadoC);
            _cliente.Credentials = new System.Net.NetworkCredential(DBComun.smtpUserName(pTipo, pEstadoC), DBComun.smtpPassword(pTipo, pEstadoC));

            _mensaje.From = new System.Net.Mail.MailAddress(DBComun.fromMail(pTipo, pEstadoC), DBComun.fromName(pTipo, pEstadoC) + "<" + DBComun.fromMail(pTipo, pEstadoC) + ">");
            _mensaje.Subject = Subject;

            foreach (Notificaciones noti in ListaNoti)
            {
                _mensaje.To.Add(new System.Net.Mail.MailAddress(noti.sMail, noti.dMail));
            }

            if(ListaNoti == null || ListaNoti.Count == 0)
                _mensaje.To.Add(new System.Net.Mail.MailAddress("elsa.sosa@cepa.gob.sv", "Elsa Sosa"));

            if (ListaCC == null)
                ListaCC = new List<Notificaciones>();

            if (ListaCC.Count > 0)
            {
                foreach (Notificaciones cc in ListaCC)
                {
                    _mensaje.CC.Add(new System.Net.Mail.MailAddress(cc.sMail, cc.dMail));
                }
            }

            if (ListArch == null)
                ListArch = new List<string>();

            foreach (string item in ListArch)
            {
                _mensaje.Attachments.Add(new System.Net.Mail.Attachment(item.ToString()));
            }            

            _mensaje.IsBodyHtml = true;
            _mensaje.BodyEncoding = System.Text.Encoding.UTF8;

         



            _mensaje.Body = m_Asunto;

            try
            {
                if (pTipo != DBComun.TipoCorreo.CEPA)
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                    delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };
                }
                _cliente.Send(_mensaje);
                _mensaje.Attachments.Dispose();
                Error = false;
            }
            catch (System.Net.Mail.SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    ListaNoti.RemoveAll(x => x.sMail == ex.InnerExceptions[i].FailedRecipient.Replace("<", "").Replace(">", ""));
                }
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    ListaCC.RemoveAll(x => x.sMail == ex.InnerExceptions[i].FailedRecipient.Replace("<", "").Replace(">", ""));
                }
                EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso);
            }
            catch (System.Net.Mail.SmtpFailedRecipientException ex)
            {
                //throw new SmtpFailedRecipientException("Error en el envio de correo", ex);
                // Mandar Escribir.
                ListaNoti.RemoveAll(x => x.sMail == ex.FailedRecipient.Replace("<", "").Replace(">", ""));
                ListaCC.RemoveAll(x => x.sMail == ex.FailedRecipient.Replace("<", "").Replace(">", ""));
                EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso);

            }
            catch (System.Net.Mail.SmtpException ex)
            {
                Error = true;
                // throw new Exception(ex.Message);
                if (pTipo == DBComun.TipoCorreo.CEPA)
                    EnviarCorreo(DBComun.TipoCorreo.Gmail, pEstadoC);
                else
                    EnviarCorreo(DBComun.TipoCorreo.CEPA, pEstadoC);
            }
        }

        public void EnviarCorreo(DBComun.TipoCorreo pTipo, DBComun.Estado pEstadoC, string c_servicio)
        {
            SmtpClient _cliente = new SmtpClient();
            MailMessage _mensaje = new MailMessage();

            _cliente.Host = DBComun.smptServer(pTipo, pEstadoC);
            _cliente.Port = Convert.ToInt32(DBComun.smtpPort(pTipo, pEstadoC));
            _cliente.EnableSsl = DBComun.smtpSSL(pTipo, pEstadoC);
            _cliente.Credentials = new System.Net.NetworkCredential(DBComun.smtpUserName(pTipo, pEstadoC), DBComun.smtpPassword(pTipo, pEstadoC));

            _mensaje.From = new System.Net.Mail.MailAddress(DBComun.fromMail(pTipo, pEstadoC), DBComun.fromName(pTipo, pEstadoC) + "<" + DBComun.fromMail(pTipo, pEstadoC) + ">");
            _mensaje.Subject = Subject;

            foreach (Notificaciones noti in ListaNoti)
            {
                _mensaje.To.Add(new System.Net.Mail.MailAddress(noti.sMail, noti.dMail));
            }

            if (ListaNoti == null || ListaNoti.Count == 0)
                _mensaje.To.Add(new System.Net.Mail.MailAddress("elsa.sosa@cepa.gob.sv", "Elsa Sosa"));

            if (ListaCC == null)
                ListaCC = new List<Notificaciones>();

            if (ListaCC.Count > 0)
            {
                foreach (Notificaciones cc in ListaCC)
                {
                    _mensaje.CC.Add(new System.Net.Mail.MailAddress(cc.sMail, cc.dMail));
                }
            }

            if (ListArch == null)
                ListArch = new List<string>();

            foreach (string item in ListArch)
            {
                _mensaje.Attachments.Add(new System.Net.Mail.Attachment(item.ToString()));
            }

            _mensaje.IsBodyHtml = true;
            _mensaje.BodyEncoding = System.Text.Encoding.UTF8;


            _mensaje.Body = m_Asunto;

            try
            {
                _cliente.Send(_mensaje);
                _mensaje.Attachments.Dispose();
                Error = false;
            }
            catch (System.Net.Mail.SmtpFailedRecipientsException ex)
            {
                
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    CorreoError _correo = new CorreoError()
                    {
                        c_asunto = _mensaje.Subject,
                        c_mail = ex.InnerExceptions[i].FailedRecipient.Replace("<", "").Replace(">", "")
                    };

                    CorreoErroresDAL.Insertar(_correo);
                }

                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    ListaNoti.RemoveAll(x => x.sMail == ex.InnerExceptions[i].FailedRecipient.Replace("<", "").Replace(">", ""));
                }
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    ListaCC.RemoveAll(x => x.sMail == ex.InnerExceptions[i].FailedRecipient.Replace("<", "").Replace(">", ""));
                }
                EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso);
            }
            catch (System.Net.Mail.SmtpFailedRecipientException ex)
            {
                //throw new SmtpFailedRecipientException("Error en el envio de correo", ex);
                // Mandar Escribir.

               
                CorreoError _correo = new CorreoError()
                {
                    c_asunto = _mensaje.Subject,
                    c_mail = ex.FailedRecipient.Replace("<", "").Replace(">", "")
                };

                CorreoErroresDAL.Insertar(_correo);
                


                ListaNoti.RemoveAll(x => x.sMail == ex.FailedRecipient.Replace("<", "").Replace(">", ""));
                ListaCC.RemoveAll(x => x.sMail == ex.FailedRecipient.Replace("<", "").Replace(">", ""));
                EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso);

            }
            catch (System.Net.Mail.SmtpException ex)
            {
                Error = true;
                // throw new Exception(ex.Message);
                if (pTipo == DBComun.TipoCorreo.CEPA)
                    EnviarCorreo(DBComun.TipoCorreo.Gmail, pEstadoC);
                else
                    EnviarCorreo(DBComun.TipoCorreo.CEPA, pEstadoC);
            }
        }
    }
}
