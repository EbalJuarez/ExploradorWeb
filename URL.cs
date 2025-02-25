using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExploradorWeb
{
    internal class URL
    {
        public string url {  get; set; }
        public int contador { get; set; }
        public DateTime fecha { get; set; }

        public URL(string url, int contador, DateTime fecha)
        {
            this.url = url;
            this.contador = contador;
            this.fecha = fecha;
        }
    }
}
