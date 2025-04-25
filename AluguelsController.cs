using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoCompletoLocadora.Data;
using ProjetoCompletoLocadora.Models;

namespace ProjetoCompletoLocadora
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
            return await _context.Alugueis.ToListAsync();
        }

        // GET: api/Aluguels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Aluguel>> GetAluguel(int id)
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

            return aluguel;
        }

        // PUT: api/Aluguels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
