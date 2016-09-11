using System;
using System.Xml;

namespace P01_RIT_v2.Clases
{

    class Opciones
    {

        private static Opciones instance;
        public string RutaOpciones;
        public string RutaColeccion;
        public string RutaStopWords;
        public string RutaArchivos;

        protected Opciones() {
            RutaOpciones = Environment.SpecialFolder.MyDocuments + "\\RIT\\Opciones.xml";
            RutaColeccion = Environment.SpecialFolder.MyDocuments + "\\RIT\\Coleccion";
            RutaStopWords = Environment.SpecialFolder.MyDocuments + "\\RIT\\Stopwords.xml";
            RutaArchivos = Environment.SpecialFolder.MyDocuments + "\\RIT\\Archivos";
        }

        public static Opciones Instance{
            get{
                if ( instance == null )
                    instance = new Opciones();
                return instance;
            }
        }

        public void guardarOpciones() {
            try {
                XmlDocument opciones = new XmlDocument();
            }
            catch(Exception) {

            }
        }

        public void cargarOpciones() {
            try {

            }
            catch ( Exception ) {

            }
        }


    }
}
