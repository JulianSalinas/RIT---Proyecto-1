using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_RIT_v2.Clases
{
    class Termino
    {
        public string Palabra;
        public int Ni;
        public int Inicio;

        public Termino( string palabra, int ni, int inicio ) {
            this.Palabra = palabra;
            this.Ni = ni;
            this.Inicio = inicio;
        }

        public override string ToString() {
            return Inicio.ToString() +"\t\t\t"+ Ni.ToString() +"\t\t\t"+ Palabra + "\n";
        }

    }
}
