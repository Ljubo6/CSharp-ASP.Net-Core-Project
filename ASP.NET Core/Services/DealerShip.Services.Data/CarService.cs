namespace DealerShip.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using DealerShip.Data.Common.Repositories;
    using DealerShip.Data.Models;
<<<<<<< HEAD
    using DealerShip.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
=======
>>>>>>> 0556923ec7b4f1d4a099784c8d86103e1d13a6d5

    public class CarService : ICarService
    {
        private readonly IDeletableEntityRepository<Car> carRepository;

        public CarService(IDeletableEntityRepository<Car> carRepository)
        {
            this.carRepository = carRepository;
        }

        public async Task AddAsync(string make, string model, decimal price)
        {
            var car = new Car
            {
                Id = Guid.NewGuid().ToString(),
                Make = make,
                Model = model,
                Price = price,
            };

            await this.carRepository.AddAsync(car);
            await this.carRepository.SaveChangesAsync();
        }
<<<<<<< HEAD

        public async Task<IEnumerable<T>> GetAllCars<T>()
        {
            return await this.carRepository
                .AllAsNoTracking()
                .To<T>()
                .ToArrayAsync();
        }
=======
>>>>>>> 0556923ec7b4f1d4a099784c8d86103e1d13a6d5
    }
}
