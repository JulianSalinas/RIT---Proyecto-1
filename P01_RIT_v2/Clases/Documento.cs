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

        /// <summary>
        /// Obtiene la cantidad de apariciones de un término dentro del documento.
        /// </summary>
        /// <param name="term">Término consultado.</param>
        /// <returns>Cantidad de apariciones del término. Si el término no aparece, se retorna cero.</returns>
        public int countTermino( string term ) {
            return Terminos.FindAll(x => x.ToString() == term).Count;
        }

        /// <summary>
        /// Determina si existe un término en el documento.
        /// </summary>
        /// <param name="term">Término consultado.</param>
        /// <returns>True si el término existe dentro del documento. False en caso contrario.</returns>
        public bool hasTermino( string term ) {
            return Terminos.Exists(x => x.ToString() == term);
        }

        /// <summary>
        /// Carga el documento XML en la memoria.
        /// </summary>
        public void cargarXmlDocument() {
            XmlDocument = new XmlDocument();
            XmlDocument.Load(Ruta);
        }

        /// <summary>
        /// Obtiene el valor del atributo "taxon_description" del documento.
        /// </summary>
        /// <returns></returns>
        public string getTaxonDescription() {
            if (XmlDocument == null)
            {
                cargarXmlDocument();
            }
            try
            {
                XmlElement treatment = XmlDocument.DocumentElement;
                XmlNode node = treatment.GetElementsByTagName("description")[0];
                return node.Attributes["taxon_description"].Value;
            }
            catch (NullReferenceException e)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene el valor del atributo "taxon_name" del documento.
        /// </summary>
        /// <returns></returns>
        public string getTaxonName() {
            // Si el documento XML no está abierto, se carga.
            if (XmlDocument == null)
            {
                cargarXmlDocument();
            }
            try
            {
                XmlElement treatment = XmlDocument.DocumentElement;
                XmlNode node = treatment.GetElementsByTagName("taxon_identification")[0];
                return node.Attributes["taxon_name"].Value;
            }
            catch (NullReferenceException e)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene el valor del atributo "rank" del documento.
        /// </summary>
        /// <returns></returns>
        public string getRank() {
            // Si el documento XML no está abierto, se carga.
            if (XmlDocument == null)
            {
                cargarXmlDocument();
            }

            try
            {
                XmlElement treatment = XmlDocument.DocumentElement;
                XmlNode node = treatment.GetElementsByTagName("taxon_identification")[0];
                return node.Attributes["rank"].Value;
            }
            catch (NullReferenceException e)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene una lista con todos los valores del atributo "name" para todos los nodos "biological_entity" del documento.
        /// </summary>
        /// <returns></returns>
        public List<string> getBiologicalEntitiesNames()
        {
            // Si el documento XML no está abierto, se carga.
            if (XmlDocument == null)
            {
                cargarXmlDocument();
            }

            XmlElement treatment = XmlDocument.DocumentElement;
            XmlNodeList nodes = treatment.GetElementsByTagName("biological_entity");
            List<string> values = new List<string>();

            foreach (XmlNode node in nodes)
            {
                try
                {
                    values.Add(node.Attributes["name"].Value);
                }
                catch (NullReferenceException)
                {
                    continue;
                }
            }
            return values;
        }

        /// <summary>
        /// Obtiene una lista con pares de valores para los atributos "name" y "value" de los nodos "character" asociados a un nodo "biological_entity" del documento.
        /// </summary>
        /// <param name="biologicalEntityName">Valor del atributo "name" para el "biological_entity" consultado.</param>
        /// <returns>Lista con pares de valores para los atributos "name" y "value" para cada "character". 
        /// Si el "biological_entity" no existe, se retorna Null.
        /// Si algún "character" no tiene valores para algún atributo, dicho atributo se devuelve como un string vacío ("").
        /// </returns>
        public List<string[]> getBiologicalEntityCharacters(string biologicalEntityName)
        {
            // Si el documento XML no está abierto, se carga.
            if (XmlDocument == null)
            {
                cargarXmlDocument();
            }

            List<string[]> charactersList = new List<string[]>();
            XmlElement treatment = XmlDocument.DocumentElement;
            try
            {
                XmlNodeList biologicalEntities = treatment.GetElementsByTagName("biological_entity");
                foreach(XmlNode entity in biologicalEntities)
                {
                    if (entity.Attributes["name"].Value.Equals(biologicalEntityName))
                    {
                        // Verificar esta expresión XPath para obtener los "character" del "biological_entity".
                        XmlNodeList characters = entity.SelectNodes(".//character");

                        foreach (XmlNode character in characters)
                        {
                            string character_name = character.Attributes["name"].Value;
                            string character_value = character.Attributes["value"].Value;
                            if (character_name == null)
                            {
                                character_name = "";
                            }
                            if (character_value == null)
                            {
                                character_value = "";
                            }
                            charactersList.Add(new string[] { character_name, character_value });
                        }
                        return charactersList;
                    }
                }
                return null;
            }
            catch (NullReferenceException e)
            {
                return null;
            }
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
