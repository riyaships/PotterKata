using PotterKata.Models.Books;
using PotterKata.Models.Discounts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotterKata.Interfaces
{
    public interface IDataService
    {
        public Task<List<DiscountSchemeModel>> GetAllHarrysMagicDiscountSchemes();
        public Task<BookSeries> GetAllBooksForHarrysSeries();
    }
}
