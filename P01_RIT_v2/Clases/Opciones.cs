using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace P01_RIT_v2.Clases
{

    /*Clase para obtener y cambiar la direcciones de los diferentes archivos*/
    /*Sirve tambien para cargar las ultimas rutas usadas por medio de un XML*/

    public class Opciones
    {

        private static Opciones instance;
        public string RutaOpciones;
        public string RutaStopWords;
        public string RutaColeccion;
        public string RutaArchivos;
        public string RutaConsultas;
        public string Prefijo;

        public static Opciones Instance{
            get{
                if ( instance == null )
                    instance = new Opciones();
                return instance;
            }
        }

        protected Opciones() {
            RutaOpciones = "...\\...\\Recursos\\";
            RutaStopWords = "...\\...\\Recursos\\Stopwords.xml";
            /*RutaColeccion = "...\\...\\Coleccion\\";
            RutaArchivos = "...\\...\\Archivos\\";
            RutaConsultas = "...\\...\\Consultas\\";
            Prefijo = "PRE-1";*/
            XmlDocument docOpciones = new XmlDocument();
            docOpciones.Load(RutaOpciones + "Opciones.xml");
            XmlElement opciones = docOpciones.DocumentElement;
            XmlElement pre = (XmlElement) opciones.GetElementsByTagName("Prefijo")[0];
            XmlElement rutaC = (XmlElement) opciones.GetElementsByTagName("RutaColeccion")[0];
            XmlElement rutaA = (XmlElement) opciones.GetElementsByTagName("RutaArchivos")[0];
            XmlElement rutaCon = (XmlElement) opciones.GetElementsByTagName("RutaConsultas")[0];
            Prefijo = pre.FirstChild.Value;
            RutaColeccion = rutaC.FirstChild.Value;
            RutaArchivos = rutaA.FirstChild.Value;
            RutaConsultas = rutaCon.FirstChild.Value;
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
        }

    }
}
