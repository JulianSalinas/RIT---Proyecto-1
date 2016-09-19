using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_RIT_v2.Clases
{
    public class BusquedaVectorial
    {
        Invertido archivoInvertido;

        public Invertido ArchivoInvertido
        {
            get { return archivoInvertido; }
        }


        // Constructor para realizar búsquedas vectoriales.
        // Utiliza un archivo invertido creado anteriormente.
        public BusquedaVectorial(Invertido archivoInvertido)
        {
            this.archivoInvertido = archivoInvertido;
        }


        // Constructor para realizar búsquedas vectoriales.
        // Abre el XML con el archivo invertido y lo crea en el momento.
        public BusquedaVectorial(String rutaArchivoInvertido)
        {
            this.archivoInvertido = Invertido.importarArchivoInvertido(rutaArchivoInvertido);
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
