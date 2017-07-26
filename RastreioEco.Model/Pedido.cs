using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RastreioEco.Model
{
    public class Pedido
    {
        public bool Checado { get; set; }
        public string PedidoWeb { get; set; }
        public string PedidoOriginal { get; set; }
        public int Notas { get; set; }
        public string NfeChv { get; set; }
        public string NfeXML { get; set; }
        public decimal Valos { get; set; }
        public string ChkRastreio { get; set; }
    }
}
