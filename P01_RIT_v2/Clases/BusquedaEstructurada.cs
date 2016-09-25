using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace P01_RIT_v2.Clases
{
    [Serializable]
    public class BusquedaEstructurada
    {
        /// <summary>
        /// Fecha y hora de la consulta estructurada.
        /// </summary>
        public DateTime FechaHoraBusquedaEstructurada;

        /// <summary>
        /// Ruta del archivo con el escalafón de la consulta vectorial utilizada.
        /// </summary>
        public string RutaBusquedaVectorial;

        /// <summary>
        /// Ruta del archivo donde se almacena la búsqueda estructurada (si esta ha sido exportada o importada).
        /// </summary>
        [NonSerialized]
        private string RutaBusquedaEstructurada;

        /// <summary>
        /// Lista de cláusulas de la consulta estructurada.
        /// </summary>
        public List<ClausulaEstructurada> ClausulasConsulta;

        /// <summary>
        /// Fecha y hora de la consulta vectorial utilizada.
        /// </summary>
        public DateTime FechaHoraBusquedaVectorial;

        /// <summary>
        /// Ruta de la colección de documentos del escalafón vectorial utilizado.
        /// </summary>
        public string RutaColeccionDocumentos;

        /// <summary>
        /// Consulta vectorial en bruto del escalafón vectorial utilizado.
        /// </summary>
        public string ConsultaVectorial;

        /// <summary>
        /// Ranking de documentos utilizados para la consulta estructurada.
        /// </summary>
        public List<RankingDocumento> RankingDocumentos;

        public BusquedaEstructurada()
        {
            FechaHoraBusquedaEstructurada = System.DateTime.Now;
            RutaBusquedaEstructurada = "";

            FechaHoraBusquedaVectorial = System.DateTime.Now;
            RutaBusquedaVectorial = "";
            ConsultaVectorial = "";

            RutaColeccionDocumentos = "";

            ClausulasConsulta = new List<ClausulaEstructurada>();
            RankingDocumentos = new List<RankingDocumento>();
        }

        public BusquedaEstructurada(string rutaArchivoConsultaVectorial, string consultaEstructurada)
        {
            FechaHoraBusquedaEstructurada = System.DateTime.Now;
            RutaBusquedaEstructurada = "";

            ClausulasConsulta = new List<ClausulaEstructurada>();
            importarConsultaVectorial(rutaArchivoConsultaVectorial);

            ejecutar(consultaEstructurada);
        }

        public BusquedaEstructurada(BusquedaVectorial consultaVectorial, string consultaEstructurada)
        {
            FechaHoraBusquedaEstructurada = System.DateTime.Now;
            RutaBusquedaEstructurada = "";

            ClausulasConsulta = new List<ClausulaEstructurada>();
            importarConsultaVectorial(consultaVectorial);

            ejecutar(consultaEstructurada);
        }

        /// <summary>
        /// Importa el contenido de un archivo XML con la consulta vectorial a utilizar.
        /// </summary>
        /// <param name="busquedaVectorial">Consulta vectorial importada.</param>
        private void importarConsultaVectorial(BusquedaVectorial busquedaVectorial)
        {
            RutaBusquedaVectorial = busquedaVectorial.RutaBusquedaVectorial;
            FechaHoraBusquedaVectorial = busquedaVectorial.FechaHoraBusquedaVectorial;
            RutaColeccionDocumentos = busquedaVectorial.RutaDocumentos;
            ConsultaVectorial = busquedaVectorial.Consulta;

            RankingDocumentos = new List<RankingDocumento>(busquedaVectorial.RankingDocumentos);
        }

        /// <summary>
        /// Importa el contenido de un archivo XML con la consulta vectorial a utilizar.
        /// </summary>
        /// <param name="rutaArchivo">Ruta absoluta del archivo.</param>
        private void importarConsultaVectorial(string rutaArchivo)
        {
            BusquedaVectorial consultaImportada = BusquedaVectorial.importarDesdeXml(rutaArchivo, true);
            importarConsultaVectorial(consultaImportada);
        }

        /// <summary>
        /// Revisa un documento del escalafón para saber si cumple la cláusula.
        /// </summary>
        /// <param name="rankingDocumento">Documento en escalafón.</param>
        /// <param name="clausula">Cláusula para evaluar el documento.</param>
        /// <returns>True si el documento contiene la estructura consultada. False en caso contrario.</returns>
        private bool revisarDocumento(RankingDocumento rankingDocumento, ClausulaEstructurada clausula)
        {
            Documento documento = new Documento(rankingDocumento.IdDocumento, rankingDocumento.RutaDocumento);
            documento.cargarDocumento();

            // Obtiene todos los valores del atributo "name" para las etiquetas "biological_entity"
            List<string> bioEntitiesDocumento = documento.getBiologicalEntitiesNames();
            foreach (string bioEntityNameDoc in bioEntitiesDocumento)
            {
                // Para cada término de la cláusua se reemplazan acentos y se utilizarán letras minúsculas.
                string bioEntityName = Stopwords.Instance.reemplazarAcentos(bioEntityNameDoc).ToLower();

                // Coincide el "name" de "biological_entity".
                if (bioEntityName.Equals(clausula.BiologicalEntity))
                {
                    // La cláusula sólo tiene un término.
                    if (clausula.CharacterName.Equals(""))
                    {
                        return true;
                    }

                    // La cláusula tiene más de un término.
                    else
                    {
                        List<string[]> charactersBioEntity = documento.getBiologicalEntityCharacters(bioEntityNameDoc);
                        foreach (string[] characterBioEntity in charactersBioEntity)
                        {
                            string characterName = Stopwords.Instance.reemplazarAcentos(characterBioEntity[0]).ToLower();

                            // Hay coincidencia en el atributo "name" de "character".
                            if (characterName.Equals(clausula.CharacterName))
                            {
                                // La cláusula tiene tres términos.
                                if (clausula.CharacterValue.Equals(""))
                                {
                                    return true;
                                }
                                else
                                {
                                    string characterValue = Stopwords.Instance.reemplazarAcentos(characterBioEntity[1]).ToLower();

                                    // La consulta tiene coincidencia en el atributo "value" de "character".
                                    if (characterValue.Equals(clausula.CharacterValue))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Divide la cláusula
        /// </summary>
        /// <param name="clausulaEncontrada"></param>
        /// <returns></returns>
        private ClausulaEstructurada procesarClausula(string clausulaEncontrada)
        {
            string[] terminosClausula = clausulaEncontrada.Split(' ');
            if (terminosClausula.Length < 1)
            {
                throw new Exception("La cláusula recibir tiene un formato inválido y no puede ser procesada.");
            }
            else
            {
                ClausulaEstructurada nuevaClausula = new ClausulaEstructurada(terminosClausula[0]);
                if (terminosClausula.Length > 1)
                {
                    nuevaClausula.CharacterName = terminosClausula[1];
                }
                if (terminosClausula.Length > 2)
                {
                    nuevaClausula.CharacterValue = terminosClausula[2];
                }
                return nuevaClausula;
            }
        }

        /// <summary>
        /// Procesa la consulta estructurada en bruto para obtener las cláusulas válidas.
        /// </summary>
        /// <param name="consulta">Consulta en bruto recibida.</param>
        private void procesarConsulta(string consulta)
        {
            // Las cláusulas deben ser insensibles a mayúsculas y acentos.
            consulta.ToLower();
            consulta = Stopwords.Instance.reemplazarAcentos(consulta);

            string patronSeparador = @"\,\s*";
            Regex regExSeparador = new Regex(patronSeparador, RegexOptions.Compiled);
            consulta = regExSeparador.Replace(consulta, ", ");

            // RegEx para separa términos de clausulas en grupos de 1 a 3 términos.
            string patronSeparadorClausulas = @"((\d*\d\,?\d+)|(\w+))(\s((\d*\d\,?\d+)|(\w+))){0,2}";
            Regex regExSeparadorClausulas = new Regex(patronSeparadorClausulas, RegexOptions.Compiled);

            MatchCollection clausulasEncontradas = regExSeparadorClausulas.Matches(consulta);
            foreach (Match clausulaEncontrada in clausulasEncontradas)
            {
                ClausulaEstructurada nuevaClausula = procesarClausula(clausulaEncontrada.Value);
                ClausulasConsulta.Add(nuevaClausula);
            }
        }

        /// <summary>
        /// Elimina las entradas del escalafón que no cumplen con las cláusulas de la consulta estructurada.
        /// </summary>
        public void filtrarEscalafon()
        {
            // Recorrido para cada cláusula procesada.
            foreach (ClausulaEstructurada clausula in ClausulasConsulta)
            {
                // Elimina todos los documentos en el escalafón que no cumplen la cláusula actual.
                RankingDocumentos.RemoveAll(ranking => (revisarDocumento(ranking, clausula) == false));
            }
        }

        public void ejecutar(string consultaEnBruto)
        {
            // Procesa la consulta en bruto para generar las cláusulas de filtrado de la consulta estructurada.
            procesarConsulta(consultaEnBruto);
            // Luego filtra el escalafón, eliminando los documentos que no cumplan las cláusulas obtenidas.
            filtrarEscalafon();
        }

        /// <summary>
        /// Exporta los resultados de la búsqueda vectorial a un archivo XML.
        /// </summary>
        /// <param name="nombreArchivoPostings">
        /// Nombre del archivo XML que será creado.
        /// Si no se ingresa un nombre se utiliza la fecha y hora del sistema y el archivo será guardado en la carpeta por defecto (...\\Archivos).
        /// </param>
        /// <param name="rutaAbsoluta">
        /// Parámetro opcional para indicar si se utiliza una ruta absoluta. True para usar una ruta absoluta, False para usar una ruta relativa.
        /// Utilizar una ruta relativa implica que el archivo será guardado en la carpeta por defecto (...\\Archivos) y no se debe agregar la extensión (*.xml).
        /// Utilizar una ruta absoluta debe incluir la extensión del archivo.
        /// </param>
        public void exportarComoXml(string rutaArchivo, bool rutaAbsoluta = false)
        {
            RutaBusquedaEstructurada = EntradaSalidaXml.exportarComoXml(this, rutaArchivo, rutaAbsoluta);
        }

        /// <summary>
        /// Importa el contenido de un archivo XML para generar el resultado de una búsqueda vectorial.
        /// </summary>
        /// <param name="rutaArchivo">
        /// Ruta del archivo que será abierto.
        /// </param>
        /// <param name="rutaAbsoluta">
        /// Indica si se utiliza una ruta absoluta (completa) de archivo. True para usar una ruta absoluta o false para usar una ruta relativa.
        /// Si utiliza una ruta relativa implica que el archivo ingresado está guardado en la carpeta por defecto (...\\Archivos).
        /// Si utiliza una ruta absoluta debe incluir la extensión del archivo.
        /// </param>
        /// <returns>
        /// Resultado de búsqueda vectorial importado.
        /// </returns>
        public static BusquedaEstructurada importarDesdeXml(string rutaArchivo, bool rutaAbsoluta = false)
        {
            BusquedaEstructurada busquedaImportada = EntradaSalidaXml.importarDesdeXml<BusquedaEstructurada>(rutaArchivo, rutaAbsoluta);
            if (!rutaAbsoluta)
            {
                rutaArchivo = Opciones.Instance.RutaArchivos + rutaArchivo;
            }

            busquedaImportada.RutaBusquedaEstructurada = rutaArchivo;
            return busquedaImportada;
        }

        /// <summary>
        /// Genera un archivo HTML con el resultado de una búsqueda estructurada.
        /// </summary>
        /// <param name="nombreArchivoPostings">
        /// Nombre del archivo XML que será creado.
        /// Si no se ingresa un nombre se utiliza la fecha y hora del sistema y el archivo será guardado en la carpeta por defecto (...\\Archivos).
        /// </param>
        /// <param name="usarRutaAbsoluta">
        /// Parámetro opcional para indicar si se utiliza una ruta absoluta. True para usar una ruta absoluta, False para usar una ruta relativa.
        /// Utilizar una ruta relativa implica que el archivo será guardado en la carpeta por defecto (...\\Archivos) y no se debe agregar la extensión (*.xml).
        /// Utilizar una ruta absoluta debe incluir la extensión del archivo.
        /// </param>
        public void generarHTML(string rutaArchivo, bool usarRutaAbsoluta = false)
        {
            if (!usarRutaAbsoluta)
            {
                rutaArchivo = Opciones.Instance.RutaConsultas + rutaArchivo;
            }

            // Información básica del HTML.
            string strFechaHoraBusquedaEstruct = FechaHoraBusquedaEstructurada.ToString("dd/MM/yyyy HH:mm:ss.fff");
            string strFechaHoraBusquedaVect = FechaHoraBusquedaVectorial.ToString("dd/MM/yyyy HH:mm:ss.fff");
            string strRutaDocumentos = RutaColeccionDocumentos;
            string strConsulaVectorial = ConsultaVectorial;

            // Contenido de cada cláusula usada en la búsqueda estructurada.
            List<string[]> clausulas = new List<string[]>();
            foreach (ClausulaEstructurada clausula in ClausulasConsulta)
            {
                // Formato de cada entrada de las cláusulas utilizadas:
                clausulas.Add(new string[] { clausula.BiologicalEntity, clausula.CharacterName, clausula.CharacterValue });
            }

            // Obtiene la información de los primeros 30 elementos del escalafón.
            List<string[]> top30 = new List<string[]>();

            // Se utiliza la longitud del escalafón filtrado si tiene menos de 30 documentos.
            int longitudEscalafon = (RankingDocumentos.Count < 30) ? RankingDocumentos.Count : 30;

            for (int posicion = 0; posicion < longitudEscalafon; posicion++)
            {
                RankingDocumento rankingObtenido = RankingDocumentos[posicion];
                string strPosicion = rankingObtenido.Posicion.ToString();
                string strSimilitud = rankingObtenido.Similitud.ToString();
                string strDocId = rankingObtenido.IdDocumento.ToString();
                string strTaxonName = rankingObtenido.TaxonNameDocumento;
                string strTaxonRank = rankingObtenido.TaxonRank;

                Documento documento = new Documento(rankingObtenido.IdDocumento, rankingObtenido.RutaDocumento);
                documento.cargarDocumento();

                string strTaxonDescription = documento.getTaxonDescription(true);
                string strRutaDocumento = documento.Ruta;

                // Formato de cada entrada del escalafón para las primeras 30 posiciones.
                top30.Add(new string[] { strPosicion, strSimilitud, strDocId, strTaxonName, strTaxonRank, strTaxonDescription, strRutaDocumento });
            }

            // Generar el html
            string html = "<head><meta charset =\"UTF-8\"></head><h1>Consulta estructurada</h1><pre>";
            html += "Fecha de la consulta estructurada: \t" + strFechaHoraBusquedaEstruct + "\n";
            html += "Fecha de la consulta vectorial: \t" + strFechaHoraBusquedaVect + "\n";
            html += "Ruta de la coleccion consultada: \t" + strRutaDocumentos + "\n";
            html += "Texto de la consulta: \t" + strConsulaVectorial + "\n";
            html += "Listas de cláusulas de la consulta: \n";
            foreach (string[] clausula in clausulas)
            {
                html += "Biological Entity (name): " + clausula[0] + "\n";
                html += "Character (name): " + clausula[1] + "\n";
                html += "Character (value): " + clausula[2] + "\n";
            }
            foreach (string[] doc in top30)
            {
                html += "\nId del documento: " + doc[2] + "\n";
                html += "Posición obtenida: " + doc[0] + "\n";
                html += "Similitud: " + doc[1] + "\n";
                html += "Taxon Name: " + doc[3] + "\n";
                html += "Taxon Rank: " + doc[4] + "\n";
                html += "Taxon Description:\n" + doc[5] + "\n";

                string urlArchivo = "file:///" + Regex.Replace(doc[6], @"\\", "/");
                html += "<a href = \"" + urlArchivo + "\">Ubicación: " + doc[6] + "</a>\n";

            }
            html += "</pre>";

            StreamWriter file = null;
            try
            {
                file = new StreamWriter(rutaArchivo);
                file.WriteLine(html);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw new Exception("No se ha podido crear el archivo html: \n" + e.Message);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
        }
    }

    /// <summary>
    /// Clase para almacenar consulta estructurada.
    /// </summary>
    [Serializable]
    public class ClausulaEstructurada
    {
        public string BiologicalEntity;
        public string CharacterName;
        public string CharacterValue;

        public ClausulaEstructurada()
        {
            BiologicalEntity = "";
            CharacterName = "";
            CharacterValue = "";
        }

        public ClausulaEstructurada(string biologicalEntity, string characterName = "", string characterValue = "")
        {
            BiologicalEntity = biologicalEntity;
            CharacterName = characterName;
            CharacterValue = characterValue;
        }
    }
}