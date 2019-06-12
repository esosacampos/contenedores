using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Web.Hosting;
using System.Web.Security;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Collections.Generic;



namespace Ldap
{
    public class LDAPRoleProvider : System.Web.Security.RoleProvider
    {


        ///*
        //* Documentación RolesProvider
        //* http://msdn2.microsoft.com/en-us/library/8fw7xh74(vs.90).aspx
        //* http://msdn2.microsoft.com/en-us/library/317sza4k(vs.90).aspx
        //*
        //* Haciendo queries contra ActiveDirectory
        //* http://technet.microsoft.com/en-us/library/aa996205.aspx
        //* http://www.rlmueller.net/ADOSearchTips.htm
        //* http://www.codeproject.com/dotnet/QueryADwithDotNet.asp
        //*/

        private static DirectoryEntry entry;
        private static string connectionString = string.Empty;
        private string nombre = string.Empty;

        /// <summary>
        /// Inicialización del proovedor.
        /// </summary>
        /// <param name=”name”>Nombre</param>
        /// <param name=”config”>Parámetros de configuración</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            connectionString = ConfigurationManager.ConnectionStrings[config["connectionStringName"]].ConnectionString;
            entry = new DirectoryEntry(connectionString);
            nombre = name;

            base.Initialize(name, config);
        }

        /// <summary>
        /// Nombre.
        /// </summary>
        public override string ApplicationName
        {

            get { return nombre; }
            set { nombre = value; }

        }

        /// <summary>
        /// Obtiene lista de usuarios cuyo nombre contiene ‘usernameToMatch’
        /// y estan en el grupo ‘roleName’
        /// </summary>
        /// <param name=”roleName”></param>
        /// <param name=”usernameToMatch”></param>
        /// <returns></returns>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {

            usernameToMatch = checkUserName(usernameToMatch);
            string[] temp = GetUsersInRole(roleName);

            List<string> nombres = new List<string>();

            foreach (string s in temp)
            {

                if (s.ToUpper().Replace(' ', '.').Contains(usernameToMatch.ToUpper()))
                    nombres.Add(s);

            }

            return nombres.ToArray();

        }

        /// <summary>
        /// Obtiene todos los grupos.
        /// </summary>
        /// <returns></returns>
        public override string[] GetAllRoles()
        {
            List<string> resultado = new List<string>();

            try
            {
                object obj = entry.NativeObject;
                using (DirectorySearcher search = new DirectorySearcher(entry))
                {
                    search.Filter = "(objectCategory=group)";
                    search.PropertiesToLoad.Add("cn");
                    using (SearchResultCollection result = search.FindAll())
                    {
                        if (result != null)
                        {
                            foreach (SearchResult sr in result)
                            {
                                // Me copio la lista de grupos.
                                foreach (string s in sr.Properties["cn"]) resultado.Add(s);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                resultado.Clear();
                // Log de excepción … 
            }
            return resultado.ToArray();
        }

        /// <summary>
        /// Obtiene los grupos a los que pertenece un usuario.
        /// </summary>
        /// <param name=”username”>Nombre de usuario.</param>
        /// <returns></returns>
        public override string[] GetRolesForUser(string username)
        {
            username = checkUserName(username);
            return GetGruposLDAP(username);
        }

        /// <summary>
        /// Obtiene los usuarios que estan en un grupo.
        /// </summary>
        /// <param name=”roleName”>Nombre de grupo.</param>
        /// <returns></returns>
        public override string[] GetUsersInRole(string roleName)
        {
            List<string> userNames = new List<string>();
            try
            {
                using (DirectorySearcher search = new DirectorySearcher(entry))
                {
                    search.Filter = String.Format("(cn={0})", roleName);
                    search.PropertiesToLoad.Add("member");
                    SearchResult result = search.FindOne();

                    if (result != null)
                    {
                        for (int i = 0; i < result.Properties["member"].Count; i++)
                        {
                            string user = (string)result.Properties["member"][i];
                            userNames.Add(extraerCN(user));
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                userNames.Clear();
                // Log de excepción … 
            }
            return userNames.ToArray();
        }

        /// <summary>
        /// Indica si un usuario pertenece a un determinado grupo.
        /// </summary>
        /// <param name=”username”>Nombre de usuario.</param>
        /// <param name=”roleName”>Nombre del grupo.</param>
        /// <returns></returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            username = checkUserName(username);
            foreach (string s in GetGruposLDAP(username))
                if (s == roleName)
                    return true;
            return false;
        }

        /// <summary>
        /// Comprueba si existe un grupo.
        /// </summary>
        /// <param name=”roleName”>Nombre del grupo.</param>
        /// <returns></returns>
        public override bool RoleExists(string roleName)
        {
            foreach (string s in GetAllRoles())
                if (s == roleName)
                    return true;
            return false;

        }

        /// <summary>
        /// Extrae el CN de una cadena DN de LDAP
        /// </summary>
        /// <param name=”DN”>DN</param>
        /// <returns></returns>
        private string extraerCN(string DN)
        {
            string[] temp = DN.Split(',');
            foreach (string s in temp)
                if (s.Trim().Substring(0, 2) == "CN")
                    return s.Replace("CN=", "").Trim();
            return string.Empty;
        }

        /// <summary>
        /// Extrae el nombre de usuario de la
        /// composición ‘usuario@dominio’.
        /// </summary>
        /// <param name=”username”>username@domain</param>
        /// <returns></returns>
        private string checkUserName(string username)
        {
            if (username.Contains("@"))
            {
                return username.Split('@')[0];
            }
            else
                return username;
        }

        /// <summary>
        /// Obtiene los grupos a los que pertenece un usuario.
        /// </summary>
        /// <param name=”username”>Nombre de usuario (sin dominio)</param>
        /// <returns></returns>
        public string[] GetGruposLDAP(string username)
        {
            List<string> resultado = new List<string>();
            try
            {
                object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("memberOf");
                SearchResult result = search.FindOne();

                if (result != null)
                {
                    // Me copio la lista de grupos.
                    string[] tr = new string[result.Properties["memberOf"].Count];
                    result.Properties["memberOf"].CopyTo(tr, 0);
                    foreach (string s in result.Properties["memberOf"])
                    {
                        resultado.Add(extraerCN(s));
                    }
                }
            }
            catch (Exception)
            {
                resultado.Clear();
                // Log de excepción … 
            }
            return resultado.ToArray();
        }

        #region Métodos no implementados por motivos de permisos
        /// <summary>
        /// Añadir usuarios. EXCEPCION.
        /// </summary>
        /// <param name=”usernames”></param>
        /// <param name=”roleNames”></param>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {

            throw new Exception("No puedes añadir usuarios/grupos al ActiveDirectory.");

        }

        /// <summary>
        /// Crear grupo. EXCEPCION.
        /// </summary>
        /// <param name=”roleName”></param>
        public override void CreateRole(string roleName)
        {

            throw new Exception("No puedes crear grupos en el ActiveDirectory.");

        }

        /// <summary>
        /// Borrar grupo. EXCEPCION.
        /// </summary>
        /// <param name=”roleName”></param>
        /// <param name=”throwOnPopulatedRole”></param>
        /// <returns></returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {

            throw new Exception("No puedes borrar grupos del ActiveDirectory.");

        }

        /// <summary>
        /// Desasignar usuario-rol. EXCEPCION.
        /// </summary>
        /// <param name=”usernames”></param>
        /// <param name=”roleNames”></param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {

            throw new Exception("No puedes desasignar usuarios de grupos.");

        }

        #endregion

    }
}
