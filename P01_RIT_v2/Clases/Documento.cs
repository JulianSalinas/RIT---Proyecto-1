using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace P01_RIT_v2.Clases
{
    [Serializable]
    public class Documento
    {
        public int Id;
        public string Ruta;

        [XmlIgnoreAttribute]
        public List<string> Terminos;

        [XmlIgnoreAttribute]
        private XmlDocument XmlDocument;

        public Documento()
        {
            Id = 0;
            Ruta = "";
            Terminos = null;
            XmlDocument = null;
        }

        public Documento( int id, string ruta) {
            this.Id = id;
            this.Ruta = ruta;
            Terminos = new List<string>();
            XmlDocument = null;
        }

        public int countTermino( string term ) {
            return Terminos.FindAll(x => x.ToString() == term).Count;
        }

        public bool hasTermino( string term ) {
            return Terminos.Exists(x => x.ToString() == term);
        }

        public void cargarXmlDocument() {
            XmlDocument = new XmlDocument();
            XmlDocument.Load(Ruta);
        }

        private string getTaxonDescription() {
            XmlElement treatment = XmlDocument.DocumentElement;
            XmlElement description = (XmlElement) treatment.GetElementsByTagName("description")[0];
            return description.GetAttributeNode("taxon_description").Value;
        }

        private string getTaxonName() {
            XmlElement treatment = XmlDocument.DocumentElement;
            XmlElement description = (XmlElement) treatment.GetElementsByTagName("taxon_identification")[0];
            return description.GetAttributeNode("taxon_name").Value;
        }

        private string getRank() {
            XmlElement treatment = XmlDocument.DocumentElement;
            XmlElement description = (XmlElement) treatment.GetElementsByTagName("taxon_identification")[0];
            return description.GetAttributeNode("rank").Value;
        }

        public List<string> getTerminos() {

            cargarXmlDocument();

            /*Patron para obtener todos los numeros incluso los que tienen decimales*/
            string numPattern = @"(\d)?(\d|,)*\.?\d";
            Regex numRegex = new Regex(numPattern, RegexOptions.Compiled);

            /*Patron para eliminar los signos de puntuacion*/
            string clearText = @"[^a-zA-Z0-9 ]|(\d)?(\d|,)*\.?\d";
            Regex clearRegex = new Regex(clearText, RegexOptions.Compiled);

            /*Se agregar los terminos del TaxonName y el Rank*/
            string tNToLower = getTaxonName().ToLower();
            string tNQuitAccents = Stopwords.Instance.quitarAcentos(tNToLower);
            string tNClearText = clearRegex.Replace(tNQuitAccents, "");
            List<string> tNWords = tNClearText.Split(' ').ToList();


            string tNTermino = tNWords.ElementAt(0);
            if (tNWords.Count > 1)
            {
                tNTermino += ' ' + tNWords.ElementAt(1);
            }

            Terminos.Add(tNTermino);
            Terminos.Add(getRank().ToLower());

            /*Se agregan los terminos pertenecientes al Taxon description*/
            string tDToLower = getTaxonDescription().ToLower();
            string tDQuitAccents = Stopwords.Instance.quitarAcentos(tDToLower);

            foreach ( Match match in numRegex.Matches(tDQuitAccents) )
                Terminos.Add(match.Value);

            string tDClearText = clearRegex.Replace(tDQuitAccents, "");
            List<string> taxonDescriptionWords = tDClearText.Split(' ').ToList();

            foreach ( string word in taxonDescriptionWords )
                if ( word != "" && ! Stopwords.Instance.hasTerm(word) )
                    Terminos.Add(word);

            return Terminos;
        }

        override public string ToString() {
            string termsString = "";
            termsString += "IDDocumento: " + Id.ToString() + "\n";
            termsString += "Ruta: " + Ruta + "\n";
            termsString += "Términos: \n";
            foreach ( string term in Terminos ) {
                termsString += "<" + term + ">";
            }
            return termsString;
        }


    }
}
