using PotterKata.Models.Books;

namespace PotterKata.Models.Shopping
{
    public class ShoppingBasketLineItem
    {
        public ShoppingBasketLineItem(int bookId, decimal price = 8, int itemQuantity = 1)
        {
            BookId = bookId;
            Price = price;
            ItemQuantity = itemQuantity;
        }
        public int BookId { get; set; }
        public int ItemQuantity { get; set; }
        public decimal Price { get; set; }
    }
    public class ShoppingBasketDiscountResponse
    {
        public ShoppingBasketDiscountResponse(decimal discountedBasketTotal, bool isSuccessful, string message)
        {
            DiscountedBasketTotal = discountedBasketTotal;
            IsSuccessful = isSuccessful;
            Message = message;
        }

        public decimal DiscountedBasketTotal { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
    }
}
