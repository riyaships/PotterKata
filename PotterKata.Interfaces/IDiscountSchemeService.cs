using PotterKata.Models.Discounts;
using System.Threading.Tasks;

namespace PotterKata.Interfaces
{
    public interface IDiscountSchemeService
    {
        public Task<DiscountSchemeModel> GetDiscountSchemeById(int id);
    }
   
}
