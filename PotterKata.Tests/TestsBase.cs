using Moq;
using PotterKata.Interfaces;

namespace PotterKata.Tests
{
    public class TestsBase
    {
        protected IDemoDataService DemoDataService { get; set; }
        protected IDiscountSchemeService DiscountSchemeService { get; set; }
        protected IShoppingBasketService ShoppingBasketService { get; set; }
        protected Mock<IDataService> MockDataService { get; set; }

        protected int CurrentDiscountSchemeId = 1;
    }
}