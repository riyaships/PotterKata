namespace PotterKata.Models.Discounts
{
    public class HarrysMagicDiscountSchemeModel
    {
        public HarrysMagicDiscountSchemeModel(int buyingQuantity, decimal discountPercentage)
        {
            BuyingQuantity = buyingQuantity;
            DiscountPercentage = discountPercentage;
        }
        public int BuyingQuantity { get; set; }

        public decimal DiscountPercentage { get; set; }
    }
}
