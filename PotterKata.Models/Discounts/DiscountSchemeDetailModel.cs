namespace PotterKata.Models.Discounts
{
    public class DiscountSchemeDetailModel
    {
        public DiscountSchemeDetailModel()
        {
            
        }
        public DiscountSchemeDetailModel(int id, int? bookSeriesId = null, int? bookId = null, int buyingQty = 0, decimal discountPercentage = 0)
        {
            Id = id;
            BookSeriesId = bookSeriesId;
            BookId = bookId;
            BuyingQty = buyingQty;
            DiscountPercentage = discountPercentage;
        }
        public int Id { get; set; }
        public int? BookSeriesId { get; set; }
        public int? BookId { get; set; }
        public int BuyingQty { get; set; }
        public decimal DiscountPercentage { get; set; }
    }

}
