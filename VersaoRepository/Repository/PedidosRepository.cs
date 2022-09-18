using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VersaoRepository.Context;
using VersaoRepository.Entities;
using VersaoRepository.Interface;



namespace VersaoRepository.Repository
{
    public class PedidosRepository : IPedidos
    {
       // private readonly IList<Pedido> ListaDePedidos = new List<Pedido>();
       private readonly BdVendasContext _bdVendasContext;

       public PedidosRepository(BdVendasContext bdVendasContext)=>this._bdVendasContext=bdVendasContext;
       
        public void AddPedido(Pedido pedido)=>_bdVendasContext.TabelaPedidos.Add(pedido);

        public IEnumerable<Pedido> GetAll()=> _bdVendasContext.TabelaPedidos.ToListAsync().Result;

        public Pedido GetById(int id)=>_bdVendasContext.TabelaPedidos.SingleOrDefaultAsync(c=>c.Id==id).Result;

        void IPedidos.UpdatePedido(Pedido pedido)
        {
            _bdVendasContext.Entry(pedido).State = EntityState.Modified;
            _bdVendasContext.SaveChanges();
        }
               
    }
}