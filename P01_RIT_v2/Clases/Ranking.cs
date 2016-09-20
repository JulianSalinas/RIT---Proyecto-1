using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_RIT_v2.Clases
{
    /// <summary>
    /// Registro de Ranking (Escalafón)
    /// </summary>
    [Serializable]
    public class Ranking
    {
        /// <summary>
        /// Fecha de creación del ranking.
        /// </summary>
        private DateTime fechaCreacion;
        public DateTime FechaCreacion
        {
            get { return fechaCreacion; }
            set { fechaCreacion = value; }
        }

        /// <summary>
        /// Ruta de la colección de documentos utilizada para crear el Ranking.
        /// </summary>
        private string rutaColeccion;
        public string RutaColeccion
        {
            get { return rutaColeccion; }
            set { rutaColeccion = value; }
        }

        /// <summary>
        /// Consulta utilizada para generar el Ranking.
        /// </summary>
        private string consulta;
        public string Consulta
        {
            get { return consulta; }
            set { consulta = value; }
        }


        /// <summary>
        /// Lista de documentos del Ranking.
        /// </summary>
        private List<DocumentoRanking> documentosRanking;
        public List<DocumentoRanking> DocumentosRanking
        {
            get { return documentosRanking; }
            set { documentosRanking = value; }
        }

        /// <summary>
        /// Constructor por defecto. Necesario para la serialización.
        /// </summary>
        public Ranking(string rutaColeccion = "", string consulta = "")
        {
            this.fechaCreacion = System.DateTime.Now;
            this.rutaColeccion = rutaColeccion;
            this.consulta = consulta;
            this.documentosRanking = new List<DocumentoRanking>();
        }
    }
}
