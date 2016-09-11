using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace P01_RIT_v2.Clases
{
    class Stopwords
    {
        private List<string> terminos;
        private XmlDocument xmlDocument;
        private static Stopwords instance;

        protected Stopwords() {
            terminos = new List<string>();
            xmlDocument = new XmlDocument();
            xmlDocument.Load(Opciones.Instance.RutaStopWords);
            cargarTerminos();
        }

        public static Stopwords Instance{
            get{
                if ( instance == null )
                    instance = new Stopwords();
                return instance;
            }
        }

        private void cargarTerminos() {
            foreach ( XmlElement word in xmlDocument.DocumentElement.ChildNodes ) {
                string original = word.FirstChild.Value;
                string sinAcentos = quitarAcentos(original);
                terminos.Add(sinAcentos);
            }
        }

        public bool hasTerm( string term ) {
            return terminos.Exists(x => x.ToString() == term);
        }

        public string quitarAcentos( string inputString ) {
            Regex a = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex e = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex i = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex o = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex u = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            inputString = a.Replace(inputString, "a");
            inputString = e.Replace(inputString, "e");
            inputString = i.Replace(inputString, "i");
            inputString = o.Replace(inputString, "o");
            inputString = u.Replace(inputString, "u");
            return inputString;
        }

        override public string ToString() {
            string termsString = "";
            foreach ( string term in terminos ) {
                termsString += "<" + term + ">";
            }
            return termsString;
        }


    }
}
