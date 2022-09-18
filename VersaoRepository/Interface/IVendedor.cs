using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VersaoRepository.Entities;

namespace VersaoRepository.Interface
{
    public interface IVendedor
    {
        void AddVendedor (Vendedor vendedor);
        Vendedor GetById(int id);
        IEnumerable<Vendedor> GetAll();                
    }
}