using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiVendasPottencial.Entities
{
    public class Pedido
    {
        
        public int Id { get; set; }
        public DateTime DataDoPedido { get; set; }
        public int VendedorId { get; set; }
        public string Istens { get; set; }
        public string StatusDisponíveis { get; set; }
        public string StatusDoPedido { get; set; }
        public DateTime DataDaUltimaAtualizacao { get; set; }
         
    }
}