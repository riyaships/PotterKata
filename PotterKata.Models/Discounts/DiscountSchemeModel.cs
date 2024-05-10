using PotterKata.Models.Enums;
using System.Collections.Generic;

namespace PotterKata.Models.Discounts
{
    public class DiscountSchemeModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DiscountSchemeTypeEnum DiscountSchemeType { get; set; }
        public List<DiscountSchemeDetailModel> DiscountSchemeDetails { get; set; }
    }


}
