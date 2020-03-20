namespace DealerShip.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DealerShip.Services.Data;
    using DealerShip.Web.ViewModels.Cars.InputModels;
    using Microsoft.AspNetCore.Mvc;

    public class CarsController : BaseController
    {
        private readonly ICarService carService;

        public CarsController(ICarService carService)
        {
            this.carService = carService;
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateInputModel inputModel)
        {
           await this.carService.AddAsync(inputModel.Make, inputModel.Make, inputModel.Price);

           return this.Redirect("/");
        }
    }
}
