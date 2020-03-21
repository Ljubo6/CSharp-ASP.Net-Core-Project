namespace DealerShip.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICarService
    {
        Task AddAsync(string make, string model, decimal price);
<<<<<<< HEAD

        Task<IEnumerable<T>> GetAllCars<T>();
=======
>>>>>>> 0556923ec7b4f1d4a099784c8d86103e1d13a6d5
    }
}
