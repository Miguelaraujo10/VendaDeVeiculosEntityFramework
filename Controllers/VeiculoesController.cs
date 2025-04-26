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
    public class VeiculoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VeiculoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Veiculoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculos()
        {
            if (_context.Veiculos == null)
            {
                return NotFound();
            }
            return await _context.Veiculos.ToListAsync();
        }

        // GET: api/Veiculoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Veiculo>> GetVeiculo(int id)
        {
            if (_context.Veiculos == null)
            {
                return NotFound();
            }
            var veiculo = await _context.Veiculos
                .Include(v => v.Fabricante)  // JOIN com Fabricante
                .Include(v => v.Categoria)   // JOIN com Categoria
                .FirstOrDefaultAsync(v => v.Id == id);

            if (veiculo == null)
            {
                return NotFound();
            }

            return veiculo;
        }

        // Filtro 1: Buscar veículos com fabricante
        [HttpGet("filtro1")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculosComFabricante()
        {
            var veiculos = await _context.Veiculos
                .Include(v => v.Fabricante)  // JOIN com Fabricante
                .ToListAsync();

            if (veiculos == null)
            {
                return NotFound();
            }

            return veiculos;
        }

        // Filtro 2: Buscar veículos por modelo
        [HttpGet("filtro2")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculosPorModelo(string modelo)
        {
            var veiculos = await _context.Veiculos
                .Where(v => v.Modelo.Contains(modelo))  // Filtro por modelo
                .Include(v => v.Fabricante)  // JOIN com Fabricante
                .ToListAsync();

            if (veiculos == null)
            {
                return NotFound();
            }

            return veiculos;
        }

        // Filtro 3: Buscar veículos por ano de fabricação
        [HttpGet("filtro3")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculosPorAno(int ano)
        {
            var veiculos = await _context.Veiculos
                .Where(v => v.AnoFabricacao == ano)  // Filtro por ano
                .Include(v => v.Fabricante)  // JOIN com Fabricante
                .Include(v => v.Categoria)   // JOIN com Categoria
                .ToListAsync();

            if (veiculos == null)
            {
                return NotFound();
            }

            return veiculos;
        }

        // Filtro 4: Buscar veículos com quilometragem maior que X
        [HttpGet("filtro4")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculosComQuilometragemMaiorQue(int quilometragem)
        {
            var veiculos = await _context.Veiculos
                .Where(v => v.Quilometragem > quilometragem)  // Filtro de quilometragem
                .Include(v => v.Fabricante)  // JOIN com Fabricante
                .ToListAsync();

            if (veiculos == null)
            {
                return NotFound();
            }

            return veiculos;
        }

        // Filtro 5: Buscar veículos com categoria específica
        [HttpGet("filtro5")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculosComCategoria(int categoriaId)
        {
            var veiculos = await _context.Veiculos
                .Where(v => v.CategoriaId == categoriaId)  // Filtro por categoria
                .Include(v => v.Categoria)  // JOIN com Categoria
                .Include(v => v.Fabricante)  // JOIN com Fabricante
                .ToListAsync();

            if (veiculos == null)
            {
                return NotFound();
            }

            return veiculos;
        }

        // PUT: api/Veiculoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVeiculo(int id, Veiculo veiculo)
        {
            if (id != veiculo.Id)
            {
                return BadRequest();
            }

            _context.Entry(veiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VeiculoExists(id))
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

        // POST: api/Veiculoes
        [HttpPost]
        public async Task<ActionResult<Veiculo>> PostVeiculo(Veiculo veiculo)
        {
            if (_context.Veiculos == null)
            {
                return Problem("Entity set 'AppDbContext.Veiculos' is null.");
            }
            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVeiculo", new { id = veiculo.Id }, veiculo);
        }

        // DELETE: api/Veiculoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeiculo(int id)
        {
            if (_context.Veiculos == null)
            {
                return NotFound();
            }
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VeiculoExists(int id)
        {
            return (_context.Veiculos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
