using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VersaoRepository.Context;
using VersaoRepository.Entities;
using VersaoRepository.Interface;
using Microsoft.EntityFrameworkCore;

namespace VersaoRepository.Repository
{
    public class VendedoresRepository : IVendedor
    {
        //public IList<Vendedor> ListaDeVendedores = new List<Vendedor>();
        private readonly BdVendasContext _bdVendasContext;
       public VendedoresRepository(BdVendasContext bdVendasContext)=>this._bdVendasContext=bdVendasContext;
       
        public void AddVendedor(Vendedor vendedor)=>_bdVendasContext.TabelaVendedores.Add(vendedor);

        public IEnumerable<Vendedor> GetAll()=> _bdVendasContext.TabelaVendedores.ToListAsync().Result;

        public Vendedor GetById(int id)=>_bdVendasContext.TabelaVendedores.SingleOrDefaultAsync(c=>c.Id==id).Result;

        


    }
}