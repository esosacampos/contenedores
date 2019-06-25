# PROYECTO CEPA - CONTENEDORES
CEPA - Contenedores --> https://acajutlaweb.cepa.gob.sv:6500 --> http://contenedores.acajutlaweb.site 

## Introducción
Este proyecto nace en el 2013, bajo la perspectiva que fuera una plataforma para que agencias navieras, pudieran cargar listados de contenedores de importación y generar estos listados bajos estructuras solicitadas por el departamento de operaciones de CEPA - Acajutla, para facilitar el proceso de descarga de contenedores y apuntando a agilizar el tiempo de respuesta.

Bajo este inicio, surge la necesidad de ir consultando información ya registrada a traves de la carga de listados, es ahí, como se convierte en una herramienta importante en el uso de las operaciones de contenedores por ahorita solo se tiene casi terminado el proceso de importación de contenedores, falta cerrar con la exportación este repositorio sera el punto donse se registraran cada uno de los cambios de este proyecto, y ayudara a controlar los cambios así como retroceder a un punto de funcionamiento anterior.


## Pre - Requisitos
Para esta aplicación web, poder editar su código fuente se necesta lo siguiente:
  * Visual Studio (.NET Framework 4) Lenguaje C#
  * Sql Server 2014 (Conectores)
  * Encontrarse en la red interna de la empresa
  * Conector de SyBase se recomienda [ASE Suite](https://www.sap.com/developer/trials-downloads/additional-downloads/sdk-for-adaptive-server-enterprise-16-0-sp02-pl07-14520.html), esto para enlazar con la base central.

## Versiones

* v3.1.1 : Se publico la gestión de usuarios para la administración de usuarios al portal web.

* v2.1.1 : Se agregó la opción de impresión de tarjeta de marcación seleccionada en formato PDF.

* v2.1.0 : Se agregó que usuarios sin cargo de encargado de personal, pudieran acceder al sitio a solicitud de jefe inmediato, así como la generación de menú según el usuario logueado, existiendo los perfiles de Admin y Consulta.

* v2.0.0 : Se agregó un detalle de licencia en la tabla de marcaciones donde hace referencia a la justificación presentada en la fecha consultada, así como poder consultar en un mes en específico. 

* v1.2.0 : Se agregó modulo de ayuda, donde se encuentra el manual de usuario.

* v1.1.1 : Se modifico que las licencias tuvieras un detalle segun el tipo de licencia.

* v1.1.0 : Se agregó un resumen de licencias con el fin de encontrar información de forma rápida sobre acumulación de licencias.

* v1.0.0 : En esta versión se logro loguear al personal unicamente si poseían el rol de jefe dentro del sistema de administración financiera, además de proporcionar la tarjeta de marcaciones de los trabajadores a cargo del usuario logueado.

## Estructura
* CEPA.CONSULTAS : Posee una página que se usa de menú para la opción de Tracking de Contenedores Importación mostrada dentro de servicios en línea del Puerto de Acajutla en su página web.

* CEPA.Portales : Posee una página que se usa de menú de forma interna, donde se puede acceder a todas las plataformas web de CEPA- Puerto de Acajutla.

* CEPA.RRHH.DAL : Genera la librería de la lógica del negocio, donde se conecta a los diferentes repositorios de datos, para su funcionamiento, esto con el fin de centralizar el acceso a datos de la aplicación web de RRHH.

* CEPA.RRHH.Entidades : Genera la liberaría con las entidades que representan una clase del deposito de datos, conteniendo propiedades u métodos que apoyen a la abstranción, consiguiendo denotar las características esenciales de un objeto, donde se capturan sus comportamientos.

* CEPA.RRHH.UI.Web : Posee la presentación grafica del proyecto, de esa manera logrando que el usuario pueda interactuar con las opciones de consulta que cuenta.

## Arquitectura del Proyecto.
La aplicación web se encuentra hecha en ASP.NET C#, bajo enfoque N-Layer, POO, utilizando Bootstrap para su diseño, JQuery para su funcionamiento, lenguaje C# para la interacción, su validación de autenticación la realiza bajo LDAP y tabla con registro de usuarios admitidos fuera del cargo de jefe.

## Autor.

Elsa Beatriz Sosa Campos
>Analista de Sistemas [CEPA - Puerto de Acajutla](http://www.cepa.gob.sv/)


## Licenciamiento
Este proyecto está bajo la Licencia EULA, y esta regulada bajo la Sección de Informatica CEPA - Acajutla, y su copyright queda bajo administración de la misma.
