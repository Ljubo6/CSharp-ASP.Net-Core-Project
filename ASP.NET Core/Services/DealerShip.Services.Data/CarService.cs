namespace DealerShip.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using DealerShip.Data.Common.Repositories;
    using DealerShip.Data.Models;
    using DealerShip.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<T>> GetAllCars<T>()
        {
            return await this.carRepository
                .AllAsNoTracking()
                .To<T>()
                .ToArrayAsync();
        }
    }
}
