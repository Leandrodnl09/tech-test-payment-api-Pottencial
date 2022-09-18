using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VersaoRepository.Entities;

namespace VersaoRepository.Interface
{
    public interface IPedidos
    {
       public Pedido GetById(int id);
       public IEnumerable<Pedido> GetAll();
       public void AddPedido(Pedido pedido);
       public void UpdatePedido(Pedido pedido);
       
    }
}