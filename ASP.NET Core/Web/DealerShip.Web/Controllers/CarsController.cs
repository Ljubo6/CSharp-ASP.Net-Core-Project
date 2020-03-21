namespace DealerShip.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DealerShip.Services.Data;
    using DealerShip.Web.ViewModels.Cars.InputModels;
    using DealerShip.Web.ViewModels.Cars.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class CarsController : BaseController
    {
        private readonly ICarService carService;

        public CarsController(ICarService carService)
        {
            this.carService = carService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateInputModel carInputModel)
        {
            await this.carService.AddAsync(carInputModel.Make, carInputModel.Model, carInputModel.Price);
            return this.Redirect("/");
        }

        public async Task<IActionResult> All()
        {
            var allCars = await this.carService.GetAllCars<AllCarsViewModel>();
            return this.View(allCars);
        }
    }
}
