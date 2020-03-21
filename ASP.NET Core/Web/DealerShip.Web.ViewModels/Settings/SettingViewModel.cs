namespace DealerShip.Web.ViewModels.Settings
{
<<<<<<< HEAD
    using AutoMapper;
    using DealerShip.Data.Models;
    using DealerShip.Services.Mapping;

=======
    using DealerShip.Data.Models;
    using DealerShip.Services.Mapping;

    using AutoMapper;

>>>>>>> 0556923ec7b4f1d4a099784c8d86103e1d13a6d5
    public class SettingViewModel : IMapFrom<Setting>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string NameAndValue { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Setting, SettingViewModel>().ForMember(
                m => m.NameAndValue,
                opt => opt.MapFrom(x => x.Name + " = " + x.Value));
        }
    }
}
