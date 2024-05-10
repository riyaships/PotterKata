using PotterKata.Interfaces;
using PotterKata.Models.Books;
using PotterKata.Models.Discounts;
using PotterKata.Models.Enums;
using PotterKata.Models.Extensions;
using PotterKata.Models.Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotterKata.Services
{
    public class ShoppingBasketService : IShoppingBasketService
    {
        private readonly IDataService DataService;

        public ShoppingBasketService(IDataService dataService)
        {
            DataService = dataService;
        }

        public async Task<ShoppingBasketDiscountResponse> CalculateBasketDiscount(ShoppingBasket basket, int currentDiscountSchemeId)
        {
            if (basket == null)
            {
                return new ShoppingBasketDiscountResponse(0, false, "Basket is unavailable");
            }

            var discountConfigurations = await DataService.GetAllHarrysMagicDiscountSchemes();
            if (discountConfigurations == null)
                return new ShoppingBasketDiscountResponse(0, false, "No discounts available");

            var activeDiscountScheme = discountConfigurations.Find(s => s.Id == currentDiscountSchemeId);
            if (activeDiscountScheme == null)
            {
                return new ShoppingBasketDiscountResponse(0, false, $"Please configure Harrys magical promotion with Id as {currentDiscountSchemeId}");
            }

            (decimal basketTotalAfterDiscounts, bool result, string message) = await CalculateBasketDiscountByDistinctItem(basket.Items, activeDiscountScheme);

            return new ShoppingBasketDiscountResponse(basketTotalAfterDiscounts, result, message);
        }

        private async Task<(decimal, bool, string)> CalculateBasketDiscountByDistinctItem(List<ShoppingBasketLineItem> items, DiscountSchemeModel discountScheme)
        {
            if (items == null || !items.Any()) return (0, false, "Basket is empty");

            var distinctProductsCount = items.Select(s => s.BookId).Distinct().Count();
            var applicableScheme = discountScheme.DiscountSchemeDetails.Find(s => s.BuyingQty == distinctProductsCount);
            if (applicableScheme == null) return (0, false, $"Discount not configured for {distinctProductsCount} distinct books");

            var totalQuantity = items.Sum(s => s.ItemQuantity);
            if (totalQuantity > 5)
            {
                return await CalculateMaximumDiscountByDistinctItemForBulkPurchase(items, discountScheme);
            }

            var discountPercent = applicableScheme.DiscountPercentage;

            var totalDiscountAmount = (items.Sum(s => s.Price) / 100.0m) * discountPercent;

            var basketTotalAfterDiscounts = (items.Sum(s => s.Price) - totalDiscountAmount).WithDecimalPlaces(2);

            return (basketTotalAfterDiscounts, true, "Success");
        }

        private async Task<(decimal, bool, string)> CalculateMaximumDiscountByDistinctItemForBulkPurchase(List<ShoppingBasketLineItem> items, DiscountSchemeModel discountScheme)
        {
            return await Task.Run(() =>
            {
                //Handling duplicate book lines for repeated items if not handled until reaching this point
                var basketItems = items.GroupBy(s => s.BookId).Select(g => new ShoppingBasketLineItem(g.Key, g.Max(s => s.Price), g.Sum(s => s.ItemQuantity)));

                var sortedBasketItems = basketItems.OrderBy(s => s.ItemQuantity).ToList();
                 
                var totalDiscountAmount = 0.0m;
                var singleBookPrice = items[0].Price;

                var totalBooksQty = items.Sum(s => s.ItemQuantity);


                while (totalBooksQty > 0)
                {
                    var minQty = sortedBasketItems.Min(s => s.ItemQuantity);
                    var booksWithMoreThanMinimumQty = sortedBasketItems.Where(s => s.ItemQuantity >= minQty);
                    var distinctBooks = booksWithMoreThanMinimumQty.Select(s => s.BookId).Distinct().Count();
                    var applicableScheme = discountScheme.DiscountSchemeDetails.Find(s => s.BuyingQty == distinctBooks);

                    var booksWithMinQty = sortedBasketItems.Where(s => s.ItemQuantity >= minQty);

                    var discountPercent = applicableScheme.DiscountPercentage;
                    var qualifyingQty = booksWithMinQty.Count() * minQty;
                    var discountAmount = (singleBookPrice / 100.0m) * discountPercent * qualifyingQty;
                    totalDiscountAmount += discountAmount;
                     
                    sortedBasketItems.RemoveAll(s=> s.ItemQuantity == minQty);
                    sortedBasketItems.ForEach(s => s.ItemQuantity -= minQty);

                    totalBooksQty = sortedBasketItems.Sum(s => s.ItemQuantity);
                }

                var basketTotalAfterDiscounts = (items.Sum(s => s.Price * s.ItemQuantity) - totalDiscountAmount).WithDecimalPlaces(2);
                return (basketTotalAfterDiscounts, true, "Success");
            });
        }

    }


}
