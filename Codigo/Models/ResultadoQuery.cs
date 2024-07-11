using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC6200.Models
{
    public class ResultadoQuery
    {
        public DataTable Tabla { get; set; }
        public string Resultado { get; set; }
        public string ResultadoMensaje { get; set; }
    }

   
}
