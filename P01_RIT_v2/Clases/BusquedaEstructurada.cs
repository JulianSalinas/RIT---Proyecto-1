using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace P01_RIT_v2.Clases
{ 
    [Serializable]
    public class BusquedaEstructurada
    {
        /// <summary>
        /// Fecha y hora de la consulta estructurada.
        /// </summary>
        private DateTime fechaHoraConsultaEstructurada;
        public DateTime FechaHoraConsultaEstructurada
        {
            get { return fechaHoraConsultaEstructurada; }
            set { fechaHoraConsultaEstructurada = value; }
        }

        /// <summary>
        /// Ruta del archivo con el escalafón de la consulta vectorial utilizada.
        /// </summary>
        private string rutaConsultaVectorial;
        public string RutaConsultaVectorial
        {
            get { return rutaConsultaVectorial; }
            set { rutaConsultaVectorial = value; }
        }

        /// <summary>
        /// Lista de cláusulas de la consulta estructurada.
        /// </summary>
        private List<ClausulaEstructurada> clausulasConsulta;
        [XmlArray("ListaClausulas")]
        public List<ClausulaEstructurada> ClausulasConsulta
        {
            get { return clausulasConsulta; }
            set { clausulasConsulta = value; }
        }

        /// <summary>
        /// Fecha y hora de la consulta vectorial utilizada.
        /// </summary>
        private DateTime fechaHoraConsultaVectorial;
        public DateTime FechaHoraConsultaVectorial
        {
            get { return fechaHoraConsultaEstructurada; }
            set { fechaHoraConsultaEstructurada = value; }
        }

        /// <summary>
        /// Ruta de la colección de documentos del escalafón vectorial utilizado.
        /// </summary>
        private string rutaColeccionDocumentos;
        public string RutaColeccionDocumentos
        {
            get { return rutaColeccionDocumentos; }
            set { rutaColeccionDocumentos = value; }
        }

        /// <summary>
        /// Consulta vectorial en bruto del escalafón vectorial utilizado.
        /// </summary>
        private string consultaVectorial;
        public string ConsultaVectorial
        {
            get { return consultaVectorial; }
            set { consultaVectorial = value; }
        }

        /// <summary>
        /// Ranking de documentos utilizados para la consulta estructurada.
        /// </summary>
        private List<RankingDocumento> rankingDocumentos;
        public List<RankingDocumento> RankingDocumentos
        {
            get { return rankingDocumentos; }
            set { rankingDocumentos = value; }
        }


        public BusquedaEstructurada()
        {
            fechaHoraConsultaEstructurada = System.DateTime.Now;
            rutaConsultaVectorial = "";
            clausulasConsulta = new List<ClausulaEstructurada>();
            fechaHoraConsultaVectorial = System.DateTime.Now;
            rutaColeccionDocumentos = "";
            consultaVectorial = "";
        }

        public BusquedaEstructurada(string rutaArchivoConsultaVectorial, string consultaEstructurada)
        {
            fechaHoraConsultaEstructurada = System.DateTime.Now;
            importarConsultaVectorial(rutaArchivoConsultaVectorial);

            procesarConsulta(consultaEstructurada);
        }

        private void importarConsultaVectorial(string rutaArchivo)
        {
            BusquedaVectorial consultaImportada = BusquedaVectorial.importarDesdeXml(rutaArchivo, true);

            fechaHoraConsultaVectorial = consultaImportada.FechaHoraBusqueda;
            rutaColeccionDocumentos = consultaImportada.RutaDocumentos;
            consultaVectorial = consultaImportada.Consulta;

            rankingDocumentos = new List<RankingDocumento>(consultaImportada.RankingDocumentos);
        }


        private void procesarConsulta(string consulta)
        {
           // PROCESAR TÉRMINOS CONSULTA ESTRUCTURADA.
        }
    }

    /// <summary>
    /// Clase para almacenar consulta estructurada.
    /// </summary>
    [Serializable]
    public class ClausulaEstructurada
    {
        private string consulta;
        public string Consulta
        {
            get { return consulta; }
            set { consulta = value; }
        }

        ClausulaEstructurada(string consulta)
        {
            this.consulta = consulta;
        }
    }
}
