using VersaoRepository.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using VersaoRepository.Repository;
using System.Collections.Generic;
using VersaoRepository.Interface;

namespace VersaoRepository.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        //private readonly BdVendasContext _context;
        private readonly IPedidos _pedidoRepository;

        public PedidosController(IPedidos pedidoRepository)
        {
            this._pedidoRepository=pedidoRepository;
        }

        // GET: api/Pedidos --> METODO PARA LISTAR TODOS OS PEDIDOS DA TABELA PEDIDOS
        [HttpGet("ListarTodos")]
         public ActionResult<IEnumerable<Pedido>> GetTabelaPedidos()
        {
            return Ok(_pedidoRepository.GetAll());
        }

        // GET: api/Pedidos ---> METODO PARA BUSCAR PEDIDO ESPECÍFICO POR SUA ID
        [HttpGet("BuscarPor{id}")]
        public ActionResult GetById (int id)
        {
            var pedido =_pedidoRepository.GetById(id);

            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        [HttpPost("NovoPedido")]
        public ActionResult PostPedido(Pedido pedido, int idVendedor, string itens)
        {
            if(idVendedor==0)
            {
                return BadRequest("Campo Id do vendedor é necessário!");
            }
            else if (itens==null)
                return BadRequest("Você precisa adicionar pelo menos um produto!");
            try
            {
                pedido.DataDoPedido=DateTime.Now;
                pedido.DataDaUltimaAtualizacao=DateTime.Now;
                pedido.VendedorId=idVendedor;
                pedido.Istens=itens;
                pedido.StatusDisponíveis="Aguardando Pagamento, Cancelado";
                _pedidoRepository.AddPedido(pedido);
                
                return CreatedAtAction(nameof(GetById), new { id = pedido.Id }, pedido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }           
            
        }


        // PUT: api/Pedidos ---> METODO PARA ALTERAR O STATUS 
       
        [HttpPut("AlterarStatus{id}")]
        public IActionResult Put(Pedido pedido, int id, string status)
        {
            
            
            if(id==0)
                return BadRequest("Campo id é necessário para buscar o pedido!");
            if(status==null)
                return BadRequest("Informe o novo status do pedido!");
            
            
            string[] statusBd1=new string[2];
            
            //BUSCAR O PEDIDO PARA FAZER A ALTERAÇAO
            
            var pedidoBd=_pedidoRepository.GetById(id);

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
                                                              

                try
                {
                    _pedidoRepository.AddPedido(pedidoBd);

                    return Ok(pedidoBd);

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    
                        return BadRequest(ex.ToString());
                }

                
        }
            
        

        // POST: api/Pedidos
               

        
        
    }
}