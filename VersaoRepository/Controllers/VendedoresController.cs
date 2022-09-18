using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VersaoRepository.Context;
using VersaoRepository.Entities;
using VersaoRepository.Interface;

namespace ApiVendasPottencial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendedoresController : ControllerBase
    {
        private readonly IVendedor _ivendedor;

        public VendedoresController(IVendedor ivendedor)
        {
            _ivendedor=ivendedor;
        }

        // GET: api/Vendedores
        [HttpGet("ListarTodos")]
        public async Task<ActionResult<IEnumerable<Vendedor>>> GetTabelaVendedores()
        {
            return Ok (_ivendedor.GetAll());
        }

        // GET: api/Vendedores/5
        [HttpGet("BuscarPor{id}")]
        public async Task<ActionResult<Vendedor>> GetVendedor(int id)
        {
            var vendedor = _ivendedor.GetById(id);

            if (vendedor == null)
            {
                return NotFound();
            }

            return vendedor;
        }
           



        // POST: api/Vendedores --> METODO PARA INSERIR UM NOVO VENDEDOR NO BD
       
        [HttpPost("NovoVendedor")]
        public async Task<ActionResult<Vendedor>> PostVendedor(Vendedor vendedor, string nome, string cpf, string telefone)
        {
            if(nome==null) return BadRequest("Informe o nome do vendedor!");
            if(cpf==null) return BadRequest("Informe o CPF do vendedor!");
            if(telefone==null) return BadRequest("Informe o telefone!");
            vendedor.Cpf=cpf;
            vendedor.Nome=nome;
            vendedor.Telefone=telefone;
            
            try{
                    _ivendedor.AddVendedor(vendedor);                    

                    return CreatedAtAction("GetVendedor", new { id = vendedor.Id }, vendedor);

                }catch (DbUpdateConcurrencyException ex)
                {
                        return BadRequest(ex.ToString());
                }    
            
        }
                
    }
}