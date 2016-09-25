using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace P01_RIT_v2.Clases
{
    [Serializable]
    public class Documento
    {
        /// <summary>
        /// Identificador del documento.
        /// </summary>
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Ruta del archivo asociado al documento.
        /// </summary>
        private string ruta;
        public string Ruta
        {
            get { return ruta; }
            set { ruta = value; }
        }

        [NonSerialized]
        /// <summary>
        /// Diccionario con los términos indizables encontrados en el Documento. 
        /// Las llaves son términos encontrados y los valores correspondiente son la cuenta de apariciones del término.
        /// </summary>
        private Dictionary<string, int> terminos;

        [NonSerialized]
        /// <summary>
        /// Archivo XML del documento cargado en memoria. Null si el documento no está cargado en memoria.
        /// </summary>
        private XmlDocument XmlDocument;

        /// <summary>
        /// Constructor por defecto. Necesario para serializar y deserializar clase.
        /// </summary>
        public Documento()
        {
            Id = 0;
            Ruta = "";
            terminos = null;
            XmlDocument = null;
            terminos = new Dictionary<string, int>();
        }

        public Documento(int id, string ruta)
        {
            this.Id = id;
            this.Ruta = ruta;
            terminos = new Dictionary<string, int>();
            XmlDocument = null;
        }

        /// <summary>
        /// Obtiene la cantidad de apariciones de un término dentro del documento.
        /// </summary>
        /// <param name="term">Término consultado.</param>
        /// <returns>Cantidad de apariciones del término. Si el término no aparece, se retorna cero.</returns>
        public int obtenerCuentaTermino(string term)
        {
            if (!terminos.ContainsKey(term))
            {
                return 0;
            }
            else
            {
                return terminos[term];
            }
        }

        /// <summary>
        /// Determina si existe un término en el documento.
        /// </summary>
        /// <param name="term">Término consultado.</param>
        /// <returns>True si el término existe dentro del documento. False en caso contrario.</returns>
        public bool hasTermino(string term)
        {
            return terminos.ContainsKey(term);
        }

        /// <summary>
        /// Carga el documento XML en la memoria.
        /// </summary>
        public void cargarDocumento()
        {
            XmlDocument = new XmlDocument();
            XmlDocument.Load(Ruta);
        }

        /// <summary>
        /// Obtiene el valor del atributo "taxon_description" del documento.
        /// </summary>
        /// <param name="truncar">Parámetro opcionar para limitar el valor obtenido a un máximo de 200 caractéres. Para usar esta opción, pasar True.</param>
        /// <returns></returns>
        public string getTaxonDescription(bool truncar = false)
        {
            if (XmlDocument == null)
            {
                cargarDocumento();
            }
            try
            {
                XmlElement treatment = XmlDocument.DocumentElement;
                XmlNode node = treatment.GetElementsByTagName("description")[0];
                string value = node.Attributes["taxon_description"].Value;

                if (truncar)
                {
                    // Comprime los espacios múltpiles y reemplaza los saltos de línea por espacios.
                    Regex.Replace(value, @"(\s{2,})|(\r\n|\r|\n)", " ");
                    if (value.Length > 200)
                    {
                        value = value.Substring(0, 200);
                    }
                }
                return value;
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
        public string getTaxonName()
        {
            // Si el documento XML no está abierto, se carga.
            if (XmlDocument == null)
            {
                cargarDocumento();
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
        public string getRank()
        {
            // Si el documento XML no está abierto, se carga.
            if (XmlDocument == null)
            {
                cargarDocumento();
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
                cargarDocumento();
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
                cargarDocumento();
            }

            List<string[]> charactersList = new List<string[]>();
            XmlElement treatment = XmlDocument.DocumentElement;
            try
            {
                XmlNodeList biologicalEntities = treatment.GetElementsByTagName("biological_entity");
                foreach (XmlNode biological_entity in biologicalEntities)
                {
                    if (biological_entity.Attributes["name"].Value.Equals(biologicalEntityName))
                    {
                        // Verificar esta expresión XPath para obtener los nodos "character" hijos del nodo "biological_entity".
                        XmlNodeList characters = biological_entity.ChildNodes;

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


        /// <summary>
        /// Obtiene una lista de todos los términos registrados dentro del documento.
        /// </summary>
        /// <returns>Lista con términos registrados en el documento.</returns>
        public List<string> getTerminos()
        {
            if (terminos.Count == 0)
            {
                indizarDocumento();
            }
            return terminos.Keys.ToList();
        }

        /// <summary>
        /// Busca todos los términos indizables dentro del documento.
        /// </summary>
        /// <returns>Lista de términos indizables encontrados.</returns>
        public List<string> indizarDocumento()
        {
            cargarDocumento();

            // RegEx para obtener todos los numeros incluso los que tienen decimales
            string numPattern = @"(\d)?(\d|,)*\.?\d";
            Regex numRegEx = new Regex(numPattern, RegexOptions.Compiled);

            // RegEx para eliminar los signos de puntuacion

            string noPuntuacionesPattern = @"[^a-zA-Z0-9ñ ]|(\d)?(\d|,)*\.?\d";      
            Regex noPuntuacionesRegEx = new Regex(noPuntuacionesPattern, RegexOptions.Compiled);

            // Agrega los terminos del TaxonName y el Rank
            string nuevoTaxonName = getTaxonName().ToLower();
            nuevoTaxonName = Stopwords.Instance.reemplazarAcentos(nuevoTaxonName);
            nuevoTaxonName = noPuntuacionesRegEx.Replace(nuevoTaxonName, "");

            List<string> palabrasNuevoTaxonName = nuevoTaxonName.Split(' ').ToList();
            nuevoTaxonName = palabrasNuevoTaxonName[0];     // Del TaxonName obtenido al principio sólo importan una o dos palabras para formar el término.

            if (palabrasNuevoTaxonName.Count > 1)           // Si el TaxonName tiene más de una palabra, se agrega sólo la segunda palabra.
            {
                nuevoTaxonName += ' ' + palabrasNuevoTaxonName[1];
            }

            if (terminos.ContainsKey(nuevoTaxonName))
            {
                ++terminos[nuevoTaxonName];
            }
            else
            {
                terminos.Add(nuevoTaxonName, 1);
            }
            string nuevoRank = getRank().ToLower();

            if (terminos.ContainsKey(nuevoRank))
            {
                ++terminos[nuevoRank];     
            }
            else
            {
                terminos.Add(nuevoRank, 1);
            }

            /* Se agregan los términos pertenecientes al Taxon description */
            string nuevoTaxonDescription = getTaxonDescription().ToLower();
            nuevoTaxonDescription = Stopwords.Instance.reemplazarAcentos(nuevoTaxonDescription);

            // Agrega primero todos los números encontrados en el Taxon description al diccionario.
            foreach (Match match in numRegEx.Matches(nuevoTaxonDescription))
            {
                if (terminos.ContainsKey(match.Value))
                {
                    ++terminos[match.Value];
                }
                else
                {
                    terminos.Add(match.Value, 1);
                }
            }

            // Agrega luego todas las palabras encontradas en el Taxon description.
            nuevoTaxonDescription = noPuntuacionesRegEx.Replace(nuevoTaxonDescription, "");
            // CORREGIR: Quitar stopwords.
            string[] palabrasTaxonDescription = nuevoTaxonDescription.Split(' ');

            foreach (string palabra in palabrasTaxonDescription)
            {
                if (!palabra.Equals("") && !Stopwords.Instance.hasTerm(palabra))
                {
                    if (terminos.ContainsKey(palabra))
                {
                    ++terminos[palabra];
                }
                else
                {
                    terminos.Add(palabra, 1);
                }
                }
            }

            return terminos.Keys.ToList();      // Lista con términos encontrados.
        }

        override public string ToString()
        {
            string termsString = "";
            termsString += "IDDocumento: " + Id.ToString() + "\n";
            termsString += "Ruta: " + Ruta + "\n";
            termsString += "Términos: \n";

            string[] terminos = this.terminos.Keys.ToArray();

            foreach (string term in terminos)
            {
                termsString += "<" + term + ">";
            }
            return termsString;
        }
    }
}