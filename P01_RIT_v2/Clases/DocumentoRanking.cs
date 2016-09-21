using System;
using System.Collections.Generic;

namespace P01_RIT_v2.Clases
{
    /// <summary>
    /// Objeto que registra la posición de un documento dentro de una lista de ranking (escalafón).
    /// </summary>
    [Serializable]
    public class DocumentoRanking : IComparable<DocumentoRanking>, IEqualityComparer<DocumentoRanking>
    {
        /// <summary>
        /// Posición del documento.
        /// </summary>
        private int posicion;

        public int Posicion
        {
            get { return posicion; }
            set { posicion = value; }
        }

        /// <summary>
        /// Identificador del documento.
        /// </summary>
        private int idDocumento;

        private int IdDocumento
        {
            get { return idDocumento; }
            set { idDocumento = value; }
        }

        /// <summary>
        /// Índice de similitud del documento con respecto a la consulta.
        /// </summary>
        private double similitud;

        public double Similitud
        {
            get { return similitud; }
            set { similitud = value; }
        }

        /// <summary>
        /// Ruta del documento.
        /// </summary>
        private string rutaDocumento;

        public string RutaDocumento
        {
            get { return rutaDocumento; }
            set { rutaDocumento = value; }
        }

        /// <summary>
        /// Atributo "taxon-name" del documento.
        /// </summary>
        private string taxonNameDocumento;
        public string TaxonNameDocumento
        {
            get { return taxonNameDocumento; }
            set { taxonNameDocumento = value; }
        }

        /// <summary>
        /// Atributo "rank" del documento.
        /// </summary>
        private string rankDocumento;
        public string RankDocumento
        {
            get { return rankDocumento; }
            set { rankDocumento = value; }
        }

        /// <summary>
        /// Constructor por defecto. Necesario para serializar y deserializar objeto.
        /// </summary>
        public DocumentoRanking()
        {
            posicion = 0;
            idDocumento = -1;
            similitud = 0;
            rutaDocumento = "";
            taxonNameDocumento = "";
            rankDocumento = "";
        }

        /// <summary>
        /// Constructor del escalafón del documento
        /// </summary>
        /// <param name="posicion">Posición del documento dentro del Ranking</param>
        /// <param name="similitud">Índice de similitud del documento</param>
        /// <param name="documento">Información del documento</param>
        public DocumentoRanking(int posicion, double similitud, Documento documento)
        {
            this.posicion = posicion;
            this.similitud = similitud;
            try
            {
                this.rutaDocumento = documento.Ruta;
                this.taxonNameDocumento = documento.getTaxonName();
                this.rankDocumento = documento.getRank();
            }
            catch(NullReferenceException e)
            {
                throw new Exception("El documento ingresado es nulo.");
            }
        }

        /// <summary>
        /// Implementación de CompareTo para el ranking del documento.
        /// Ordena dos rankings siguiendo dos criterios: similitud (descendente) y número del documento (ascendente)
        /// </summary>
        /// <param name="other">Segundo objeto DocumentoRanking para realizar comparación.</param>
        /// <returns></returns>
        public int CompareTo(DocumentoRanking other)
        {
            if (this.similitud > other.similitud)
            {
                return -1;
            }
            else if (this.similitud < other.similitud)
            {
                return 1;
            }
            else if (this.idDocumento < other.idDocumento)
            {
                return -1;
            }
            else if (this.idDocumento > other.idDocumento)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Implementación de Equals para DocumentoRanking. Dos objetos de este tipo son iguales si tienen la misma posición y número de documento.
        /// </summary>
        /// <param name="x">Primer objeto DocumentoRanking</param>
        /// <param name="y">Segundo objeto DocumentoRanking</param>
        /// <returns>True si cumplen la condición de igualdad. False en caso contrario.</returns>
        public bool Equals(DocumentoRanking x, DocumentoRanking y)
        {
            return ((x.posicion == y.posicion) && (x.idDocumento == y.idDocumento));
        }

        /// <summary>
        /// Implementación del GetHashCode para DocumentoRanking. Obtiene el valor hash del objeto.
        /// </summary>
        /// <param name="obj">Objeto a consultar.</param>
        /// <returns>Valor hash del objeto.</returns>
        public int GetHashCode(DocumentoRanking obj)
        {
            return (obj.posicion * obj.idDocumento);
        }
    }
}