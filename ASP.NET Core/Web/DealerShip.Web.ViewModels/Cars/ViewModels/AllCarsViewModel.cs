namespace DealerShip.Web.ViewModels.Cars.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DealerShip.Data.Models;
    using DealerShip.Services.Mapping;

    public class AllCarsViewModel : IMapFrom<Car>
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
