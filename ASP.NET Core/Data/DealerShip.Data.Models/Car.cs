namespace DealerShip.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DealerShip.Data.Common.Models;

    public class Car : BaseDeletableModel<string>
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public decimal Price { get; set; }
    }
}
