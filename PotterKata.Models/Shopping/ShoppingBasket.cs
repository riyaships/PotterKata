using PotterKata.Models.Discounts;
using System;
using System.Collections.Generic;

namespace PotterKata.Models.Shopping
{
    public class ShoppingBasket
    {
        public string SessionId { get; set; } = Guid.NewGuid().ToString("N");
        public List<ShoppingBasketLineItem> Items { get; set; } = new List<ShoppingBasketLineItem>();
    }

}
