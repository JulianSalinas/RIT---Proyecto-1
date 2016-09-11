using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_RIT_v2.Clases
{
    class Posting
    {
        public int DocId;
        public int Frecuencia;
        public int Peso;

        public Posting( int docId, int frecuencia, int peso ) {
            this.DocId = docId;
            this.Frecuencia = frecuencia;
            this.Peso = peso;
        }
    }
}
