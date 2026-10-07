using Microsoft.EntityFrameworkCore;
using MotoFix.Data;
using MotoFix.Models;

namespace MotoFix.Services
{
    public class MotocicletaService : IMotocicletaService
    {
        private readonly MotoFixContext _context;

        public MotocicletaService(MotoFixContext context)
        {
            _context = context;
        }

        public async Task<List<Motocicleta>> ObtenerTodasAsync()
        {
            return await _context.Motocicletas
                .Include(m => m.Cliente)
                .ToListAsync();
        }

        public async Task<Motocicleta?> ObtenerPorIdAsync(int id)
        {
            return await _context.Motocicletas
                .Include(m => m.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CrearAsync(Motocicleta motocicleta)
        {
            _context.Motocicletas.Add(motocicleta);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Motocicleta motocicleta)
        {
            _context.Motocicletas.Update(motocicleta);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var motocicleta = await _context.Motocicletas.FindAsync(id);

            if (motocicleta != null)
            {
                _context.Motocicletas.Remove(motocicleta);
                await _context.SaveChangesAsync();
            }
        }
    }
}