using Microsoft.EntityFrameworkCore;
using MotoFix.Data;
using MotoFix.Models;

namespace MotoFix.Services
{
    public class ClienteService : IClienteService
    {
        private readonly MotoFixContext _context;

        public ClienteService(MotoFixContext context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            return await _context.Clientes
                .Include(c => c.Motocicletas)
                .ToListAsync();
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int id)
        {
            return await _context.Clientes
                .Include(c => c.Motocicletas)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CrearAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }
    }
}

//IClienteService define el contrato de las operaciones que debe ofrecer el servicio, mientras que 
//ClienteService implementa ese contrato y contiene la lógica necesaria para ejecutar esas operaciones usando MotoFixContext