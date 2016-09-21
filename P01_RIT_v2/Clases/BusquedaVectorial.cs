using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace P01_RIT_v2.Clases
{
    [Serializable]
    public class BusquedaVectorial
    {

        /// <summary>
        /// Archivo Invertido sobre el cual se hará la consulta.
        /// </summary>
        private Invertido archivoInvertido;

        [XmlIgnore]
        public Invertido ArchivoInvertido
        {
            get { return archivoInvertido; }
        }


        private string rutaArchivoInvertido;
        public string RutaArchivoInvertido
        {
            get { return rutaArchivoInvertido; }
            set { rutaArchivoInvertido = value; }
        }

        private DateTime fechaHoraConsulta;



        /// <summary>
        /// Constructor de clase.
        /// </summary>
        /// <param name="archivoInvertido">
        /// Referencia al archivo invertido.
        /// </param>
        public BusquedaVectorial(Invertido archivoInvertido)
        {
            this.archivoInvertido = archivoInvertido;
        }


        // Realizar una búsqueda vectorial con un término, dado un peso (opcional).
        public void buscarTermino(String termino, int peso = 2) { }

        // Realizar una búsqueda vectorial con un conjunto de términos.
        // Cada término puede tener un peso opcional.
        public void buscarTerminos(String[][] terminos)
        {
            foreach (String[] termino in terminos){
                if (termino == null)
                {
                    continue;
                }
                else
                {
                    String strTermino = termino[0];
                    if (terminos.Length > 1)
                    {
                        try
                        {
                            int pesoTermino = Int32.Parse(termino[1]);
                            // buscarTermino(strTermino, pesoTermino);
                        }
                        catch (Exception)
                        {
                            // Consulta recibida tiene un valor de peso inadecuado.
                            continue;
                        }
                    }
                    else
                    {
                        //buscarTermino(strTermino);
                    }
                }
            }
        }
    }
}
