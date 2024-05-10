using PotterKata.Interfaces;
using PotterKata.Models.Books;
using PotterKata.Models.Discounts;
using PotterKata.Models.Enums;
using PotterKata.Models.Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotterKata.Services
{
    public class DemoDataService : IDemoDataService
    {
        public DemoDataService()
        {
        }
         
        public async Task<ShoppingBasket> GetDemoShoppingBasketDataWithUniqueItems(int numberOfUniqueItems)
        {
            //if (numberOfUniqueItems > 5) throw new NotImplementedException("Program not implemented support more than 5 items.");

            var bookSeries = await GetAllBooksForHarrysSeries();
            return new ShoppingBasket()
            {
                SessionId = Guid.NewGuid().ToString("N"),
                Items = bookSeries.Books.Take(numberOfUniqueItems).Select(s => new ShoppingBasketLineItem(s.Id)).ToList()
            };
        }

        public async Task<ShoppingBasket> GetDemoShoppingBasketDataWithBulkItems(params int[] quantities)
        {
            var bookSeries = await GetAllBooksForHarrysSeries();
            var result = new ShoppingBasket()
            {
                SessionId = Guid.NewGuid().ToString("N") 
            };
            for (var i=0;i< quantities.Length; i++)
            {
                var quantity = quantities[i];
                if (quantity == 0) continue;
                result.Items.Add(new ShoppingBasketLineItem(bookSeries.Books[i].Id, 8, quantity));
            }
            return result;
        }

        public async Task<List<DiscountSchemeModel>> GetAllHarrysMagicDiscountSchemes()
        {
            return await Task.Run(() =>
            {
                return new List<DiscountSchemeModel>()
                {
                new DiscountSchemeModel(){
                Id = 1,
                Title = "Harry’s magical powers promotion",
                DiscountSchemeType = DiscountSchemeTypeEnum.DiscountByDistinctItemCount,
                DiscountSchemeDetails = new List<DiscountSchemeDetailModel>()
                    {
                    new DiscountSchemeDetailModel(1, 1, 1, 1, 0.0m),
                    new DiscountSchemeDetailModel(2, 1, 2, 2, 5.0m),
                    new DiscountSchemeDetailModel(3, 1, 3, 3, 10.0m),
                    new DiscountSchemeDetailModel(4, 1, 4, 4, 20.0m),
                    new DiscountSchemeDetailModel(5, 1, 5, 5, 25.0m)
                    }
                }
            };
            });
        }

        public async Task<BookSeries> GetAllBooksForHarrysSeries()
        {
            return await Task.Run(() =>
            {
                return new BookSeries()
                {
                    Id = 1,
                    Title = "Potter",
                    Books = Enumerable.Range(1, 5).Select((i) => new Book(i, $"Book{i}", 1, 8.0m)).ToList()
                };
            });
        }
        public async Task<BookSeries> GetAllBooksForHarrysSeriesWithExtraBook()
        {
            return await Task.Run(() =>
            {
                return new BookSeries()
                {
                    Id = 1,
                    Title = "Potter",
                    Books = Enumerable.Range(1, 6).Select((i) => new Book(i, $"Book{i}", 1, 8.0m)).ToList()
                };
            });
        }

        public async Task<ShoppingBasket> GetDemoShoppingBasketDataWithSixUniqueBooks()
        {
            //if (numberOfUniqueItems > 5) throw new NotImplementedException("Program not implemented support more than 5 items.");

            var basket = await GetDemoShoppingBasketDataWithUniqueItems(5);
            basket.Items.Add(new ShoppingBasketLineItem(6));
            return basket;
        }
    }
}
