namespace DealerShip.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using DealerShip.Data.Common.Repositories;
    using DealerShip.Data.Models;

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
    }
}
