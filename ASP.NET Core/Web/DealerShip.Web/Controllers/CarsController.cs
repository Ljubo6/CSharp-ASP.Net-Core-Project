namespace DealerShip.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DealerShip.Services.Data;
    using DealerShip.Web.ViewModels.Cars.InputModels;
<<<<<<< HEAD
    using DealerShip.Web.ViewModels.Cars.ViewModels;
=======
>>>>>>> 0556923ec7b4f1d4a099784c8d86103e1d13a6d5
    using Microsoft.AspNetCore.Mvc;

    public class CarsController : BaseController
    {
        private readonly ICarService carService;

        public CarsController(ICarService carService)
        {
            this.carService = carService;
        }

<<<<<<< HEAD
        [HttpGet]
=======
>>>>>>> 0556923ec7b4f1d4a099784c8d86103e1d13a6d5
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
<<<<<<< HEAD
        public async Task<IActionResult> Add(CreateInputModel carInputModel)
        {
            await this.carService.AddAsync(carInputModel.Make, carInputModel.Model, carInputModel.Price);
            return this.Redirect("/");
        }

        public async Task<IActionResult> All()
        {
            var allCars = await this.carService.GetAllCars<AllCarsViewModel>();
            return this.View(allCars);
=======
        public async Task<IActionResult> Add(CreateInputModel inputModel)
        {
           await this.carService.AddAsync(inputModel.Make, inputModel.Make, inputModel.Price);

           return this.Redirect("/");
>>>>>>> 0556923ec7b4f1d4a099784c8d86103e1d13a6d5
        }
    }
}
