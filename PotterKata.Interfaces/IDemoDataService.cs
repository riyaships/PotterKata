using PotterKata.Models.Books;
using PotterKata.Models.Discounts;
using PotterKata.Models.Shopping;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PotterKata.Interfaces
{
    public interface IDemoDataService
    {
        public Task<ShoppingBasket> GetDemoShoppingBasketDataWithUniqueItems(int numberOfUniqueItems);

        Task<ShoppingBasket> GetDemoShoppingBasketDataWithBulkItems(params int[] quantities);
        public Task<List<DiscountSchemeModel>> GetAllHarrysMagicDiscountSchemes();
        public Task<BookSeries> GetAllBooksForHarrysSeries();
        public Task<ShoppingBasket> GetDemoShoppingBasketDataWithSixUniqueBooks();

    }
}
