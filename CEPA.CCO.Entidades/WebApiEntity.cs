using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    class WebApiEntity
    {
    }
    public class EntLlenos
    {
        public string Recno { get; set; }
        public string Salida { get; set; }
        public string Contenedor { get; set; }
        public DateTime? Despacho { get; set; }
        public DateTime? Fec_Ent { get; set; }
        public string Tipo_Despacho { get; set; }
        public string Tarja { get; set; }
        public string Poliza { get; set; }
        public double Peso_Total { get; set; }
        public double Peso_Entregado { get; set; }
        public double Saldo { get; set; }
        public string Contenido { get; set; }
        public string Consigna { get; set; }
        public string Nom_Consigna { get; set; }
        public string Placa { get; set; }
        public string Chasis { get; set; }
        public string Motorista { get; set; }
        public string Transporte { get; set; }
        public string Destino { get; set; }
        public string N_Genset { get; set; }
        public Int64 Selectividad { get; set; }
        public string Cod_Sadfi { get; set; }
        public DateTime? Fec_Inisal { get; set; }
        public bool B_Sadfi { get; set; }
        public DateTime? Fecha_Salidap1 { get; set; }
        public DateTime? Fec_Caseta_Ing { get; set; }
        public DateTime? Fec_Caseta_Sal { get; set; }
        public DateTime? Fec_Ing_Puerta1 { get; set; }
        public DateTime? Fec_Vence_Salida { get; set; }
        public bool Mov_Prueba { get; set; }
        public string Ubica { get; set; }
        public string Observa { get; set; }
        public string Condicion { get; set; }
        public string Tamaño { get; set; }        
        public string Manifiesto { get; set; }
        public string Navcorto { get; set; }
        public string Tiempoba { get; set; }
        public string Tiempocb { get; set; }
        public string Tiempoda { get; set; }
    }

    public class EstadiaConte
    {
        public string Categoria{ get; set; }
        public string Cod_agen { get; set; }
        public string Corto { get; set; }
        public string Agencia { get; set; }
        public string Manifiesto { get; set; }
        public string Contenedor { get; set; }
        public string Tipo { get; set; }
        public string Condicion { get; set; }
        public DateTime Ingreso { get; set; }
        public DateTime Fec_vacio { get; set; }
        public Int64 Estadia { get; set; }
        public string Sitio { get; set; }
        public string Observa { get; set; }
        public string Cliente { get; set; }
        public string Estb3 { get; set; }
    }

    public class Procedure
    {
        public string NBase { get; set; }
        public string Servidor { get; set; }
        public string Procedimiento { get; set; }
        public List<Parametros> Parametros { get; set; }
    }

    public class Parametros
    {
        public string nombre { get; set; }
        public string valor { get; set; }
    }

    public class Cont_Exp
    {
        public string Contenedor { get; set; }
        public string Notificar { get; set; }
        public DateTime? Ingreso { get; set; }
        public DateTime? Exportacion { get; set; }
        public double Peso { get; set; }
        public Int64 Ndias { get; set; }
        public string Leyenda { get; set; }
    }

    public class EncProvi
    {
        public string Descripcion { get; set; }
        public double Total { get; set; }
        public string C_Llegada { get; set; }
        public string Contenedor { get; set; }
        public int IdDeta { get; set; }
    }

}
