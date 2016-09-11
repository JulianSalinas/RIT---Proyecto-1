using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace P01_RIT_v2.Clases
{
    class Documento
    {
        public int Id;
        public string Ruta;
        public List<string> Terminos;
        private XmlDocument XmlDocument;

        public Documento( int id, string ruta) {
            this.Id = id;
            this.Ruta = ruta;
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

        public string getTaxonDescription() {
            XmlElement treatment = XmlDocument.DocumentElement;
            XmlElement description = (XmlElement) treatment.GetElementsByTagName("description")[0];
            return description.GetAttributeNode("taxon_description").Value;
        }

        public string getTaxonName() {
            XmlElement treatment = XmlDocument.DocumentElement;
            XmlElement description = (XmlElement) treatment.GetElementsByTagName("taxon_identification")[0];
            return description.GetAttributeNode("taxon_name").Value;
        }

        public string getRank() {
            XmlElement treatment = XmlDocument.DocumentElement;
            XmlElement description = (XmlElement) treatment.GetElementsByTagName("taxon_identification")[0];
            return description.GetAttributeNode("rank").Value;
        }

        public void cargarTerminos() {

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
            Terminos.Add(tNWords.ElementAt(0) + ' ' + tNWords.ElementAt(1));
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
