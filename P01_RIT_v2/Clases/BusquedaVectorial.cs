using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace P01_RIT_v2.Clases
{
    [Serializable]
    public class BusquedaVectorial
    {

        /// <summary>
        /// Archivo Invertido sobre el cual se hará la consulta.
        /// </summary>
        private Invertido archivoInvertido;
        [XmlIgnore]
        public Invertido ArchivoInvertido
        {
            get { return archivoInvertido; }
            set { archivoInvertido = value; }
        }

        /// <summary>
        /// Ruta del archivo donde se almacena la búsqueda vectorial (si esta ha sido exportada o importada).
        /// </summary>
        private string rutaBusquedaVectorial;

        [XmlIgnore]
        public string RutaBusquedaVectorial
        {
            get { return rutaBusquedaVectorial; }
            set { rutaBusquedaVectorial = value; }
        }


        /// <summary>
        /// Ruta del archivo invertido utilizado para efectuar la búsqueda vectorial.
        /// </summary>
        private string rutaArchivoInvertido;
        public string RutaArchivoInvertido
        {
            get { return rutaArchivoInvertido; }
            set { rutaArchivoInvertido = value; }
        }

        
        /// <summary>
        /// Ruta de la colección de documentos.
        /// </summary>
        private string rutaDocumentos;
        public string RutaDocumentos
        {
            get { return rutaDocumentos; }
            set { rutaDocumentos = value; }
        }

        /// <summary>
        /// Fecha y hora de la consulta efectuada.
        /// </summary>
        private DateTime fechaHoraBusqueda;
        public DateTime FechaHoraBusqueda
        {
            get { return fechaHoraBusqueda; }
            set { fechaHoraBusqueda = value;}
        }

        /// <summary>
        /// Consulta (en bruto) efectuada. La consulta en bruto son los términos ingresados por el usuario sin procesar (parse).
        /// </summary>
        private string consulta;
        public string Consulta
        {
            get { return consulta; }
            set { consulta = value; }
        }

        /// <summary>
        /// Ranking (escalafón) de documentos de la búsqueda vectorial.
        /// </summary>
        private List<RankingDocumento> rankingDocumentos;
        public List<RankingDocumento> RankingDocumentos
        {
            get { return rankingDocumentos; }
            set { rankingDocumentos = value; }
        }

        
        /// <summary>
        /// Constructor por defecto del objeto. Necesario para serializar y deserializar objeto.
        /// </summary>
        public BusquedaVectorial()
        {
            rutaArchivoInvertido = "";
            archivoInvertido = null;
            rutaDocumentos = "";
            fechaHoraBusqueda = System.DateTime.Now;
            consulta = "";
            rankingDocumentos = new List<RankingDocumento>();

        }

        public BusquedaVectorial(string rutaArchivoInvertido, string rutaDocumentos, string consulta)
        {
            this.rutaArchivoInvertido = rutaArchivoInvertido;
            this.rutaDocumentos = rutaDocumentos;
            this.consulta = consulta;
            this.fechaHoraBusqueda = System.DateTime.Now;

            rankingDocumentos = new List<RankingDocumento>();
            archivoInvertido = Invertido.importarArchivoInvertido(rutaArchivoInvertido, true);

            ejecutar();
        }

        public BusquedaVectorial(Invertido archivoInvertido, string rutaDocumentos, string consulta)
        {
            this.archivoInvertido = archivoInvertido;
            this.rutaDocumentos = rutaDocumentos;
            this.consulta = consulta;
            this.fechaHoraBusqueda = System.DateTime.Now;

            rankingDocumentos = new List<RankingDocumento>();
            rutaArchivoInvertido = archivoInvertido.RutaArchivoInvertido;

            ejecutar();
        }

        /// <summary>
        /// Calcula la normal de un documento, utilizando los pesos de sus postings.
        /// </summary>
        /// <param name="idDocumento">Identificador del documento</param>
        /// <returns>Normal del documento : |Dj|</returns>
        private double calcularNormalDocumento(int idDocumento)
        {
            List<Posting> postingsDocumento = archivoInvertido.obtenerPostingsDocumento(idDocumento);
            double sumaPesos = 0;

            foreach (Posting posting in postingsDocumento)
            {
                sumaPesos += Math.Pow(posting.Peso, 2);
            }

            return Math.Sqrt(sumaPesos);
        }

        /// <summary>
        /// Calcula la normal para una consulta.
        /// </summary>
        /// <param name="terminosConsulta">Pesos de la consulta.</param>
        /// <returns>Normal de la consulta : |Q| </returns>
        private double calcularNormalConsulta(List<TerminoConsultaVectorial> terminosConsulta)
        {
            double sumaPesos = 0;
            foreach (TerminoConsultaVectorial terminoConsulta in terminosConsulta){
                sumaPesos += Math.Pow(terminoConsulta.Peso, 2);
            }

            return Math.Sqrt(sumaPesos);
        }

        /// <summary>
        /// Llena el ranking de la búsqueda vectorial con una nueva lista de rankings "en blanco" con todos los documentos del archivo invertido.
        /// </summary>
        /// <returns>Diccionario con referencias a los documentos del ranking, generado para facilitar acceso a los rankings.</returns>
        private Dictionary<int, RankingDocumento> generarNuevoEscalafon()
        {
            List<Documento> coleccionArchivoInvertido = archivoInvertido.Documentos;

            // Diccionario utilizado para agregación de pesos.
            Dictionary<int, RankingDocumento> diccionarioEscalafon = new Dictionary<int, RankingDocumento>();

            foreach(Documento documento in coleccionArchivoInvertido)
            {
                RankingDocumento nuevoRankingDocumento = new RankingDocumento(documento, 0, 0);
                rankingDocumentos.Add(nuevoRankingDocumento);
                diccionarioEscalafon.Add(documento.Id, nuevoRankingDocumento);
            }

            return diccionarioEscalafon;
        }

        /// <summary>
        /// Ordena el ranking de documentos por similitud (descendente) e identificador del documento (ascendente).
        /// </summary>
        private void ordenarRankingDocumentos()
        {
            rankingDocumentos.Sort();

            // Actualiza posiciones de los documentos del ranking.
            int posicion = 1;
            foreach(RankingDocumento ranking in rankingDocumentos){
                ranking.Posicion = posicion++;
            }
        }

        /// <summary>
        /// Normaliza el índice de similitud del ranking de documentos con las normales del documento y la consulta.
        /// </summary>
        /// <param name="normalConsulta">Normal de la consulta.</param>
        private void normalizarRankingDocumentos(double normalConsulta)
        {
            foreach(RankingDocumento rankingDocumento in rankingDocumentos)
            {
                int idDocumento = rankingDocumento.IdDocumento;
                double normalDocumento = calcularNormalDocumento(idDocumento);

                // Si alguna de las normales tiene valor cero, la similitud será cero.
                if ((normalDocumento == 0) || (normalConsulta == 0))
                {
                    rankingDocumento.Similitud = 0;
                }
                else
                {
                    rankingDocumento.Similitud /= (normalDocumento * normalConsulta);
                }
            }
        }

        /// <summary>
        /// Procesa la consulta (refinada) y agrega los pesos a los documentos del ranking (escalafón). Las similitudes resultantes no están normalizados.
        /// </summary>
        /// <param name="terminosConsulta">Consulta refinada y separada por términos con peso.</param>
        /// <param name="diccionarioRankings">Diccionario asociado a ranking de documentos.</param>
        private void agregarPesosRanking(List<TerminoConsultaVectorial> terminosConsulta, Dictionary<int, RankingDocumento> diccionarioRankings)
        {
            // Se recorre cadá término de la consulta para obtener sus postings asociados.
            foreach(TerminoConsultaVectorial terminoConsulta in terminosConsulta)
            {
                List<Posting> postingsTermino = archivoInvertido.obtenerPostingsTerminoExacto(terminoConsulta.Termino);

                // Se recorren los postings del término y se suman las similitudes para cada documento de los postings.
                foreach (Posting postingTermino in postingsTermino)
                {
                    int docIdPosting = postingTermino.DocId;
                    RankingDocumento rankingAsociado;
                    if (diccionarioRankings.TryGetValue(docIdPosting, out rankingAsociado))
                    {
                        rankingAsociado.Similitud += (postingTermino.Peso * terminoConsulta.Peso);
                    }
                }
            }
        }

        /// <summary>
        /// Procesa los metacaracteres del término obtenido para generar el término de consulta válido.
        /// </summary>
        /// <param name="termino">Término obtenido del procesamiento inicial.</param>
        /// <param name="terminosPreparados">Diccionario asociado a términos de consulta.</param>
        private void procesarTermino(string termino, Dictionary<string, TerminoConsultaVectorial> terminosPreparados)
        {
            int pesoConsulta = 2;

            if (termino.Length > 2)
            {
                // Quitar + al inicio del término.
                if (termino[0] == '+')
                {
                    pesoConsulta = 4;
                    termino = termino.Substring(1);
                }

                // Quitar - al inicio del término.
                else if (termino[0] == '-')
                {
                    pesoConsulta = 1;
                    termino = termino.Substring(1);
                }

                // Si término tiene comillas al inicio, es un término compuesto.
                if (termino[0] == '\"')
                {
                    termino = termino.Substring(1);
                }

                // Si el término tiene un asterisco al final, es un prefijo.
                if (termino[termino.Length - 1] == '*')
                {
                    List<Termino> terminosConPrefijo = archivoInvertido.obtenerTerminosConPrefijo(termino);
                    foreach (Termino terminoConPrefijo in terminosConPrefijo)
                    {
                        if (terminosPreparados.ContainsKey(terminoConPrefijo.Contenido))
                        {
                            Console.WriteLine("Está intentando consultar el mismo término dos veces: " + terminoConPrefijo.Contenido);
                        }
                        else
                        {
                            terminosPreparados.Add(terminoConPrefijo.Contenido, new TerminoConsultaVectorial(terminoConPrefijo.Contenido, pesoConsulta));
                        }
                    }
                }
                else
                {
                    if (terminosPreparados.ContainsKey(termino))
                    {
                        Console.WriteLine("Está intentando consultar el mismo término dos veces: " + termino);
                    }
                    else
                    {
                        terminosPreparados.Add(termino, new TerminoConsultaVectorial(termino, pesoConsulta));
                    }
                }
            }
        }

        /// <summary>
        /// Procesa la consulta en bruto recibida para generar los términos de consulta válidos.
        /// </summary>
        /// <returns></returns>
        private List<TerminoConsultaVectorial> procesarConsulta()
        {
            string consulta = this.consulta.ToLower();
            consulta = Stopwords.Instance.reemplazarAcentos(consulta);

            // Hace la división inicial de la consulta en bruto.
            string patronInicial = @"[\+\-]?((" + '\u0022' + @"(\w+\s\w+))|(\d+\,?\d+\*?)|([a-zA-Z]+\*?))";
            Regex regexTerminos = new Regex(patronInicial, RegexOptions.Compiled);

            List<string> listaMatchesTerminos = new List<string>();

            foreach (Match match in regexTerminos.Matches(consulta))
            {
                listaMatchesTerminos.Add(match.Value);
            }

            // Primera pasada - Procesa términos encontrados por RegEx verificando que no se consulta el término dos veces.
            Dictionary<string, TerminoConsultaVectorial> terminosProcesados = new Dictionary<string, TerminoConsultaVectorial>();
            foreach(string matchConsulta in listaMatchesTerminos)
            {
                Console.WriteLine("Procesando match: " + matchConsulta);
                procesarTermino(matchConsulta, terminosProcesados);
            }

            // Segunda pasada - Genera la lista de consultas vectoriales.
            List<TerminoConsultaVectorial> consultasVectoriales = new List<TerminoConsultaVectorial>();
            foreach(KeyValuePair<string, TerminoConsultaVectorial> terminoProcesado in terminosProcesados)
            {
                consultasVectoriales.Add(terminoProcesado.Value);
            }

            return consultasVectoriales;
        }

        /// <summary>
        /// Ejecuta la búsqueda vectorial.
        /// </summary>
        public void ejecutar()
        {
            // Procesar consulta en bruto y calcular normal.
            List<TerminoConsultaVectorial> terminosConsulta = procesarConsulta();
           
            double normalConsulta = calcularNormalConsulta(terminosConsulta);

            // Se crea el nuevo escalafón y se le asocia a un diccionario para optimizar búsquedas no secuenciales.
            Dictionary<int, RankingDocumento> diccionarioEscalafon = generarNuevoEscalafon();

            // Se realiza el cálculo inicial: Obtener postings, sumar pesos en escalafón y obtener similitudes sin normalizar.
            agregarPesosRanking(terminosConsulta, diccionarioEscalafon);

            // Se normaliza y ordena el escalafón.
            normalizarRankingDocumentos(normalConsulta);
            ordenarRankingDocumentos();

            // OPCIONAL : Salida en consola
            Console.WriteLine("Consulta procesada.");

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
            rutaBusquedaVectorial = EntradaSalidaXml.exportarComoXml(this, rutaArchivo, rutaAbsoluta);
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
        public static BusquedaVectorial importarDesdeXml(string rutaArchivo, bool rutaAbsoluta = false)
        {
            BusquedaVectorial busquedaImportada = EntradaSalidaXml.importarDesdeXml<BusquedaVectorial>(rutaArchivo, rutaAbsoluta);
            if (!rutaAbsoluta)
            {
                rutaArchivo = Opciones.Instance.RutaArchivos + rutaArchivo;
            }

            busquedaImportada.rutaBusquedaVectorial = rutaArchivo;
            return busquedaImportada;
        }

        /// <summary>
        /// Genera un archivo HTML con el resultado de una búsqueda vectorial.
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
            // Información básica del HTML.
            string strFechaHoraBusqueda = fechaHoraBusqueda.ToString("dd/MM/yyyy hh:mm:ss.fff tt");
            string strRutaDocumentos = rutaDocumentos;
            string strTextoConsulta = consulta;

            // Obtiene la información de los primeros 30 elementos del escalafón.
            List<string[]> top30 = new List<string[]>();
            for (int pos = 0; pos < 30; pos++)
            {
                RankingDocumento rankingObtenido = rankingDocumentos[pos];
                string strPosicion = rankingObtenido.Posicion.ToString();
                string strSimilitud = rankingObtenido.Similitud.ToString("F3");
                string strDocId = rankingObtenido.IdDocumento.ToString();
                string strTaxonName = rankingObtenido.TaxonNameDocumento;
                string strTaxonRank = rankingObtenido.TaxonRank;

                Documento documento = new Documento(rankingObtenido.IdDocumento, rankingObtenido.RutaDocumento);
                documento.cargarDocumento();

                string strTaxonDescription = documento.getTaxonDescription(true);

                // Formato de cada entrada del escalafón para las primeras 30 posiciones.
                top30.Add(new string[] { strPosicion, strSimilitud, strDocId, strTaxonName, strTaxonRank, strTaxonDescription });
            }


            // Generar el html
            string html = "<head><meta charset =\"UTF-8\"></head><h1>Consulta vectorial</h1><pre>";
            html += "Fecha de la consulta vectorial: \t" + strFechaHoraBusqueda + "\n";
            html += "Ruta de la coleccion consultada: \t" + strRutaDocumentos + "\n";
            html += "Texto de la consulta: \t" + strTextoConsulta + "\n";
            foreach ( string[] doc in top30 ) {
                html += "\nID del documento: " + doc[2] + "\n";
                html += "Posicion obtenida: " + doc[0] + "\n";
                html += "Similitud: " + doc[1] + "\n";
                html += "Taxon Name: " + doc[3] + "\n";
                html += "Taxon Rank: " + doc[4] + "\n";
                html += "Taxon Description:\n" + doc[5] + "\n";
            }
            html += "</pre>";

            try {
                string fullpath =
                    Opciones.Instance.RutaConsultas +
                    Opciones.Instance.Prefijo + " Busqueda Vect " + DateTime.Now.ToString() + ".html";
                StreamWriter file = new StreamWriter(fullpath);
                file.WriteLine(html);
                file.Close();
            }
            catch ( Exception e ) {
                throw new Exception("No se ha podido crear el archivo html: \n" + e.Message);
            }
        }
    }

    /// <summary>
    /// Clase interna para estructurar cada término de la consulta vectorial.
    /// </summary>
    public class TerminoConsultaVectorial
    {
        private string termino;
        public string Termino
        {
            get { return termino; }
            set { termino = value; }
        }

        private int peso;
        public int Peso
        {
            get { return peso; }
            set { peso = value; }
        }

        public TerminoConsultaVectorial(string termino, int peso)
        {
            this.termino = termino;
            this.peso = peso;
        }
    }
}
