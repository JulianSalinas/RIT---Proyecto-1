using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_RIT_v2.Clases
{
    class Invertido
    {

        public List<Termino> Diccionario;
        public List<Posting> Postings;
        public List<Documento> Documentos;
        private static Invertido instance;

        public static Invertido Instance{
            get{
                if ( instance == null )
                    instance = new Invertido();
                return instance;
            }
        }

        public Invertido() {
            Diccionario = new List<Termino>();
            Postings = new List<Posting>();
            Documentos = new List<Documento>();
        }

        /*Despues de indexar se debe guardar en archivos xml los posting y diccionario*/

        public void indexarColeccion() {
            cargarDocumentos();
            crearDiccionario();

            int currentBegin = 0;
            foreach ( Termino term in Diccionario ) {
                int currentNi = 0;
                foreach ( Documento doc in Documentos ) {
                    if ( doc.hasTermino(term.Palabra) ) {
                        currentNi++;
                        int freq = doc.countTermino(term.Palabra);
                        double peso = calcularPeso(freq, currentNi);
                        Postings.Add(new Posting(doc.Id, freq, peso));
                    }
                }
                term.Ni = currentNi;
                term.Inicio = currentBegin;
                currentBegin = currentBegin + currentNi;
            }

        }

        public double calcularPeso(int freq, int ni) {
            int N = Documentos.Count;
            return ( 1 + Math.Log(freq) ) * Math.Log((N+0.5)/(ni-0.5));
        }

        /*Lee la carpeta de la coleccion y carga el id y ruta de cada documento*/

        public void cargarDocumentos() {
            int currentId = 0;
            foreach ( string file in Directory.GetFiles(Opciones.Instance.RutaColeccion, "*.xml").ToList() ) {
                FileInfo fileInfo = new FileInfo(file);
                Documento doc = new Documento(currentId, fileInfo.FullName);
                Documentos.Add(doc);
                currentId++;
            }
        }

        /*Obtiene una sola copia de todas las palabras en la coleccion*/ 

        public void crearDiccionario() {
            foreach (Documento doc in Documentos ) 
                foreach(string termino in doc.getTerminos()) 
                    if ( !Diccionario.Exists(x => x.Palabra == termino) )
                        Diccionario.Add(new Termino(termino, 0, 0));     
        }

        /*Desde los xml que genere en la previa indexacion*/

        public void cargarDiccionario() {

        }

        public void cargarPostings() {

        }

        /*Guardar en xml para su posterior uso*/

        public void guardarDiccionario() {

        }

        public void guardarPostings() {

        }

        /*Para comprobar que se ha creado bien*/

        public override string ToString() {
            string str = "Diccionario: \n";
            str += "Palabra\t\t\tNi\t\t\tInicio\n";

            foreach ( Termino term in Diccionario )
                str += term.ToString();

            str += "\nPostings: \n";
            str += "Identif\t\t\tFreq\t\t\tPeso\n";

            foreach ( Posting post in Postings )
                str += post.ToString();

            return str;
        }


    }
}
