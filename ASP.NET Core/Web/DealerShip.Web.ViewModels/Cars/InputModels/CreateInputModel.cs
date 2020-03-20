namespace DealerShip.Web.ViewModels.Cars.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CreateInputModel
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public decimal Price { get; set; }
    }
}
