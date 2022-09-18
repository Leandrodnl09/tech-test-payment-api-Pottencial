using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VersaoRepository.Entities;

namespace VersaoRepository.Context
{
    public class BdVendasContext : DbContext 
    {
        public BdVendasContext(DbContextOptions<BdVendasContext> options) : base(options)
            { }
        public DbSet<Vendedor> TabelaVendedores { get; set; }
        public DbSet<Pedido> TabelaPedidos { get; set;}
    }
}