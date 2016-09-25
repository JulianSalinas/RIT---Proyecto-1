using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

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
        private string rutaArchivo;
        public string RutaArchivo
        {
            get { return rutaArchivo; }
            set { rutaArchivo = value; }
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
        private XDocument archivo;

        /// <summary>
        /// Constructor por defecto. Necesario para serializar y deserializar clase.
        /// </summary>
        public Documento()
        {
            Id = 0;
            RutaArchivo = "";
            terminos = null;
            archivo = null;
            terminos = new Dictionary<string, int>();
        }

        public Documento(int id, string ruta)
        {
            Id = id;
            RutaArchivo = ruta;
            terminos = new Dictionary<string, int>();
            archivo = null;
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
            try
            {
                archivo = XDocument.Load(rutaArchivo);
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo abrir el documento.");
            }
        }

                /// <summary>
        /// Obtiene el valor del atributo "taxon_name" del documento.
        /// </summary>
        /// <returns></returns>
        public string getTaxonName()
        {
            // Si el documento XML no está abierto, se carga.
            if (archivo == null)
            {
                cargarDocumento();
            }
            try
            {
                XNamespace ns = archivo.Root.Name.Namespace;
                return archivo.Descendants(ns + "taxon_identification").Select(x => x.Attribute("taxon_name").Value).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new Exception("Error al leer name de taxon_identification");
            }
        }

        /// <summary>
        /// Obtiene el valor del atributo "rank" del documento.
        /// </summary>
        /// <returns></returns>
        public string getTaxonRank()
        {
            // Si el documento XML no está abierto, se carga.
            if (archivo == null)
            {
                cargarDocumento();
            }
            try
            {
                XNamespace ns = archivo.Root.Name.Namespace;
                return archivo.Root.Descendants(ns + "taxon_identification").Select(x => x.Attribute("rank").Value).FirstOrDefault();

            }
            catch (Exception e)
            {
                throw new Exception("Error al leer rank de taxon_identification");
            }
        }


        /// <summary>
        /// Obtiene el valor del atributo "taxon_description" del documento.
        /// </summary>
        /// <param name="truncar">Parámetro opcionar para limitar el valor obtenido a un máximo de 200 caractéres. Para usar esta opción, pasar True.</param>
        /// <returns></returns>
        public string getTaxonDescription(bool truncar = false)
        {
            if (archivo == null)
            {
                cargarDocumento();
            }
            try
            {
                XNamespace ns = archivo.Root.Name.Namespace;
                string taxonDescription = 
                    archivo.Root.Descendants(ns + "description").Select(x => x.Attribute("taxon_description").Value).FirstOrDefault();

                if (truncar)
                {
                    // Comprime los espacios múltpiles y reemplaza los saltos de línea por espacios.
                    Regex.Replace(taxonDescription, @"(\s{2,})|(\r\n|\r|\n)", " ");
                    if (taxonDescription.Length > 200)
                    {
                        taxonDescription = taxonDescription.Substring(0, 200);
                    }
                }
                return taxonDescription;
            }
            catch (Exception e)
            {
                throw new Exception("Error al leer taxon_description");
            }
        }


        public bool encontrarDato(string biologicalEntityName, string characterName = "", string characterValue = "")
        {
            try
            {
                XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
                nsm.AddNamespace("ns", archivo.Root.Name.NamespaceName);

                string xpath = "/ns:treatment/ns:description/ns:statement/ns:biological_entity[@name=\'" + biologicalEntityName + "\'";
                if (!characterName.Equals(""))
                {
                    xpath += "]/ns:character[@name=\'" + characterName + "\'";
                }
                if (!characterValue.Equals(""))
                {
                    xpath += " and @value='" + characterValue + "\'";
                }
                xpath += "]";

                IEnumerable<XElement> encontrado = archivo.XPathSelectElements(xpath, nsm);
                int cuenta = encontrado.Count();

                return cuenta > 0;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message + "\n" + e.StackTrace);
                return false;
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

            string noPuntuacionesPattern = @"[^a-zA-Z0-9ñÑ ]|(\d)?(\d|,)*\.?\d";      
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
            string nuevoRank = getTaxonRank().ToLower();

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
            termsString += "Ruta: " + RutaArchivo + "\n";
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