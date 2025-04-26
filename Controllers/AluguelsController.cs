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
    public class AluguelsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AluguelsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Aluguels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aluguel>>> GetAlugueis()
        {
            if (_context.Alugueis == null)
            {
                return NotFound();
            }
            return await _context.Alugueis
                .Include(a => a.Cliente)   // INNER JOIN com Cliente
                .Include(a => a.Veiculo)   // INNER JOIN com Veiculo
                .ToListAsync();
        }

        // GET: api/Aluguels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Aluguel>> GetAluguel(int id)
        {
            if (_context.Alugueis == null)
            {
                return NotFound();
            }

            var aluguel = await _context.Alugueis
                .Include(a => a.Cliente)   // INNER JOIN com Cliente
                .Include(a => a.Veiculo)   // INNER JOIN com Veiculo
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aluguel == null)
            {
                return NotFound();
            }

            return aluguel;
        }

        // Filtro 1: Buscar todos os alugueis de um cliente
        [HttpGet("filtro1")]
        public async Task<ActionResult<IEnumerable<Aluguel>>> GetAlugueisPorCliente(int clienteId)
        {
            var alugueis = await _context.Alugueis
                .Where(a => a.ClienteId == clienteId)
                .Include(a => a.Cliente)  // INNER JOIN com Cliente
                .Include(a => a.Veiculo)  // INNER JOIN com Veiculo
                .ToListAsync();

            if (alugueis == null || !alugueis.Any())
            {
                return NotFound();
            }

            return alugueis;
        }

        // Filtro 2: Buscar todos os veículos alugados em um determinado período
        [HttpGet("filtro2")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculosPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            var veiculos = await _context.Alugueis
                .Where(a => a.DataInicio >= dataInicio && a.DataFim <= dataFim)
                .Include(a => a.Veiculo)  // INNER JOIN com Veiculo
                .Select(a => a.Veiculo)
                .ToListAsync();

            if (veiculos == null || !veiculos.Any())
            {
                return NotFound();
            }

            return veiculos;
        }

        // Filtro 3: Buscar todos os veículos e seus respectivos fabricantes
        [HttpGet("filtro3")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculosComFabricantes()
        {
            var veiculos = await _context.Veiculos
                .Include(v => v.Fabricante)  // LEFT JOIN com Fabricante
                .ToListAsync();

            if (veiculos == null || !veiculos.Any())
            {
                return NotFound();
            }

            return veiculos;
        }

        // Filtro 4: Buscar todos os alugueis com informações do cliente e do veículo
        [HttpGet("filtro4")]
        public async Task<ActionResult<IEnumerable<Aluguel>>> GetAlugueisComClientesEVeiculos()
        {
            var alugueis = await _context.Alugueis
                .Include(a => a.Cliente)  // LEFT JOIN com Cliente
                .Include(a => a.Veiculo)  // LEFT JOIN com Veiculo
                .ToListAsync();

            if (alugueis == null || !alugueis.Any())
            {
                return NotFound();
            }

            return alugueis;
        }

        // Filtro 5: Buscar todos os veículos de uma categoria específica
        [HttpGet("filtro5")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculosPorCategoria(int categoriaId)
        {
            var veiculos = await _context.Veiculos
                .Where(v => v.CategoriaId == categoriaId)
                .Include(v => v.Categoria)  // INNER JOIN com Categoria
                .ToListAsync();

            if (veiculos == null || !veiculos.Any())
            {
                return NotFound();
            }

            return veiculos;
        }

        // PUT: api/Aluguels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAluguel(int id, Aluguel aluguel)
        {
            if (id != aluguel.Id)
            {
                return BadRequest();
            }

            _context.Entry(aluguel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AluguelExists(id))
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

        // POST: api/Aluguels
        [HttpPost]
        public async Task<ActionResult<Aluguel>> PostAluguel(Aluguel aluguel)
        {
            if (_context.Alugueis == null)
            {
                return Problem("Entity set 'AppDbContext.Alugueis'  is null.");
            }
            _context.Alugueis.Add(aluguel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAluguel", new { id = aluguel.Id }, aluguel);
        }

        // DELETE: api/Aluguels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAluguel(int id)
        {
            if (_context.Alugueis == null)
            {
                return NotFound();
            }
            var aluguel = await _context.Alugueis.FindAsync(id);
            if (aluguel == null)
            {
                return NotFound();
            }

            _context.Alugueis.Remove(aluguel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AluguelExists(int id)
        {
            return (_context.Alugueis?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
