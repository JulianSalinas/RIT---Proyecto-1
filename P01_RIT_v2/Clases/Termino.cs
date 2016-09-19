using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace P01_RIT_v2.Clases
{

    [Serializable]
    public class Termino
    {
        public string Contenido;
        public int Ni;
        public int Inicio;

        public Termino()
        {
            Contenido = "";
            Ni = 0;
            Inicio = -1;
        }
        public Termino( string palabra, int ni, int inicio ) {
            this.Contenido = palabra;
            this.Ni = ni;
            this.Inicio = inicio;
        }

        public override string ToString() {
            return Inicio.ToString() +"\t\t\t"+ Ni.ToString() +"\t\t\t"+ Contenido + "\n";
        }

    }
}
