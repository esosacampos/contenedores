using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
   
    public class Menu
    {
        public int IdPerfil { get; set; }
        public int MenuId { get; set; }
        public string NombreMenu { get; set; }
        public string DescripcionMenu { get; set; }
        public int PadreId { get; set; }
        public int Posicion { get; set; }
        public string Icono { get; set; }
        public bool Habilitado { get; set; }
        public string Url { get; set; }
        public string FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }

    public class MenuUsuarios
    {
        public string Usuario { get; set; }
        public string Habilitado { get; set; }
    }

    public class Perfil
    {
        public int IdPerfil { get; set; }
        public string NombrePerfil { get; set; }
        public string Habilitado { get; set; }
    }

    public class PerfilMenu
    {
        public int IdPerfilMenu { get; set; }
        public int IdPerfil { get; set; }
        public int MenuId { get; set; }
        public bool Habilitado { get; set; }
    }

    public class Usuarios
    {
        public string c_id_usuario { get; set; }
        public string s_nombre { get; set; }
        public string c_cen_cos { get; set; }
        public string Habilitado { get; set; }

        public int IdReg { get; set; }
        public string c_usuario { get; set; }
        public string d_usuario { get; set; }
        public string c_naviera { get; set; }
        public string c_mail { get; set; }
        public string c_iso_navi { get; set; }
        public string c_navi_corto { get; set; }

    }

    public class PerfilUsuario
    {
        public int IdPerfilUsuario { get; set; }
        public int IdPerfil { get; set; }
        public string IdUser { get; set; }
        public string Habilitado { get; set; }

    }

    public class ProcesoRelacionado
    {
        #region "Propiedades Utilizadas"
        /// <summary>
        /// Propiedad utilizada para agregar o recuperar el código
        /// del proceso relacionado
        /// </summary>
        public int IdProceso { get; set; }
        /// <summary>
        /// Propiedad utilizada para agregar o recuperar 
        /// la descripción del proceso relacionado
        /// </summary>
        public string Descripcion { get; set; }
        /// <summary>
        /// Propiedad utilizada para agregar o recuperar 
        /// el estado del proceso
        /// </summary>
        public string Estado { get; set; }
        /// <summary>
        /// Propiedad utilizada para recuperar el código 
        /// de la empresa 
        /// </summary>
        public string IdEmpresa { get; set; }
        /// <summary>
        /// Propiedad utilizada para recuperar el código
        /// del centro de costos relacionado
        /// </summary>
        public string IdCentroCostos { get; set; }

        public bool bEstado { get; set; }
        #endregion
    }
    
}
