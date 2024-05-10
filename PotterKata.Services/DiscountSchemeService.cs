using PotterKata.Interfaces;
using PotterKata.Models.Discounts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotterKata.Services
{
    public class DiscountSchemeService : IDiscountSchemeService
    {
        private readonly IDemoDataService DemoDataService;
        public DiscountSchemeService(IDemoDataService demoDataService)
        {
            DemoDataService = demoDataService;
        } 
        public async Task<DiscountSchemeModel> GetDiscountSchemeById(int id)
        {
            return (await DemoDataService.GetAllHarrysMagicDiscountSchemes()).Find(s => s.Id == id);
        }
    }
}
