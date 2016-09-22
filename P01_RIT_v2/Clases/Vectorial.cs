using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P01_RIT_v2.Clases
{
    public class TerminoConsulta
    {
        public string Contenido;
        public int Peso;
    }

    public class Ranking
    {
        public DateTime FechaCreacion;
        public string RutaColeccion;
        public string Consulta;
        public List<Documento>
        Documentos = new List<Documento>();
    }

    public class Vectorial
    {
        public DateTime FechaCreacion;
        public string RutaColeccion;
        public string Consulta;
        public Ranking Ranking;

        public Vectorial( string StringConsulta ) {
            this.FechaCreacion = DateTime.Now;
            this.RutaColeccion = Opciones.Instance.RutaColeccion;
            this.Consulta = StringConsulta;
            this.Ranking = new Ranking();
        }

        /* El primer sub patron toma lo que posee frases
        *  El segun toma los que tiene * al final (prefijos)
        *  El tercero toma los demas terminos que sobran
        *  Se toman con todo y el simbolo "+" ó "-" ó ninguno */

        public List<TerminoConsulta> procesarTerminosDesdeConsulta() {
            string patron = @"((\+|\-|)("+'\u0022'+@"\w+\s\w+))|(\+|\-|)(\w*\*)|(\+|\-|)(\w+)";
            Regex regexTerminos = new Regex(patron, RegexOptions.Compiled);
            List<TerminoConsulta> terminos = new List<TerminoConsulta>();
            foreach ( Match match in regexTerminos.Matches(Consulta) ) {
                TerminoConsulta termino = new TerminoConsulta() {
                    Peso = calcularImportancia(match.Value),
                    Contenido = normalizarTermino(match.Value)
                };
                terminos.Add(termino);
            }
            return terminos;
        }

        private int calcularImportancia(string termino) {
            if ( termino[0] == '+' )
                return 4;
            else if ( termino[0] == '-' )
                return 1;
            else
                return 2;
        }

        /*Se conserva el simbolo * para los terminos que se buscaran por prefijo*/
        private string normalizarTermino(string termino) {
            string patronImportancia = @"[\+|\-|\" + '\u0022' + "]+";
            string patronEspacios = @"\s+";
            Regex regexImportancia = new Regex(patronImportancia, RegexOptions.Compiled);
            Regex regexEspacios = new Regex(patronEspacios, RegexOptions.Compiled);
            termino = termino.ToLower();
            termino = Stopwords.Instance.quitarAcentos(termino);
            termino = regexImportancia.Replace(termino, "");
            termino = regexEspacios.Replace(termino, " ");
            return termino;
        }

        public void buscarTermino(TerminoConsulta terminoConsulta) {

        }


        /*Voy hasta aqui*/
        public Termino obtenerTerminoExacto( string terminoBusqueda ) {
            foreach ( Termino terminoEncontrado in Invertido.Instance.Diccionario ) {
                if ( terminoEncontrado.Contenido.Equals(terminoBusqueda) ) {
                    return terminoEncontrado;
                }
            }
            return null;
        }

        public List<Termino> obtenerTerminosConPrefijo( string prefijo ) {
            if ( prefijo.Equals("") || prefijo == null ) {
                return null;
            }
            else {
                string strRegex = "^(" + prefijo + ")";
                Regex regex = new Regex(strRegex, RegexOptions.Compiled);

                List<Termino> listaTerminos = new List<Termino>();

                foreach ( Termino terminoEncontrado in Invertido.Instance.Diccionario ) {
                    if ( regex.IsMatch(terminoEncontrado.Contenido) ) {
                        listaTerminos.Add(terminoEncontrado);
                    }
                }
                return listaTerminos;
            }
        }

        public List<Posting> obtenerPostingsTerminoExacto( Termino entradaTermino ) {
            if ( entradaTermino == null ) {
                return null;
            }
            else {
                List<Posting> postingsTermino = new List<Posting>();

                for ( int posicionBusqueda = entradaTermino.Inicio ;
                        posicionBusqueda < entradaTermino.Inicio + entradaTermino.Ni ;
                        posicionBusqueda++ ) {
                    postingsTermino.Add(Invertido.Instance.Postings.ElementAt(posicionBusqueda));
                }
                return postingsTermino;
            }
        }

        public List<Posting> obtenerPostingsTerminoExacto( string terminoBusqueda ) {
            if ( ( terminoBusqueda == null ) || ( terminoBusqueda.Equals("") ) ) {
                return null;
            }

            Termino terminoEncontrado = obtenerTerminoExacto(terminoBusqueda);
            return obtenerPostingsTerminoExacto(terminoEncontrado);
        }

    }
}
