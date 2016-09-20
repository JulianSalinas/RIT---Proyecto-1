using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
        /// Constructor por defecto. Necesario para serializar y deserializar objeto.
        /// </summary>
        public DocumentoRanking()
        {
            posicion = 0;
            idDocumento = -1;
            similitud = 0;
        }

        /// <summary>
        /// Constructor de objeto.
        /// </summary>
        /// <param name="posicion">Posición del documento en el escalafón.</param>
        /// <param name="idDocumento">Identificador del documento en el escalafón.</param>
        /// <param name="similitud">Índice de similitud del documento con respecto a la consulta.</param>
        public DocumentoRanking(int posicion, int idDocumento, double similitud)
        {
            this.posicion = posicion;
            this.idDocumento = idDocumento;
            this.similitud = similitud;
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
            else if (this.similitud < other.similitud){
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
