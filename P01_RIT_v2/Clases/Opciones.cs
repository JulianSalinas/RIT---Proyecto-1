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
        public string RutaColeccion;
        public string RutaArchivos;
        public string RutaConsultas;
        public string Prefijo;

        public static Opciones Instance
        {
            get
            {
                if ( instance == null )
                    instance = new Opciones();
                return instance;
            }
        }

        protected Opciones() {
            RutaOpciones = Path.GetFullPath("...\\...\\Recursos\\");
            RutaStopWords = RutaOpciones += "Stopwords.xml";
            RutaColeccion = Path.GetFullPath("...\\...\\Coleccion\\");
            RutaArchivos = Path.GetFullPath("...\\...\\Archivos\\");
            RutaConsultas = Path.GetFullPath("...\\...\\Archivos\\");
            Prefijo = "PRE-1";
            
        }


        public void guardarOpciones() {
            XmlWriter writer = XmlWriter.Create(RutaOpciones + "Opciones.xml");
            writer.WriteStartElement("Opciones");
            writer.WriteElementString("Prefijo", Prefijo);
            writer.WriteElementString("RutaConsultas", RutaConsultas);
            writer.WriteElementString("RutaColeccion", RutaColeccion);
            writer.WriteElementString("RutaArchivos", RutaArchivos);
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
        }

        public override string ToString() {
            string str = "";
            str += RutaOpciones + "\n";
            str += RutaStopWords + "\n";
            str += RutaColeccion + "\n";
            str += RutaArchivos + "\n";
            str += RutaConsultas + "\n";
            str += Prefijo + "\n";
            return str;
        }



    }
}
