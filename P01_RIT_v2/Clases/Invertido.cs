using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace P01_RIT_v2.Clases
{
    /// <summary>
    /// Clase del Archivo Invertido.
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "ArchivoInvertido")]
    public class Invertido
    {
        /// <summary>
        /// Colección de documentos.
        /// </summary>
        public List<Documento> Documentos;

        /// <summary>
        /// Términos registrados.
        /// </summary>
        public List<Termino> Diccionario;

        /// <summary>
        /// Posting de términos registrados.
        /// </summary>
        public List<Posting> Postings;

        /// <summary>
        /// Ruta del archivo invertido en almacenamiento (si el archivo invertido ya fue exportado.)
        /// </summary>
        public string rutaArchivoInvertido;

        /// <summary>
        /// Instancia reservada para usar patrón Singleton.
        /// </summary>
        [NonSerialized]
        private static Invertido instance;

        [XmlIgnore]
        public static Invertido Instance
        {
            get
            {
                if ( instance == null )
                    instance = new Invertido();
                return instance;
            }
        }

        /// <summary>
        /// Constructor por defecto. Necesario para serializar y deserializar archivo invertido.
        /// </summary>
        public Invertido() {
            Diccionario = new List<Termino>();
            Postings = new List<Posting>();
            Documentos = new List<Documento>();
        }


        /// <summary>
        /// Genera los Postings del archivo invertido.
        /// </summary>
        public void indexarColeccion() {
            cargarDocumentos();
            crearDiccionario();

            int currentBegin = 0;
            foreach (Termino term in Diccionario) {
                int currentNi = 0;
                term.Ni = calcularNi(term);
                foreach (Documento doc in Documentos) {
                    if (doc.hasTermino(term.Contenido)) {
                        currentNi++;
                        int freq = doc.countTermino(term.Contenido);
                        double peso = calcularPeso(term, doc);
                        Postings.Add(new Posting(doc.Id, freq, peso));
                    }
                }
                term.Inicio = currentBegin;
                currentBegin = currentBegin + currentNi;
            }

        }

        /// <summary>
        /// Calcula el valor Ni: Apariciones de un término en diferentes documentos.
        /// </summary>
        /// <param name="term">Término a consultar.</param>
        /// <returns>Cantidad de documentos donde aparece el término.</returns>
        private int calcularNi(Termino term) {
            int Ni = 0;
            foreach (Documento doc in Documentos)
                if (doc.hasTermino(term.Contenido))
                    Ni++;
            return Ni;
        }

        /// <summary>
        /// Calcula el peso correspondiente para un término asociado a un documento.
        /// </summary>
        /// <param name="term">Término registrado.</param>
        /// <param name="doc">Documento registrado.</param>
        /// <returns>Peso calculado para el término en el documento.</returns>
        private double calcularPeso(Termino term, Documento doc) {
            int N = Documentos.Count;
            int frecuencia = doc.countTermino(term.Contenido);
            int Ni = term.Ni;

            // Fórmula a utilizar según especificación del programa.
            return (1 + (Math.Log(frecuencia, 2) * Math.Log((N / Ni), 2)));
        }
        
        /// <summary>
        /// Lee la carpeta de la coleccion y carga el id y ruta de cada documento.
        /// </summary>
        private void cargarDocumentos() {
            int currentId = 0;
            foreach (string file in Directory.GetFiles(Opciones.Instance.RutaColeccion, "*.xml").ToList()) {
                FileInfo fileInfo = new FileInfo(file);
                Documento doc = new Documento(currentId, fileInfo.FullName);
                Documentos.Add(doc);
                currentId++;
            }
        }

        /// <summary>
        /// Obtiene una sola copia de todas las palabras en la coleccion.
        /// </summary>
        private void crearDiccionario() {
            foreach (Documento doc in Documentos)
                foreach (string termino in doc.getTerminos())
                    if (!Diccionario.Exists(x => x.Contenido == termino))
                        Diccionario.Add(new Termino(termino, 0, 0));
        }

        /// <summary>
        /// Exporta el archivo invertido creado en un String. El archivo será guardado en la carpeta por defecto del programa.
        /// </summary>
        /// <param name="nombreArchivoPostings">
        /// Nombre del archivo XML que será creado. No insertar extensión.
        /// Si no se ingresa un nombre se utiliza la fecha y hora del sistema. 
        /// </param>
        public static void exportarArchivoInvertido(String nombreArchivoPostings = "")
        {
            // Regex para verificar que archivo tiene nombre válido.
            Regex caracteresInvalidos = new Regex("[" + Regex.Escape(System.IO.Path.GetInvalidFileNameChars().ToString()) + "]");

            if (nombreArchivoPostings == "")
            {
                nombreArchivoPostings = System.DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss");
            }
            else if (caracteresInvalidos.IsMatch(nombreArchivoPostings))
            {
                throw new Exception("El nombre de archivo ingresado no es válido.");
            }
            nombreArchivoPostings += ".xml";
            try
            {
                System.Xml.Serialization.XmlSerializer serializador = new System.Xml.Serialization.XmlSerializer(typeof(Invertido));
                System.IO.FileStream archivoSalida = System.IO.File.Create(Opciones.Instance.RutaArchivos + nombreArchivoPostings);
                serializador.Serialize(archivoSalida, instance);
                archivoSalida.Close();
            }
            catch (System.IO.PathTooLongException e)
            {
                throw new Exception("La ruta del archivo tiene un nombre muy largo.");
            }
            catch (NotSupportedException e)
            {
                throw new Exception("No hay soporte para crear el archivo.");
            }
            catch (IOException e)
            {
                throw new Exception("Error al crear el archivo.");
            }
        }

        /// <summary>
        /// Importa el contenido de un archivo XML para generar un Archivo Invertido. El archivo debe estar guardado en la carpeta por defecto del programa.
        /// </summary>
        /// <param name="nombreArchivoPostings">
        /// Nombre del archivo XML a cargar. El nombre debe tener la extensión (.xml).
        /// </param>
        /// <returns></returns>
        public static void importarArchivoInvertido(string nombreArchivoPostings)
        {
            // Regex para verificar que archivo tiene nombre válido.
            Regex caracteresInvalidos = new Regex("[" + Regex.Escape(System.IO.Path.GetInvalidFileNameChars().ToString()) + "]");

            if (nombreArchivoPostings == "")
            {
                throw new Exception("El nombre del archivo no puede estar vacío.");
            }
            else if (caracteresInvalidos.IsMatch(nombreArchivoPostings))
            {
                throw new Exception("El nombre de archivo ingresado no es válido.");
            }
            try
            {
                System.Xml.Serialization.XmlSerializer deserializador = new System.Xml.Serialization.XmlSerializer(typeof(Invertido));
                System.IO.FileStream archivoEntrada = System.IO.File.OpenRead(Opciones.Instance.RutaArchivos + nombreArchivoPostings);
                instance = (Invertido)deserializador.Deserialize(archivoEntrada);
                archivoEntrada.Close();
            }
            catch (FileNotFoundException e)
            {
                throw new Exception ("El archivo XML no existe.");
            }
            catch (InvalidOperationException e)
            {
                throw new Exception("El archivo XML ingresado no contiene la estructura de un Archivo Invertido.");
            }
            catch (IOException e)
            {
                throw new Exception("Error al leer el archivo.");
            }
        }

        /// <summary>
        /// Exporta el Archivo Invertido a un String. Utilizar para probar si el Archivo Invertido fue creado correctamente.
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            string str = "Diccionario: \n";
            str += "Inicio\t\t\tNi\t\t\tPalabra\n";

            foreach (Termino term in Diccionario)
                str += term.ToString();

            str += "\nPostings: \n";
            str += "Identif\t\t\tFreq\t\t\tPeso\n";

            foreach (Posting post in Postings)
                str += post.ToString();

            return str;
        }

        /// <summary>
        /// Genera una nueva instancia de un Archivo Invertido con las colecciones por defecto, utilizando las carpetas del programa.
        /// </summary>
        /// <returns>
        /// Archivo invertido generado.
        /// </returns>
        public static Invertido generarArchivoInvertidoPorDefecto()
        {
            try
            {
                Invertido nuevoInvertido = new Invertido();
                nuevoInvertido.indexarColeccion();
                return nuevoInvertido;
            }
            catch(Exception e)
            {
                throw new Exception("No se pudo crear el archivo invertido por defecto.");
            }
        }

        /// <summary>
        /// Busca la entrada del término en el Archivo Invertido.
        /// </summary>
        /// <param name="terminoBusqueda">
        /// Término a buscar.
        /// </param>
        /// <returns>
        /// Objeto Termino encontrado. Null si el término no existe.
        /// </returns>
        public Termino obtenerTerminoExacto(string terminoBusqueda)
        {
            foreach (Termino terminoEncontrado in Diccionario)
            {
                if (terminoEncontrado.Contenido.Equals(terminoBusqueda))
                {
                    return terminoEncontrado;
                }
            }
            return null;
        }

        /// <summary>
        /// Obtiene una lista con los términos que comparten un prefijo.
        /// </summary>
        /// <param name="prefijo">
        /// Prefijo de los términos buscados.
        /// </param>
        /// <returns>
        /// Lista con objetos Término encontrados. Si no se encontró ningún término, la lista estará vacía.
        /// </returns>
        public List<Termino> obtenerTerminosConPrefijo(string prefijo)
        {
            if (prefijo.Equals("") || prefijo == null)
            {
                return null;
            }
            else
            {
                String strRegex = "^(" + prefijo + ")";
                Regex regex = new Regex(strRegex, RegexOptions.Compiled);

                List<Termino> listaTerminos = new List<Termino>();
                
                foreach(Termino terminoEncontrado in Diccionario)
                {
                    if (regex.IsMatch(terminoEncontrado.Contenido))
                    {
                        listaTerminos.Add(terminoEncontrado);
                    }
                }
                return listaTerminos;
            }
        }
        
        /// <summary>
        /// Obtiene la lista de Postings asociados al registro del término.
        /// </summary>
        /// <param name="entradaTermino">
        /// Objeto Término registrado.
        /// </param>
        /// <returns>
        /// Lista de Postings del término.
        /// </returns>
        List<Posting> obtenerPostingsTerminoExacto(Termino entradaTermino)
        {
            if (entradaTermino == null)
            {
                return null;
            }
            else
            {
                List<Posting> postingsTermino = new List<Posting>();

                for (int posicionBusqueda = entradaTermino.Inicio;
                        posicionBusqueda < entradaTermino.Inicio + entradaTermino.Ni;
                        posicionBusqueda++)
                {
                    postingsTermino.Add(Postings.ElementAt(posicionBusqueda));
                }
                return postingsTermino;
            }
        }
        
        /// <summary>
        /// Obtiene los Postings de un término buscado en el Archivo Invertido.
        /// </summary>
        /// <param name="terminoBusqueda">
        /// Término a buscar dentro del Archivo Invertido.
        /// </param>
        /// <returns>
        /// Lista de Postings del término. Si el término no pudo ser encontrado, se retorna Null.
        /// </returns>
        public List<Posting> obtenerPostingsTerminoExacto(string terminoBusqueda)
        {
            if ((terminoBusqueda == null) || (terminoBusqueda.Equals(""))) {
                return null;
            }

            Termino terminoEncontrado = obtenerTerminoExacto(terminoBusqueda);
            return obtenerPostingsTerminoExacto(terminoEncontrado);
        }
    }
}
