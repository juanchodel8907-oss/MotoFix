using MotoFix.Models;

namespace MotoFix.Services
{
    public interface IMotocicletaService
    {
        Task<List<Motocicleta>> ObtenerTodasAsync();
        Task<Motocicleta?> ObtenerPorIdAsync(int id);
        Task CrearAsync(Motocicleta motocicleta);
        Task ActualizarAsync(Motocicleta motocicleta);
        Task EliminarAsync(int id);
    }
}