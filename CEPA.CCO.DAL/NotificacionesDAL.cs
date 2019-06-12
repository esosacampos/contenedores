using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CEPA.CCO.Entidades;
using System.Data;
using System.Data.SqlClient;

namespace CEPA.CCO.DAL
{
    public class NotificacionesDAL
    {
        public static List<Notificaciones> ObtenerNotificaciones(string pCampo, DBComun.Estado pEstado, string pIdentifica)
        {
            List<Notificaciones> notiLista = new List<Notificaciones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT IdNotificacion, s_mail, d_mail 
                                    FROM CCO_NOTIFICACIONES WHERE {0} = 1 AND c_identifica = '{1}' AND Habilitado = 1 ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pCampo, pIdentifica), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Notificaciones _notificacion = new Notificaciones
                    {
                        IdNotificacion = (int)_reader.GetInt32(0),
                        sMail = _reader.GetString(1),
                        dMail = _reader.GetString(2)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }
            
        }

        public static List<Notificaciones> ObtenerNotificacionesCC(string pCampo, DBComun.Estado pEstado, string pIdentifica)
        {
            List<Notificaciones> notiLista = new List<Notificaciones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT IdNotificacion, s_mail, d_mail 
                                    FROM CCO_NOTIFICACIONES WHERE {0} = 1 AND c_identifica <> '{1}' AND c_identifica = '0' AND Habilitado = 1 ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pCampo, pIdentifica), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Notificaciones _notificacion = new Notificaciones
                    {
                        IdNotificacion = (int)_reader.GetInt32(0),
                        sMail = _reader.GetString(1),
                        dMail = _reader.GetString(2)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<Notificaciones> ObtenerNotificacionesCCN(string pCampo, DBComun.Estado pEstado, string pIdentifica)
        {
            List<Notificaciones> notiLista = new List<Notificaciones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT IdNotificacion, s_mail, d_mail 
                                    FROM CCO_NOTIFICACIONES WHERE {0} = 1 AND c_identifica = '{1}' AND Habilitado = 1 ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pCampo, pIdentifica), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Notificaciones _notificacion = new Notificaciones
                    {
                        IdNotificacion = (int)_reader.GetInt32(0),
                        sMail = _reader.GetString(1),
                        dMail = _reader.GetString(2)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<Notificaciones> ObtenerNotificacionesCCNC(string pCampo, DBComun.Estado pEstado, string pIdentifica)
        {
            List<Notificaciones> notiLista = new List<Notificaciones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT IdNotificacion, s_mail, d_mail 
                                    FROM CCO_NOTIFICACIONES WHERE {0} = 1 AND c_identifica <> '{1}' AND Habilitado = 1 ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pCampo, pIdentifica), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Notificaciones _notificacion = new Notificaciones
                    {
                        IdNotificacion = (int)_reader.GetInt32(0),
                        sMail = _reader.GetString(1),
                        dMail = _reader.GetString(2)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<Notificaciones> ObtenerNotificacionesAlert(string pCampo, DBComun.Estado pEstado)
        {
            List<Notificaciones> notiLista = new List<Notificaciones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT IdNotificacion, s_mail, d_mail 
                                    FROM CCO_NOTIFICACIONES WHERE {0} = 1 AND c_identifica not in ('0', '219') AND Habilitado = 1 ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pCampo), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Notificaciones _notificacion = new Notificaciones
                    {
                        IdNotificacion = (int)_reader.GetInt32(0),
                        sMail = _reader.GetString(1),
                        dMail = _reader.GetString(2)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<Notificaciones> ObtenerNotificacionesCCAlert(string pCampo, DBComun.Estado pEstado)
        {
            List<Notificaciones> notiLista = new List<Notificaciones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT IdNotificacion, s_mail, d_mail 
                                    FROM CCO_NOTIFICACIONES WHERE {0} = 1 AND c_identifica in ('0', '219') AND Habilitado = 1 ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pCampo), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Notificaciones _notificacion = new Notificaciones
                    {
                        IdNotificacion = (int)_reader.GetInt32(0),
                        sMail = _reader.GetString(1),
                        dMail = _reader.GetString(2)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<Notificaciones> ObtenerNotificacionesRestrAlert(DBComun.Estado pEstado)
        {
            List<Notificaciones> notiLista = new List<Notificaciones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT IdNotificacion, s_mail, d_mail 
                                    FROM CCO_NOTIFICACIONES WHERE b_noti_alerta = 1 and habilitado = 1 and IdNotificacion in(1, 2, 9, 24, 25)";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Notificaciones _notificacion = new Notificaciones
                    {
                        IdNotificacion = (int)_reader.GetInt32(0),
                        sMail = _reader.GetString(1),
                        dMail = _reader.GetString(2)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }
        public static List<Notificaciones> ObtenerNotiLIQUID(DBComun.Estado pEstado)
        {
            List<Notificaciones> notiLista = new List<Notificaciones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT IdNotificacion, s_mail, d_mail, c_identifica
                                    FROM CCO_NOTIFICACIONES WHERE b_noti_liquid = 1 and habilitado = 1 and c_identifica != 0";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Notificaciones _notificacion = new Notificaciones
                    {
                        IdNotificacion = (int)_reader.GetInt32(0),
                        sMail = _reader.GetString(1),
                        dMail = _reader.GetString(2)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<Notificaciones> ObtenerNotiCOTECNA(DBComun.Estado pEstado)
        {
            List<Notificaciones> notiLista = new List<Notificaciones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT IdNotificacion, s_mail, d_mail, c_identifica
                                    FROM CCO_NOTIFICACIONES WHERE b_noti_cotecna = 1 and habilitado = 1 and c_identifica != 0";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Notificaciones _notificacion = new Notificaciones
                    {
                        IdNotificacion = (int)_reader.GetInt32(0),
                        sMail = _reader.GetString(1),
                        dMail = _reader.GetString(2)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }


        public static List<Notificaciones> ObtenerAlertaParametro(DBComun.Estado pEstado)
        {
            List<Notificaciones> notiLista = new List<Notificaciones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT IdNotificacion, s_mail, d_mail, c_identifica
                                    FROM CCO_NOTIFICACIONES WHERE IdNotificacion in(1, 2, 22)";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Notificaciones _notificacion = new Notificaciones
                    {
                        IdNotificacion = (int)_reader.GetInt32(0),
                        sMail = _reader.GetString(1),
                        dMail = _reader.GetString(2)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }
    }
}
