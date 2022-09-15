using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiVendasPottencial.Context;
using ApiVendasPottencial.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiVendasPottencial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly BdVendasContext _context;

        public PedidosController(BdVendasContext context)
        {
            _context = context;
        }

        // GET: api/Pedidos --> METODO PARA LISTAR TODOS OS PEDIDOS DA TABELA PEDIDOS
        [HttpGet("ListarTodos")]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetTabelaPedidos()
        {
            return await _context.TabelaPedidos.ToListAsync();
        }

        // GET: api/Pedidos ---> METODO PARA BUSCAR PEDIDO ESPECÍFICO POR SUA ID
        [HttpGet("BuscarPor{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _context.TabelaPedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            return pedido;
        }

        // PUT: api/Pedidos ---> METODO PARA ALTERAR O STATUS 
        
        [HttpPut("AlterarStatus{id}")]
        public async Task<IActionResult> PutPedido(int id, string status, Pedido pedido)
        {
            
            if(id==0)
                return BadRequest("Campo id é necessário para buscar o pedido!");
            if(status==null)
                return BadRequest("Informe o novo status do pedido!");
            string[] statusBd1=new string[2];
            
            //BUSCAR O PEDIDO PARA FAZER A ALTERAÇAO
            Pedido pedidoBd = await _context.TabelaPedidos.FindAsync(id);

            //SE O PEDIDO ESTIVER EM ENVIADO PARA A TRANSPORTADORA OU CANCELADO NÃO SERÁ POSSÍVEL FAZER ALTERAÇÃO

            if(pedidoBd.StatusDisponíveis.ToUpper() == "ENVIADO PARA TRANSPORTADORA" || pedidoBd.StatusDisponíveis.ToUpper() == "CANCELADO")
            {
                return BadRequest($"O pedido em tela não aceita modificações no seu status: {pedidoBd.StatusDoPedido}");
            }
            statusBd1 = pedidoBd.StatusDisponíveis.Split(",");    
            statusBd1[0].Trim();
            statusBd1[1].Trim();
            
            
                /* 
            
                    VERIFICA AS POSSIBILIDADES DE ALTERAÇÃO CONFORME O ESQUEMA:

                
                        guardando pagamento Para: Pagamento Aprovado
                        De: Aguardando pagamento Para: Cancelada
                        De: Pagamento Aprovado Para: Enviado para Transportadora
                        De: Pagamento Aprovado Para: Cancelada
                        De: Enviado para Transportador. Para: Entregue

                */

            if(status.ToUpper() != statusBd1[0].ToUpper() && status.ToUpper() != statusBd1[1].ToUpper()  )
            {
                 return BadRequest($"O pedido em tela só aceita um dos status: {statusBd1[0]} ou {statusBd1[1]}");
            }
                DateTime data=DateTime.Now;
                
                
                data.ToShortDateString();
                pedidoBd.StatusDoPedido=status;
                pedidoBd.DataDaUltimaAtualizacao=data;
                if(status.ToUpper() == "PAGAMENTO APROVADO")
                {
                    pedidoBd.StatusDisponíveis="Enviado para Transportadora, Cancelado";
                }else if (status.ToUpper() == "ENVIADO PARA TRANSPORTADORA")
                {
                    pedidoBd.StatusDisponíveis="Entregue";
                }
                
                _context.Entry(pedidoBd).State = EntityState.Modified;
                

                try
                {
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetPedido", new { id = pedido.Id }, pedido);

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!PedidoExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        return BadRequest(ex.ToString());
                    }
                }

                return NoContent();
        }
            
        

        // POST: api/Pedidos
        
        [HttpPost("NovoPedido")]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido, int vendedorId, string itens)
        {
            if(vendedorId==0)
            {
                return BadRequest("Campo Id do vendedor é necessário!");
            }
           
            DateTime data = DateTime.Now;
            data.ToShortDateString();
            pedido.VendedorId = vendedorId;
            pedido.StatusDoPedido = "Aguardando Pagamento";
            pedido.StatusDisponíveis = "Pagamento Aprovado, Cancelado";
            pedido.DataDoPedido=data;
            pedido.DataDaUltimaAtualizacao = data;
            pedido.Istens = itens;

            try
            {
                _context.TabelaPedidos.Add(pedido);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetPedido", new { id = pedido.Id }, pedido);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.ToString());
            }           
            
        }

        
        private bool PedidoExists(int id)
        {
            return _context.TabelaPedidos.Any(e => e.Id == id);
        }
    }
}