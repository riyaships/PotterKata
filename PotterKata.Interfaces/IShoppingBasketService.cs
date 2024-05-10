using PotterKata.Models.Discounts;
using PotterKata.Models.Shopping;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PotterKata.Interfaces
{
    public interface IShoppingBasketService
    {
        public Task<ShoppingBasketDiscountResponse> CalculateBasketDiscount(ShoppingBasket basket, int currentDiscountSchemeId);

    }
}
