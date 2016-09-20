using System;
using System.IO;
using System.Xml;

namespace P01_RIT_v2.Clases
{

    /*Clase para obtener y cambiar la direcciones de los diferentes archivos*/
    /*Sirve tambien para cargar las ultimas rutas usadas por medio de un XML*/


    class Opciones
    {

        private static Opciones instance;
        public string RutaOpciones;
        public string RutaStopWords;
        public string RutaColeccion;  // Carpeta con archivos XML de colección.   
        public string RutaArchivos;	  // Carpeta con archivos invertidos.
        public string Prefijo;

        public static Opciones Instance{
            get{
                if ( instance == null )
                    instance = new Opciones();
                return instance;
            }
        }

        /*Lee las rutas del XML opciones*/

        protected Opciones() {
            Prefijo = "PRE-1";
            RutaOpciones = "...\\...\\Recursos\\Opciones.xml";
            RutaStopWords = "...\\...\\Recursos\\Stopwords.xml";
            RutaColeccion = "...\\...\\Coleccion\\";
            RutaArchivos = "...\\...\\Archivos\\";
        }


        override public string ToString() {
            string str = "Ruta de opciones: " + RutaOpciones + "\n";
            str += "Ruta de archivos: " + RutaArchivos + "\n";
            str += "Ruta de coleccion: " + RutaColeccion + "\n";
            str += "Ruta de stopwords: " + RutaStopWords + "\n";
            return str;
        }
      


    }
}
