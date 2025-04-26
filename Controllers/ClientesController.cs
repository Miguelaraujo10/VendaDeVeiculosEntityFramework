using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoCompletoLocadora.Data;
using ProjetoCompletoLocadora.Models;

namespace ProjetoCompletoLocadora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            if (_context.Clientes == null)
            {
                return NotFound();
            }
            return await _context.Clientes.ToListAsync();
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            if (_context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Alugueis)  // JOIN com Alugueis
                .ThenInclude(a => a.Veiculo)  // JOIN com Veiculo
                .ThenInclude(v => v.Fabricante)  // JOIN com Fabricante
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        // Filtro 1: Buscar clientes com alugueis e seus veículos
        [HttpGet("filtro1")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientesComAlugueisEVeiculos()
        {
            var clientes = await _context.Clientes
                .Include(c => c.Alugueis)  // JOIN com Alugueis
                .ThenInclude(a => a.Veiculo)  // JOIN com Veiculo
                .ToListAsync();

            if (clientes == null)
            {
                return NotFound();
            }

            return clientes;
        }

        // Filtro 2: Buscar clientes por nome
        [HttpGet("filtro2")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientesPorNome(string nome)
        {
            var clientes = await _context.Clientes
                .Where(c => c.Nome.Contains(nome))  // Filtro por nome
                .Include(c => c.Alugueis)  // JOIN com Alugueis
                .ToListAsync();

            if (clientes == null)
            {
                return NotFound();
            }

            return clientes;
        }

        // Filtro 3: Buscar clientes com um determinado número de alugueis
        [HttpGet("filtro3")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientesComAlugueisMaiorQue(int numeroAlugueis)
        {
            var clientes = await _context.Clientes
                .Where(c => c.Alugueis.Count() >= numeroAlugueis)  // Filtro de quantidade de alugueis
                .Include(c => c.Alugueis)  // JOIN com Alugueis
                .ToListAsync();

            if (clientes == null)
            {
                return NotFound();
            }

            return clientes;
        }

        // Filtro 4: Buscar clientes e seus alugueis com dados do veiculo
        [HttpGet("filtro4")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientesComAlugueisEVeiculosEFabricante()
        {
            var clientes = await _context.Clientes
                .Include(c => c.Alugueis)  // JOIN com Alugueis
                .ThenInclude(a => a.Veiculo)  // JOIN com Veiculo
                .ThenInclude(v => v.Fabricante)  // JOIN com Fabricante
                .ToListAsync();

            if (clientes == null)
            {
                return NotFound();
            }

            return clientes;
        }

        // Filtro 5: Buscar clientes com um aluguel específico
        [HttpGet("filtro5")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientesComAluguelId(int aluguelId)
        {
            var clientes = await _context.Clientes
                .Where(c => c.Alugueis.Any(a => a.Id == aluguelId))  // Filtro de aluguel específico
                .Include(c => c.Alugueis)  // JOIN com Alugueis
                .ThenInclude(a => a.Veiculo)  // JOIN com Veiculo
                .ToListAsync();

            if (clientes == null)
            {
                return NotFound();
            }

            return clientes;
        }

        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest();
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Clientes
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            if (_context.Clientes == null)
            {
                return Problem("Entity set 'AppDbContext.Clientes'  is null.");
            }
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCliente", new { id = cliente.Id }, cliente);
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            if (_context.Clientes == null)
            {
                return NotFound();
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClienteExists(int id)
        {
            return (_context.Clientes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
